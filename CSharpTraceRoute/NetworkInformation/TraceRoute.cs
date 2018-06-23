using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace CSharpTraceRoute.NetworkInformation
{
    public class TraceRoute
    {
        private readonly int _timeout;
        private readonly int _maxTTL;
        private readonly int _messageSize;

        public TraceRoute(int timeout, int maxTTL, int messageSize = 32)
        {
            _timeout = timeout;
            _maxTTL = maxTTL;
            _messageSize = messageSize;
        }

        public IEnumerable<PingReply> Send(string hostName)
        {
            // Generate random message.
            byte[] message = new byte[_messageSize];
            new Random().NextBytes(message);
            
            using (Ping pinger = new Ping())
            {
                PingOptions options = new PingOptions(1, true);

                bool continueTraceLoop = true;
                for (int ttl = 1; continueTraceLoop && ttl < _maxTTL; ttl++)
                {
                    options.Ttl = ttl;
                    PingReply reply = pinger.Send(hostName, _timeout, message, options);

                    switch (reply.Status)
                    {
                        case IPStatus.Success:
                            yield return reply;
                            continueTraceLoop = false;
                            break;
                        case IPStatus.TimedOut:
                            yield return reply;
                            break;
                        case IPStatus.TtlExpired:
                            yield return reply;
                            break;
                        default:
                            continueTraceLoop = false;
                            break;
                    } // switch
                } // for
            } // using
        }
    }
}
