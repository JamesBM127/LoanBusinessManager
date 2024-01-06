namespace LBMLibrary.Entity
{
    public class Rent : BusinessClass
    {
        public DateTime DueDate { get; set; }
        public Guid HouseId { get; set; }
        public House House { get; set; }
    }
}
