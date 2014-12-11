namespace PortSimulator.Core.Entities
{
    public sealed class Port : Entity
    {
        public string Name { get; set; }
        public int CityId { get; set; }

        public override string ToString()
        {
            return string.Format("{0}\t{1}\t{2}",
                Id, Name, CityId);
        }
    }
}
