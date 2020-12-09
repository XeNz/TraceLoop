using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace TraceLoop.Tracer
{
    public static class TraceExecutor
    {
        /// <summary>
        /// Traces the route which data have to travel through in order to reach an IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address of the destination.</param>
        /// <param name="maxHops">Max hops to be returned.</param>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<TracertEntry> Tracert(string ipAddress, int maxHops, int timeout)
        {
            if (!IPAddress.TryParse(ipAddress, out var address))
                throw new ArgumentException($"{ipAddress} is not a valid IP address.");

            if (maxHops < 1)
                throw new ArgumentException("Max hops can't be lower than 1.");

            if (timeout < 1)
                throw new ArgumentException("Timeout value must be higher than 0.");
            var ping = new Ping();
            var pingOptions = new PingOptions(1, true);
            var pingReplyTime = new Stopwatch();

            PingReply reply;
            do
            {
                pingReplyTime.Start();
                reply = ping.Send(address, timeout, new byte[] {0}, pingOptions);
                pingReplyTime.Stop();
                var hostname = string.Empty;
                if (reply.Address != null)
                {
                    try
                    {
                        hostname = Dns.GetHostByAddress(reply.Address).HostName; // Retrieve the hostname for the replied address.
                    }
                    catch (SocketException)
                    {
                        // do nothing
                    }
                }

                yield return new TracertEntry
                {
                    HopID = pingOptions.Ttl,
                    Address = reply.Address.ToString(),
                    Hostname = string.IsNullOrWhiteSpace(hostname) ? "N/A" : hostname,
                    ReplyTime = pingReplyTime.ElapsedMilliseconds,
                    ReplyStatus = reply.Status
                };

                pingOptions.Ttl++;
                pingReplyTime.Reset();
            } while (reply.Status != IPStatus.Success && pingOptions.Ttl <= maxHops);
        }
    }
}