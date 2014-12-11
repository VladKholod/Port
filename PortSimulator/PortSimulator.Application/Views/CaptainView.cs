using System;
using System.Linq;
using PortSimulator.Application.Views.Abstractions;
using PortSimulator.Core.Entities;

namespace PortSimulator.Application.Views
{
    public sealed class CaptainView : BaseView
    {
        #region BaseView
        public async override void CreateAsync()
        {
            await DbManager.CaptainRepository.Save(CreateEntity<Captain>(null));
            OnUpdate();
        }

        public async override void UpdateAsync()
        {
            var id = SelectMenuItem(Catalog.Captains);
            await DbManager.CaptainRepository.Save(CreateEntity<Captain>(Catalog.Captains[id]));
            OnUpdate();
        }

        public async override void DeleteAsync()
        {
            var id = SelectMenuItem(Catalog.Captains);
            await DbManager.CaptainRepository.Delete(Catalog.Captains[id].Id);
            OnUpdate();
        }

        public override void Show()
        {
            try
            {
                var id = int.Parse(Console.ReadLine());
                Console.WriteLine("ID\tFName\tLName");
                Console.WriteLine(Catalog.Captains.First(x => x.Id == id));
            }
            catch (Exception)
            {
                Console.WriteLine("Any column with current id.");
            }
        }

        public override void ShowAll()
        {
            Console.WriteLine("ID\tFName\tLName");
            foreach (var trip in Catalog.Captains)
            {
                Console.WriteLine(trip);
            }
        }
        #endregion
    }
}
