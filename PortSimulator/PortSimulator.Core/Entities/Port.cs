using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSimulator.Core.Entities
{
    public sealed class Port : Entity
    {
        public string Name { get; set; }
        public int CityID { get; set; }

        public override string ToString()
        {
            return string.Format("{0}\t{1}\t{2}",
                ID, Name, CityID);
        }
    }
}
