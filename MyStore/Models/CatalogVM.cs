namespace MyStore.Models
{
    public class CatalogVM
    {
        public IEnumerable<CategoryVM> Categories { get; set; }
        public IEnumerable<ProductVM> Products { get; set; }
        public string FilterBy { get; set; }
    }
}
