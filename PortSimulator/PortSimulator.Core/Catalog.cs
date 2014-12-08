using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PortSimulator.Core.Entities;

namespace PortSimulator.Core
{
    public sealed class Catalog
    {
        public List<Captain> Captains { get; set; }
        public List<Cargo> Cargos { get; set; }
        public List<CargoType> CargoTypes { get; set; }
        public List<City> Cities { get; set; }
        public List<Port> Ports { get; set; }
        public List<Ship> Ships { get; set; }
        public List<Trip> Trips { get; set; }

        public Catalog()
        { 
        }
    }
}
