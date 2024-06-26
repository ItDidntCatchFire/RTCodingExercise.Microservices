﻿using Catalog.Domain;
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


		[InlineData(0)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(200)]
		[Theory]
		public void CalculateSalesPrice_DISCOUNT(decimal price)
		{
			var plate = new Domain.Plate()
			{
				SalePrice = price
			};

			var expectedValue = price - 25;
			var actualValue = plate.CalculateSalesPrice("DISCOUNT");

			Assert.Equal(expectedValue, actualValue);
		}

		[InlineData(0)]
		[InlineData(10)]
		[InlineData(100)]
		[Theory]
		public void CalculateSalesPrice_PERCENTOFF(decimal price)
		{
			var plate = new Domain.Plate()
			{
				SalePrice = price
			};

			var expectedValue = price * 0.85m;
			var actualValue = plate.CalculateSalesPrice("PERCENTOFF");

			Assert.Equal(expectedValue, actualValue);
		}

		[Fact]
		public void IsPriceTooLow_DISCOUNT()
		{
			var plate = new Domain.Plate()
			{
				SalePrice = 200
			};
			var expectedValue = true;

			var salePrice = plate.CalculateSalesPrice("DISCOUNT");
			var actualValue = plate.IsPriceTooLow(salePrice);
			
			Assert.Equal(expectedValue, actualValue);
		}
	}
}
