using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSimulator.Core.Entities
{
    public sealed class Ship : Entity
    {
        public int Number { get; set; }
        public int Capacity { get; set; }
        public DateTime CreateDate { get; set; }
        public int MaxDistance { get; set; }
        public int TeamCount { get; set; }
        public int PortID { get; set; }

        public override string ToString()
        {
            return string.Format("{0}\t{1}\t{2}\t{3}/{4}/{5}\t{6}\t{7}\t{8}",
                ID, Number, Capacity,
                CreateDate.Month, CreateDate.Day, CreateDate.Year,
                MaxDistance, TeamCount, PortID);
        }
    }
}
