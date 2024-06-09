using Xunit;

namespace Catalog.UnitTests
{
	public class Plate
	{

		[Fact]
		public void CalculateSalesPrice_Default()
		{
			var plate = new Domain.Plate()
			{
				SalePrice = 10
			};

			var expectedValue = 12;
			var actualValue = plate.CalculateSalesPrice();

			Assert.Equal(expectedValue, actualValue);
		}

	}
}
