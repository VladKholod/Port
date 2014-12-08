using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;

using PortSimulator.Core;
using PortSimulator.Core.Entities;

using PortSimulator.DatabaseManager;
using PortSimulator.DatabaseManager.Repositories;

using PortSimulator.Application.Views;
using PortSimulator.Application.Views.Abstractions;

namespace PortSimulator.Application.Views.Abstractions
{
    public abstract class BaseView
    {
        public delegate void UpdateEventHandler();

        public UpdateEventHandler OnUpdate;

        protected Catalog _catalog = new Catalog();
        protected DbManager _dbManager = new DbManager();

        public BaseView()
        {
            OnUpdate += UpdateCatalog;
            OnUpdate();
        }

        public abstract void CreateAsync();
        public abstract void UpdateAsync();
        public abstract void DeleteAsync();
        public abstract void Show();
        public abstract void ShowAll();

        protected T CreateEntity<T>(Entity entity) where T : Entity
        {
            Type type = typeof(T);

            Console.WriteLine(type.Name + ":");

            object instance = entity;

            if(entity==null)
                instance = Activator.CreateInstance(type);
             

            foreach (PropertyInfo property in type.GetProperties())
            {
                if (property.Name.Equals("ID"))
                {
                    continue;
                }

                bool flag = true;
                while (flag)
                {
                    try
                    {
                        Console.Write("{0} : ", property.Name);
                        string value = Console.ReadLine();
                        if (value != string.Empty)
                        {
                            var converter = TypeDescriptor.GetConverter(property.PropertyType);
                            var result = converter.ConvertFrom(value);
                            property.SetValue(instance, result);
                        }

                        flag = false;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Invalid value");
                    }
                }
            }

            return (T)instance;
        }

        public void UpdateCatalog() 
        {
            _catalog.Captains = _dbManager.CaptainRepository.GetAll();
            _catalog.Cargos = _dbManager.CargoRepository.GetAll();
            _catalog.CargoTypes = _dbManager.CargoTypeRepository.GetAll();
            _catalog.Cities = _dbManager.CityRepository.GetAll();
            _catalog.Ports = _dbManager.PortRepository.GetAll();
            _catalog.Ships = _dbManager.ShipRepository.GetAll();
            _catalog.Trips = _dbManager.TripRepository.GetAll();
        }

        protected int SelectMenuItem<T>(List<T> entities)
        {
            Console.CursorVisible = false;
            int currentMenuItem = 0;

            List<string> items = new List<string>();

            foreach (var item in entities) 
            {
                items.Add(item.ToString());
            }

            while (true)
            {
                DisplayItems(items, currentMenuItem);

                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow)
                {
                    if (currentMenuItem > 0)
                        currentMenuItem--;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    if (currentMenuItem < items.Count - 1)
                        currentMenuItem++;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    return currentMenuItem;
                }
            }
        }

        protected void DisplayItems(List<string> items, int selectedMenuItem)
        {
            Console.Clear();
            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine("{0} {1}", i == selectedMenuItem ? "    >\t" : "\t", items[i]);
            }

            DisplayHelp();
        }

        protected void DisplayHelp()
        {
            Console.SetCursorPosition(0, Console.WindowHeight - 4);
            Console.WriteLine("Press <Enter> to select menu item.");
            Console.WriteLine("Press <Backspace> to return to previous submenu.");
        }
    }
}
