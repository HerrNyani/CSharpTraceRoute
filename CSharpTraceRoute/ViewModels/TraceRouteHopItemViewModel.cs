using CSharpTraceRoute.Models;
using System;

namespace CSharpTraceRoute.ViewModels
{
    public class TraceRouteHopItemViewModel
    {
        private readonly TraceRouteHop _hop;

        public TraceRouteHopItemViewModel(TraceRouteHop hop)
        {
            _hop = hop ?? throw new ArgumentNullException(nameof(hop));
        }
        
        public string IPAddress => _hop.IpAddress?.ToString() ?? "Unknown";
        public string HostName => _hop.HostName;
        public bool IsTimedOut => _hop.IsTimedOut;
        public double RoundtripTime => _hop.RoundtripTime;
        public string RoundtripStatus => IsTimedOut ? "Timeout" : $"OK: {_hop.RoundtripTime} ms";
    }
}
