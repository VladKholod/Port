using System;

namespace PortSimulator.Core.Entities
{
    public sealed class Trip : Entity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ShipId { get; set; }
        public int CaptainId { get; set; }
        public int PortFromId { get; set; }
        public int PortToId { get; set; }

        public override string ToString()
        {
            return string.Format("{0}\t{1}/{2}/{3}\t{4}/{5}/{6}\t{7}\t{8}\t{9}\t{10}",
                Id,
                StartDate.Month, StartDate.Day, StartDate.Year,
                EndDate.Month, EndDate.Day, EndDate.Year,
                ShipId, CaptainId, PortFromId, PortToId);
        }
    }
}
