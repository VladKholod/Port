using System;
using System.Linq;

using PortSimulator.Core.Entities;

using PortSimulator.Application.Views.Abstractions;

namespace PortSimulator.Application.Views
{
    public sealed class CargoTypeView : BaseView
    {
        #region BaseView
        public async override void CreateAsync()
        {
            await _dbManager.CargoTypeRepository.Save(CreateEntity<CargoType>(null));
            OnUpdate();
        }

        public async override void UpdateAsync()
        {
            int id = SelectMenuItem(_catalog.Cargos);
            await _dbManager.CargoTypeRepository.Save(CreateEntity<CargoType>(_catalog.CargoTypes[id]));
            OnUpdate();
        }

        public async override void DeleteAsync()
        {
            int id = SelectMenuItem(_catalog.CargoTypes);
            await _dbManager.CargoTypeRepository.Delete(_catalog.CargoTypes[id].ID);
            OnUpdate();
        }

        public override void Show()
        {
            Console.WriteLine("ID\tName");
            try
            {
                int id = int.Parse(Console.ReadLine());
                Console.WriteLine(_catalog.CargoTypes.First(x => x.ID == id));
            }
            catch (Exception e)
            {
                Console.WriteLine("Any column current id.");
            }
        }

        public override void ShowAll()
        {
            Console.WriteLine("ID\tName");
            foreach (var trip in _catalog.CargoTypes)
            {
                Console.WriteLine(trip);
            }
        }
        #endregion
    }
}
