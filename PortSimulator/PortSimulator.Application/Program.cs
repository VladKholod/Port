using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PortSimulator.Core;
using PortSimulator.Core.Entities;

using PortSimulator.DatabaseManager;
using PortSimulator.DatabaseManager.Repositories;

using PortSimulator.Application.Views;
using PortSimulator.Application.Views.Abstractions;

namespace PortSimulator.Application
{
    public sealed class Program
    {
        static void Main(string[] args)
        {
            Application app = new Application();
            app.Start();
        }
    }
}
