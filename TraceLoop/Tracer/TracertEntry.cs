using System.Net.NetworkInformation;

namespace TraceLoop.Tracer
{
    public class TracertEntry
    {
        public int HopID { get; set; }

        public string Address { get; set; }

        public string Hostname { get; set; }

        public long ReplyTime { get; set; }

        public IPStatus ReplyStatus { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(HopID)}: {HopID}, {nameof(Address)}: {Address}, {nameof(Hostname)}: {Hostname}, {nameof(ReplyTime)}: {ReplyTime}, {nameof(ReplyStatus)}: {ReplyStatus}";
        }
    }
}