namespace PortSimulator.Core.Entities
{
    public sealed class City : Entity
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("{0}\t{1}",
                Id, Name);
        }
    }
}
