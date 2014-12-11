using System;
using System.Collections.Generic;
using System.Linq;
using PortSimulator.Application.Views;
using PortSimulator.Application.Views.Abstractions;
using PortSimulator.Core.Entities;
using PortSimulator.DatabaseManager;

namespace PortSimulator.Application
{
    public sealed class Application
    {
        private BaseView _viewBehaviour;

        private Commands _selectedCommand = Commands.Create;
        private Entities _selectedEntity = Entities.Captain;

        public Application()
        {
            _viewBehaviour = new TripView();
        }

        public void Start()
        {
            Console.WriteLine("Press any key to start..");
            while (Console.ReadKey(false).Key != ConsoleKey.F12)
            {
                SelectMenuItem(Enum.GetNames(typeof(Commands)).ToList());
            }
        }

        private void PerformCommand() 
        {
            ChangeIViewBehaviour();

            if (_selectedCommand == Commands.Create) 
            {
                _viewBehaviour.CreateAsync();
            }
            else if (_selectedCommand == Commands.Delete)
            {
                _viewBehaviour.DeleteAsync();
            }
            else if (_selectedCommand == Commands.Select)
            {
                _viewBehaviour.Show();
            }
            else if (_selectedCommand == Commands.SelectAll)
            {
                _viewBehaviour.ShowAll();
            }
            else if (_selectedCommand == Commands.Update)
            {
                _viewBehaviour.UpdateAsync();
            }
        }

        private void ChangeIViewBehaviour() 
        {
            if (_selectedEntity == Entities.Captain)
            {
                _viewBehaviour = new CaptainView();
            }
            else if (_selectedEntity == Entities.Cargo)
            {
                _viewBehaviour = new CargoView();
            }
            else if (_selectedEntity == Entities.CargoType)
            {
                _viewBehaviour = new CargoTypeView();
            }
            else if (_selectedEntity == Entities.City)
            {
                _viewBehaviour = new CityView();
            }
            else if (_selectedEntity == Entities.Port)
            {
                _viewBehaviour = new PortView();
            }
            else if (_selectedEntity == Entities.Ship)
            {
                _viewBehaviour = new ShipView();
            }
            else if (_selectedEntity == Entities.Trip)
            {
                _viewBehaviour = new TripView();
            }
        }

        private void DisplayMenu(List<string> items, int selectedMenuItem)
        {
            Console.Clear();
            for (var i = 0; i < items.Count; i++)
            {
                Console.WriteLine("{0} {1}", i == selectedMenuItem ? "    >\t" : "\t", items[i]);
            }

            DisplayHelp();
        }

        private void DisplayHelp()
        {
            Console.SetCursorPosition(0, Console.WindowHeight - 4);
            Console.WriteLine("Press <Enter> to select menu item.");
            Console.WriteLine("Press <Backspace> to return to previous submenu.");
        }

        private void SelectMenuItem(List<string> items)
        {
            Console.CursorVisible = false;
            var currentMenuItem = 0;
            while (true)
            {
                DisplayMenu(items, currentMenuItem);

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
                    SetSelectedCommand(currentMenuItem);
                    SelectSubMenuItem(Enum.GetNames(typeof(Entities)).ToList());
                    break;
                }
            }
        }

        private void SelectSubMenuItem(List<string> items)
        {
            Console.CursorVisible = false;
            var currentSubMenuItem = 0;
            while (true)
            {
                DisplayMenu(items, currentSubMenuItem);

                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow)
                {
                    if (currentSubMenuItem > 0)
                        currentSubMenuItem--;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    if (currentSubMenuItem < items.Count - 1)
                        currentSubMenuItem++;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    SetSelectedEntity(currentSubMenuItem);
                    Console.Clear();
                    PerformCommand();
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace) 
                {
                    SelectMenuItem(Enum.GetNames(typeof(Commands)).ToList());
                    break;
                }
            }
        }

        private void SetSelectedCommand(int index) 
        {
            if (index == 0)
            {
                _selectedCommand = Commands.Create;
            }
            else if (index == 1)
            {
                _selectedCommand = Commands.Update;
            }
            else if (index == 2)
            {
                _selectedCommand = Commands.Delete;
            }
            else if (index == 3)
            {
                _selectedCommand = Commands.Select;
            }
            else if (index == 4)
            {
                _selectedCommand = Commands.SelectAll;
            }
        }

        private void SetSelectedEntity(int index)
        {
            if (index == 0)
            {
                _selectedEntity = Entities.Captain;
            }
            else if (index == 1)
            {
                _selectedEntity = Entities.Cargo;
            }
            else if (index == 2)
            {
                _selectedEntity = Entities.CargoType;
            }
            else if (index == 3)
            {
                _selectedEntity = Entities.City;
            }
            else if (index == 4)
            {
                _selectedEntity = Entities.Port;
            }
            else if (index == 5)
            {
                _selectedEntity = Entities.Ship;
            }
            else if (index == 6)
            {
                _selectedEntity = Entities.Trip;
            }
        }
    }
}
