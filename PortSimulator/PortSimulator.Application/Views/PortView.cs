using System;
using System.Linq;
using PortSimulator.Application.Views.Abstractions;
using PortSimulator.Core.Entities;

namespace PortSimulator.Application.Views
{
    public sealed class PortView : BaseView
    {
        #region BaseView
        public async override void CreateAsync()
        {
            await DbManager.PortRepository.Save(CreateEntity<Port>(null));
            OnUpdate();
        }

        public async override void UpdateAsync()
        {
            var id = SelectMenuItem(Catalog.Ports);
            await DbManager.PortRepository.Save(CreateEntity<Port>(Catalog.Ports[id]));
            OnUpdate();
        }

        public async override void DeleteAsync()
        {
            var id = SelectMenuItem(Catalog.Ports);
            await DbManager.PortRepository.Delete(Catalog.Ports[id].Id);
            OnUpdate();
        }

        public override void Show()
        {
            Console.WriteLine("ID\tName\tCityID");
            try
            {
                var id = int.Parse(Console.ReadLine());
                Console.WriteLine(Catalog.Ports.First(x => x.Id == id));
            }
            catch (Exception)
            {
                Console.WriteLine("Any column current id.");
            }
        }

        public override void ShowAll()
        {
            Console.WriteLine("ID\tName\tCityID");
            foreach (var trip in Catalog.Ports)
            {
                Console.WriteLine(trip);
            }
        }
        #endregion
    }
}
