using System;
using System.Linq;
using PortSimulator.Application.Views.Abstractions;
using PortSimulator.Core.Entities;

namespace PortSimulator.Application.Views
{
    public sealed class CargoView : BaseView
    {
        #region BaseView
        public async override void CreateAsync()
        {
            await DbManager.CargoRepository.Save(CreateEntity<Cargo>(null));
            OnUpdate();
        }

        public async override void UpdateAsync()
        {
            var id = SelectMenuItem(Catalog.Cargos);
            await DbManager.CargoRepository.Save(CreateEntity<Cargo>(Catalog.Cargos[id]));
            OnUpdate();
        }

        public async override void DeleteAsync()
        {
            var id = SelectMenuItem(Catalog.Cargos);
            await DbManager.CargoRepository.Delete(Catalog.Cargos[id].Id);
            OnUpdate();
        }

        public override void Show()
        {
            try
            {
                var id = int.Parse(Console.ReadLine());
                Console.WriteLine("ID\tNumber\tWeight\tPrice\tIPrice\tCTypeID\tTripID");
                Console.WriteLine(Catalog.Cargos.First(x => x.Id == id));
            }
            catch (Exception)
            {
                Console.WriteLine("Any column current id.");
            }
        }

        public override void ShowAll()
        {
            Console.WriteLine("ID\tNumber\tWeight\tPrice\tIPrice\tCTypeID\tTripID");
            foreach (var trip in Catalog.Cargos)
            {
                Console.WriteLine(trip);
            }
        }
        #endregion
    }
}
