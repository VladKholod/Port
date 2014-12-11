namespace PortSimulator.Core.Entities
{
    public sealed class Cargo : Entity
    {
        public int Number { get; set; }
        public int Weight { get; set; }
        public int Price { get; set; }
        public int InsurancePrice { get; set; }
        public int CargoTypeId { get; set; }
        public int TripId { get; set; }

        public override string ToString()
        {
            return string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}",
                Id, Number, Weight, Price, InsurancePrice, CargoTypeId, TripId);
        }
    }
}
