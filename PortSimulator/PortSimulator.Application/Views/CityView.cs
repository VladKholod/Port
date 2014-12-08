using System;
using System.Linq;

using PortSimulator.Core.Entities;

using PortSimulator.Application.Views.Abstractions;

namespace PortSimulator.Application.Views
{
    public sealed class CityView : BaseView
    {
        #region BaseView
        public async override void CreateAsync()
        {
            await _dbManager.CityRepository.Save(CreateEntity<City>(null));
            OnUpdate();
        }

        public async override void UpdateAsync()
        {
            int id = SelectMenuItem(_catalog.Cities);
            await _dbManager.CityRepository.Save(CreateEntity<City>(_catalog.Cities[id]));
            OnUpdate();
        }

        public async override void DeleteAsync()
        {
            int id = SelectMenuItem(_catalog.Cities);
            await _dbManager.CityRepository.Delete(_catalog.Cities[id].ID);
            OnUpdate();
        }

        public override void Show()
        {
            Console.WriteLine("ID\tName");
            try
            {
                int id = int.Parse(Console.ReadLine());
                Console.WriteLine(_catalog.Cities.First(x => x.ID == id));
            }
            catch (Exception e)
            {
                Console.WriteLine("Any column with current id.");
            }
        }

        public override void ShowAll()
        {
            Console.WriteLine("ID\tName");
            foreach (var trip in _catalog.Cities)
            {
                Console.WriteLine(trip);
            }
        }
        #endregion
    }
}
