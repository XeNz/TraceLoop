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
    }
}