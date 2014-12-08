using System;
using System.Linq;

using PortSimulator.Core.Entities;

using PortSimulator.Application.Views.Abstractions;

namespace PortSimulator.Application.Views
{
    public sealed class TripView : BaseView
    {
        #region BaseView
        public async override void CreateAsync()
        {
            await _dbManager.TripRepository.Save(CreateEntity<Trip>(null));
            OnUpdate();
        }

        public async override void UpdateAsync()
        {
            int id = SelectMenuItem(_catalog.Trips);
            await _dbManager.TripRepository.Save(CreateEntity<Trip>(_catalog.Trips[id]));
            OnUpdate();
        }

        public async override void DeleteAsync()
        {
            int id = SelectMenuItem(_catalog.Trips);
            await _dbManager.TripRepository.Delete(_catalog.Trips[id].ID);
            OnUpdate();
        }

        public override void Show()
        {
            try
            {
                int id = int.Parse(Console.ReadLine());
                Console.WriteLine("ID\tStartDate\tEndDate\tShipID\tCapID\tPFromID\tPToID");
                Console.WriteLine(_catalog.Trips.First(x => x.ID == id));
            }
            catch (Exception e)
            {
                Console.WriteLine("Any column current id.");
            }
        }

        public override void ShowAll()
        {
            Console.WriteLine("ID\tStartDate\tEndDate\t\tShipID\tCapID\tPFromID\tPToID");
            foreach (var trip in _catalog.Trips)
            {
                Console.WriteLine(trip);
            }
        }
        #endregion
    }
}
