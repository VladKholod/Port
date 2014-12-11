namespace PortSimulator.Core.Entities
{
    public sealed class CargoType : Entity
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("{0}\t{1}",
                Id, Name);
        }
    }
}
