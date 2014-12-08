using System;
using System.Linq;

using PortSimulator.Core.Entities;

using PortSimulator.Application.Views.Abstractions;

namespace PortSimulator.Application.Views
{
    public sealed class CargoView : BaseView
    {
        #region BaseView
        public async override void CreateAsync()
        {
            await _dbManager.CargoRepository.Save(CreateEntity<Cargo>(null));
            OnUpdate();
        }

        public async override void UpdateAsync()
        {
            int id = SelectMenuItem(_catalog.Cargos);
            await _dbManager.CargoRepository.Save(CreateEntity<Cargo>(_catalog.Cargos[id]));
            OnUpdate();
        }

        public async override void DeleteAsync()
        {
            int id = SelectMenuItem(_catalog.Cargos);
            await _dbManager.CargoRepository.Delete(_catalog.Cargos[id].ID);
            OnUpdate();
        }

        public override void Show()
        {
            try
            {
                int id = int.Parse(Console.ReadLine());
                Console.WriteLine("ID\tNumber\tWeight\tPrice\tIPrice\tCTypeID\tTripID");
                Console.WriteLine(_catalog.Cargos.First(x => x.ID == id));
            }
            catch (Exception e)
            {
                Console.WriteLine("Any column current id.");
            }
        }

        public override void ShowAll()
        {
            Console.WriteLine("ID\tNumber\tWeight\tPrice\tIPrice\tCTypeID\tTripID");
            foreach (var trip in _catalog.Cargos)
            {
                Console.WriteLine(trip);
            }
        }
        #endregion
    }
}
