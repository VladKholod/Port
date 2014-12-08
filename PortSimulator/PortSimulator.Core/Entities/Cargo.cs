using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSimulator.Core.Entities
{
    public sealed class Cargo : Entity
    {
        public int Number { get; set; }
        public int Weight { get; set; }
        public int Price { get; set; }
        public int InsurancePrice { get; set; }
        public int CargoTypeID { get; set; }
        public int TripID { get; set; }

        public override string ToString()
        {
            return string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}",
                ID, Number, Weight, Price, InsurancePrice, CargoTypeID, TripID);
        }
    }
}
