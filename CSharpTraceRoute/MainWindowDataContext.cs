using CSharpTraceRoute.NetworkInformation;
using CSharpTraceRoute.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace CSharpTraceRoute
{
    public class MainWindowDataContext
    {
        private readonly ObservableCollection<TraceRouteHopItemViewModel> _hops = new ObservableCollection<TraceRouteHopItemViewModel>();
        private readonly BackgroundWorker _traceRouteWorker = new BackgroundWorker();

        public MainWindowDataContext()
        {
            _traceRouteWorker.WorkerReportsProgress = true;
            _traceRouteWorker.DoWork += TraceRouteWorker_DoWork;
            _traceRouteWorker.ProgressChanged += TraceRouteWorker_ProgressChanged;
        }

        public string HostName { get; set; }
        public bool IsTracingRoute => _traceRouteWorker.IsBusy;
        public IEnumerable<TraceRouteHopItemViewModel> TraceRouteHops => _hops;

        public void StartTraceRoute()
        {
            if(_traceRouteWorker.IsBusy)
            {
                return;
            }

            if(string.IsNullOrEmpty(HostName))
            {
                return;
            }

            _hops.Clear();
            _traceRouteWorker.RunWorkerAsync(HostName);
        }

        private void TraceRouteWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker thisWorker = (BackgroundWorker)sender;

            string hostName = (string)e.Argument;
            TraceRoute traceRouter = new TraceRoute(3000, 30);

            try
            {
                foreach (var reply in traceRouter.Send(hostName))
                {
                    thisWorker.ReportProgress(0, reply);
                }
            }
            catch (PingException pingEx)
            {
                Debug.WriteLine(pingEx);
            }
        }

        private void TraceRouteWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            PingReply reply = (PingReply)e.UserState;
            _hops.Add(new TraceRouteHopItemViewModel(new Models.TraceRouteHop(reply)));
        }

    }
}
