namespace WebMVC.Models
{
    public class HomeModel
    {
        public ICollection<Catalog.Domain.Plate> Plates { get; set; }
        public string Initials { get; set; }
        public string Age { get; set; }
        public string PageNumber { get; set; }
        public string OrderBy { get; set; }
        public string DiscountCode { get; set; }
    }
}
