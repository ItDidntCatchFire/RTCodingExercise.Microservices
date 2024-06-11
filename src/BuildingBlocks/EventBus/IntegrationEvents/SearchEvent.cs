namespace IntegrationEvents
{
    public class SearchEvent
    {
        public int PageNumber { get; set; }
        public int Age { get; set; } = -1;
        public string? Initials { get; set; }

        public string DiscountCode { get; set; }
        public string OrderBy { get; set; } = "id";
        public bool OrderAscending { get; set; }
    }
}
