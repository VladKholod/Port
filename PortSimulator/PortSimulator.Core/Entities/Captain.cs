namespace PortSimulator.Core.Entities
{
    public sealed class Captain : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override string ToString()
        {
            return string.Format("{0}\t{1}\t{2}",
                Id, FirstName, LastName);
        }
    }
}
