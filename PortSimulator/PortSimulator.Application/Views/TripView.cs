using System;
using System.Linq;
using PortSimulator.Application.Views.Abstractions;
using PortSimulator.Core.Entities;

namespace PortSimulator.Application.Views
{
    public sealed class TripView : BaseView
    {
        #region BaseView
        public async override void CreateAsync()
        {
            await DbManager.TripRepository.Save(CreateEntity<Trip>(null));
            OnUpdate();
        }

        public async override void UpdateAsync()
        {
            var id = SelectMenuItem(Catalog.Trips);
            await DbManager.TripRepository.Save(CreateEntity<Trip>(Catalog.Trips[id]));
            OnUpdate();
        }

        public async override void DeleteAsync()
        {
            var id = SelectMenuItem(Catalog.Trips);
            await DbManager.TripRepository.Delete(Catalog.Trips[id].Id);
            OnUpdate();
        }

        public override void Show()
        {
            try
            {
                var id = int.Parse(Console.ReadLine());
                Console.WriteLine("ID\tStartDate\tEndDate\tShipID\tCapID\tPFromID\tPToID");
                Console.WriteLine(Catalog.Trips.First(x => x.Id == id));
            }
            catch (Exception)
            {
                Console.WriteLine("Any column current id.");
            }
        }

        public override void ShowAll()
        {
            Console.WriteLine("ID\tStartDate\tEndDate\t\tShipID\tCapID\tPFromID\tPToID");
            foreach (var trip in Catalog.Trips)
            {
                Console.WriteLine(trip);
            }
        }
        #endregion
    }
}
