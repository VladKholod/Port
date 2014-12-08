using System;
using System.Linq;

using PortSimulator.Core.Entities;

using PortSimulator.Application.Views.Abstractions;

namespace PortSimulator.Application.Views
{
    public sealed class CaptainView : BaseView
    {
        #region BaseView
        public async override void CreateAsync()
        {
            await _dbManager.CaptainRepository.Save(CreateEntity<Captain>(null));
            OnUpdate();
        }

        public async override void UpdateAsync()
        {
            int id = SelectMenuItem(_catalog.Captains);
            await _dbManager.CaptainRepository.Save(CreateEntity<Captain>(_catalog.Captains[id]));
            OnUpdate();
        }

        public async override void DeleteAsync()
        {
            int id = SelectMenuItem(_catalog.Captains);
            await _dbManager.CaptainRepository.Delete(_catalog.Captains[id].ID);
            OnUpdate();
        }

        public override void Show()
        {
            try
            {
                int id = int.Parse(Console.ReadLine());
                Console.WriteLine("ID\tFName\tLName");
                Console.WriteLine(_catalog.Captains.First(x => x.ID == id));
            }
            catch (Exception e)
            {
                Console.WriteLine("Any column with current id.");
            }
        }

        public override void ShowAll()
        {
            Console.WriteLine("ID\tFName\tLName");
            foreach (var trip in _catalog.Captains)
            {
                Console.WriteLine(trip);
            }
        }
        #endregion
    }
}
