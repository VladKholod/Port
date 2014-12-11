using System;
using System.Linq;
using PortSimulator.Application.Views.Abstractions;
using PortSimulator.Core.Entities;

namespace PortSimulator.Application.Views
{
    public sealed class CargoTypeView : BaseView
    {
        #region BaseView
        public async override void CreateAsync()
        {
            await DbManager.CargoTypeRepository.Save(CreateEntity<CargoType>(null));
            OnUpdate();
        }

        public async override void UpdateAsync()
        {
            var id = SelectMenuItem(Catalog.Cargos);
            await DbManager.CargoTypeRepository.Save(CreateEntity<CargoType>(Catalog.CargoTypes[id]));
            OnUpdate();
        }

        public async override void DeleteAsync()
        {
            var id = SelectMenuItem(Catalog.CargoTypes);
            await DbManager.CargoTypeRepository.Delete(Catalog.CargoTypes[id].Id);
            OnUpdate();
        }

        public override void Show()
        {
            Console.WriteLine("ID\tName");
            try
            {
                var id = int.Parse(Console.ReadLine());
                Console.WriteLine(Catalog.CargoTypes.First(x => x.Id == id));
            }
            catch (Exception)
            {
                Console.WriteLine("Any column current id.");
            }
        }

        public override void ShowAll()
        {
            Console.WriteLine("ID\tName");
            foreach (var trip in Catalog.CargoTypes)
            {
                Console.WriteLine(trip);
            }
        }
        #endregion
    }
}
