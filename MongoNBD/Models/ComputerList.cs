namespace MongoNBD.Models
{
    public class ComputerList
    {
        public IEnumerable<Computers> Computers { get; set; }   
        public ComputerFilter Filter { get; set; }
    }
}
