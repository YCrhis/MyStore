namespace MyStore.Models
{
    public class OrderVM
    {
        public string OrderDate { get; set; }
        public string TotalAmount { get; set; }
        public ICollection<OrdenItemVM>? OrdenItems { get; set; }
    }
}
