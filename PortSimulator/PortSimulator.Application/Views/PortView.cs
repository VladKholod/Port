using System;
using System.Linq;

using PortSimulator.Core.Entities;

using PortSimulator.Application.Views.Abstractions;

namespace PortSimulator.Application.Views
{
    public sealed class PortView : BaseView
    {
        #region BaseView
        public async override void CreateAsync()
        {
            await _dbManager.PortRepository.Save(CreateEntity<Port>(null));
            OnUpdate();
        }

        public async override void UpdateAsync()
        {
            int id = SelectMenuItem(_catalog.Ports);
            await _dbManager.PortRepository.Save(CreateEntity<Port>(_catalog.Ports[id]));
            OnUpdate();
        }

        public async override void DeleteAsync()
        {
            int id = SelectMenuItem(_catalog.Ports);
            await _dbManager.PortRepository.Delete(_catalog.Ports[id].ID);
            OnUpdate();
        }

        public override void Show()
        {
            Console.WriteLine("ID\tName\tCityID");
            try
            {
                int id = int.Parse(Console.ReadLine());
                Console.WriteLine(_catalog.Ports.First(x => x.ID == id));
            }
            catch (Exception e)
            {
                Console.WriteLine("Any column current id.");
            }
        }

        public override void ShowAll()
        {
            Console.WriteLine("ID\tName\tCityID");
            foreach (var trip in _catalog.Ports)
            {
                Console.WriteLine(trip);
            }
        }
        #endregion
    }
}
