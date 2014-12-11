using System;
using System.Linq;
using PortSimulator.Application.Views.Abstractions;
using PortSimulator.Core.Entities;

namespace PortSimulator.Application.Views
{
    public sealed class ShipView : BaseView
    {
        #region BaseView
        public async override void CreateAsync()
        {
            await DbManager.ShipRepository.Save(CreateEntity<Ship>(null));
            OnUpdate();
        }

        public async override void UpdateAsync()
        {
            var id = SelectMenuItem(Catalog.Ships);
            await DbManager.ShipRepository.Save(CreateEntity<Ship>(Catalog.Ships[id]));
            OnUpdate();
        }

        public async override void DeleteAsync()
        {
            var id = SelectMenuItem(Catalog.Ships);
            await DbManager.ShipRepository.Delete(Catalog.Ships[id].Id);
            OnUpdate();
        }

        public override void Show()
        {
            
            try
            {
                var id = int.Parse(Console.ReadLine());
                Console.WriteLine("ID\tNumber\tCapa\tCreateDate\tMaxDist\tTCount\tPortID");
                Console.WriteLine(Catalog.Ships.First(x => x.Id == id));
            }
            catch (Exception)
            {
                Console.WriteLine("Any column whit current id.");
            }
        }

        public override void ShowAll()
        {
            Console.WriteLine("ID\tNumber\tCapa\tCreateDate\tMaxDist\tTCount\tPortID");
            foreach (var trip in Catalog.Ships)
            {
                Console.WriteLine(trip);
            }
        }
        #endregion
    }
}
