using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

using PortSimulator.DatabaseManager.Repositories;

namespace PortSimulator.DatabaseManager
{
    public sealed class DbManager
    {
        public CaptainRepository CaptainRepository = new CaptainRepository();
        public CargoRepository CargoRepository = new CargoRepository();
        public CargoTypeRepository CargoTypeRepository = new CargoTypeRepository();
        public CityRepository CityRepository = new CityRepository();
        public PortRepository PortRepository = new PortRepository();
        public ShipRepository ShipRepository = new ShipRepository();
        public TripRepository TripRepository = new TripRepository();
    }
}
