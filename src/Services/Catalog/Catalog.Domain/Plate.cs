namespace Catalog.Domain
{
	public class Plate
	{
		public Guid Id { get; set; }

		public string? Registration { get; set; }

		public decimal PurchasePrice { get; set; }

		public decimal SalePrice { get; set; }

		public string? Letters { get; set; }

		public int Numbers { get; set; }

		public PlateStatus Status { get; set; }


		public decimal CalculateSalesPrice(string discountCode = "")
		{
			var salePrice = SalePrice * 1.2m;

			if (string.IsNullOrEmpty(discountCode))
				return salePrice;

			if (discountCode == "DISCOUNT")
				return salePrice - 25;

			if (discountCode == "PERCENTOFF")
				return salePrice * 0.85m;

			return salePrice;
		}
	}

	public enum PlateStatus
	{
		Available,
		Reserved,
		Sold
	}
}