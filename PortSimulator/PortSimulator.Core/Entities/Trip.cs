using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSimulator.Core.Entities
{
    public sealed class Trip : Entity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ShipID { get; set; }
        public int CaptainID { get; set; }
        public int PortFromID { get; set; }
        public int PortToID { get; set; }

        public override string ToString()
        {
            return string.Format("{0}\t{1}/{2}/{3}\t{4}/{5}/{6}\t{7}\t{8}\t{9}\t{10}",
                ID,
                StartDate.Month, StartDate.Day, StartDate.Year,
                EndDate.Month, EndDate.Day, EndDate.Year,
                ShipID, CaptainID, PortFromID, PortToID);
        }
    }
}
