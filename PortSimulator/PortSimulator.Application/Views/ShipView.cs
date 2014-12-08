using System;
using System.Linq;

using PortSimulator.Core.Entities;

using PortSimulator.Application.Views.Abstractions;

namespace PortSimulator.Application.Views
{
    public sealed class ShipView : BaseView
    {
        #region BaseView
        public async override void CreateAsync()
        {
            await _dbManager.ShipRepository.Save(CreateEntity<Ship>(null));
            OnUpdate();
        }

        public async override void UpdateAsync()
        {
            int id = SelectMenuItem(_catalog.Ships);
            await _dbManager.ShipRepository.Save(CreateEntity<Ship>(_catalog.Ships[id]));
            OnUpdate();
        }

        public async override void DeleteAsync()
        {
            int id = SelectMenuItem(_catalog.Ships);
            await _dbManager.ShipRepository.Delete(_catalog.Ships[id].ID);
            OnUpdate();
        }

        public override void Show()
        {
            
            try
            {
                int id = int.Parse(Console.ReadLine());
                Console.WriteLine("ID\tNumber\tCapa\tCreateDate\tMaxDist\tTCount\tPortID");
                Console.WriteLine(_catalog.Ships.First(x => x.ID == id));
            }
            catch (Exception e)
            {
                Console.WriteLine("Any column whit current id.");
            }
        }

        public override void ShowAll()
        {
            Console.WriteLine("ID\tNumber\tCapa\tCreateDate\tMaxDist\tTCount\tPortID");
            foreach (var trip in _catalog.Ships)
            {
                Console.WriteLine(trip);
            }
        }
        #endregion
    }
}
