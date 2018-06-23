using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace CSharpTraceRoute.Models
{
    public class TraceRouteHop
    {
        private readonly PingReply _pingReply;
        private readonly IPHostEntry _hostEntry;

        public TraceRouteHop(PingReply pingReply)
        {
            _pingReply = pingReply ?? throw new ArgumentNullException(nameof(pingReply));

            if(_pingReply.Address != null)
            {
                try
                {
                    _hostEntry = Dns.GetHostEntry(_pingReply.Address);
                }
                catch (SocketException socketEx)
                {
                    Debug.WriteLine(socketEx);
                }
            }
        }

        public IPAddress IpAddress => _pingReply.Address;
        public bool IsTimedOut => _pingReply.Status.Equals(IPStatus.TimedOut);
        public double RoundtripTime => _pingReply.RoundtripTime;

        public bool HasIPHostEntry => _hostEntry != null;
        public string HostName => _hostEntry?.HostName ?? String.Empty;

        public override string ToString()
        {
            return $"[{nameof(TraceRouteHop)}] {nameof(IpAddress)}={IpAddress}; {nameof(HostName)}={HostName}; {nameof(RoundtripTime)}={RoundtripTime}";
        }
    }
}
