namespace PortSimulator.Core.Entities
{
    public abstract class Entity
    {
        public int Id { get; set; }

        protected Entity()
        {
            Id = -1;
        }

        public bool IsNew() 
        {
            return Id == -1;
        }
    }
}
