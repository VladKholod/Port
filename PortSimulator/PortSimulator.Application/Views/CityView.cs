using System;
using System.Linq;
using PortSimulator.Application.Views.Abstractions;
using PortSimulator.Core.Entities;

namespace PortSimulator.Application.Views
{
    public sealed class CityView : BaseView
    {
        #region BaseView
        public async override void CreateAsync()
        {
            await DbManager.CityRepository.Save(CreateEntity<City>(null));
            OnUpdate();
        }

        public async override void UpdateAsync()
        {
            var id = SelectMenuItem(Catalog.Cities);
            await DbManager.CityRepository.Save(CreateEntity<City>(Catalog.Cities[id]));
            OnUpdate();
        }

        public async override void DeleteAsync()
        {
            var id = SelectMenuItem(Catalog.Cities);
            await DbManager.CityRepository.Delete(Catalog.Cities[id].Id);
            OnUpdate();
        }

        public override void Show()
        {
            Console.WriteLine("ID\tName");
            try
            {
                var id = int.Parse(Console.ReadLine());
                Console.WriteLine(Catalog.Cities.First(x => x.Id == id));
            }
            catch (Exception)
            {
                Console.WriteLine("Any column with current id.");
            }
        }

        public override void ShowAll()
        {
            Console.WriteLine("ID\tName");
            foreach (var trip in Catalog.Cities)
            {
                Console.WriteLine(trip);
            }
        }
        #endregion
    }
}
