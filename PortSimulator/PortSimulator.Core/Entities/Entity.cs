using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortSimulator.Core.Entities
{
    public abstract class Entity
    {
        public int ID { get; set; }
        
        public Entity()
        {
            ID = -1;
        }

        public bool IsNew() 
        {
            return ID == -1;
        }
    }
}
