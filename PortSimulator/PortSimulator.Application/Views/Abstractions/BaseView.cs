using System;
using System.Collections.Generic;
using System.ComponentModel;
using PortSimulator.Core;
using PortSimulator.Core.Entities;
using PortSimulator.DatabaseManager;

namespace PortSimulator.Application.Views.Abstractions
{
    public delegate void UpdateEventHandler();

    public abstract class BaseView
    {
        protected readonly UpdateEventHandler OnUpdate;

        protected readonly Catalog Catalog = new Catalog();
        protected readonly DbManager DbManager = new DbManager();

        protected BaseView()
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
            var type = typeof(T);

            Console.WriteLine(type.Name + ":");

            object instance = entity;

            if(entity==null)
                instance = Activator.CreateInstance(type);
             

            foreach (var property in type.GetProperties())
            {
                if (property.Name.Equals("Id"))
                {
                    continue;
                }

                var flag = true;
                while (flag)
                {
                    try
                    {
                        Console.Write("{0} : ", property.Name);
                        var value = Console.ReadLine();
                        if (value != string.Empty)
                        {
                            var converter = TypeDescriptor.GetConverter(property.PropertyType);
                            var result = converter.ConvertFrom(value);
                            property.SetValue(instance, result);
                        }

                        flag = false;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid value");
                    }
                }
            }

            return (T)instance;
        }

        public void UpdateCatalog() 
        {
            Catalog.Captains = DbManager.CaptainRepository.GetAll();
            Catalog.Cargos = DbManager.CargoRepository.GetAll();
            Catalog.CargoTypes = DbManager.CargoTypeRepository.GetAll();
            Catalog.Cities = DbManager.CityRepository.GetAll();
            Catalog.Ports = DbManager.PortRepository.GetAll();
            Catalog.Ships = DbManager.ShipRepository.GetAll();
            Catalog.Trips = DbManager.TripRepository.GetAll();
        }

        protected int SelectMenuItem<T>(List<T> entities)
        {
            Console.CursorVisible = false;
            var currentMenuItem = 0;

            var items = new List<string>();

            foreach (var item in entities) 
            {
                items.Add(item.ToString());
            }

            while (true)
            {
                DisplayItems(items, currentMenuItem);

                var key = Console.ReadKey(true);
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
            for (var i = 0; i < items.Count; i++)
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
