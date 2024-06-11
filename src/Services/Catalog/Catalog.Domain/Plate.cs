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
			if (discountCode == "DISCOUNT")
				return SalePrice - 25;

			if (discountCode == "PERCENTOFF")
				return SalePrice * 0.85m;

			return SalePrice * 1.2m;
		}

		public bool IsPriceTooLow(decimal price)
			=> price <= (0.9m * SalePrice);
	}

	public enum PlateStatus
	{
		Available,
		Reserved,
		Sold
	}
}