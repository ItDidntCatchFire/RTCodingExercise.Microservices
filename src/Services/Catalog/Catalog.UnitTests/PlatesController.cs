using Catalog.API.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Catalog.UnitTests
{
	public class PlatesController
    {
        [Fact]
        public void GetAllPlates()
        {
            // Initialize a list of MyEntity objects to back the DbSet with.
            var myEntities = new List<Domain.Plate>()
            { 
                new Domain.Plate() {
                    Id = Guid.Parse("0812851E-3EC3-4D12-BAF6-C9F0E6DC2F76"),
                    Registration = "T44GUE",
                    PurchasePrice = 2722.51m,
                    SalePrice = 8995.00m,
                    Numbers = 44,
                    Letters = "TAG"
            }, new Domain.Plate() { 
                    Id = Guid.Parse("DF81D7FC-319B-46A8-AB66-2574B4169C3D"),
                    Registration = "M44BEY",
                    PurchasePrice = 859.10m,
                    SalePrice = 8995.00m,
                    Numbers = 44,
                    Letters = "MAB"
            }, new Domain.Plate() {
                    Id = Guid.Parse("0E9C83BF-94E2-484A-97CB-A8B06E3410FD"),
                    Registration = "P777PER",
                    PurchasePrice = 1494.08m,
                    SalePrice = 4995.00m,
                    Numbers = 777,
                    Letters = "PYP"
                } 
            };
 
            // Create a mock DbContext.
            var dbContext = new Mock<ApplicationDbContext>();
 
            // Create a mock DbSet.
            var dbSet = MockDbSetFactory.Create(myEntities);
 
            // Set up the MyEntities property so it returns the mocked DbSet.
            dbContext.Setup(o => o.Plates).Returns(dbSet.Object);
 
            var platesController = new Catalog.API.Controllers.PlatesController(dbContext.Object);

            var actualPlates =  platesController.GetPlates();

            var expectedPlates = myEntities
				.OrderBy(x => x.Id)
				.Select(x => new
                {
                    Registration = x.Registration,
                    PurchasePrice = x.PurchasePrice,
				    SalePrice = x.CalculateSalesPrice()
			    });
            
            // assert
            var okResult = Assert.IsType<OkObjectResult>(actualPlates);
            Assert.NotNull(actualPlates);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(JsonSerializer.Serialize(expectedPlates), JsonSerializer.Serialize(okResult.Value));
        }
        
        [Fact]
        public void GetAllPlates_CheckAdd()
        {
            // Initialize a list of MyEntity objects to back the DbSet with.
            var myEntities = new List<Domain.Plate>()
            { 
                new Domain.Plate() {
                    Id = Guid.Parse("0812851E-3EC3-4D12-BAF6-C9F0E6DC2F76"),
                    Registration = "T44GUE",
                    PurchasePrice = 2722.51m,
                    SalePrice = 8995.00m,
                    Numbers = 44,
                    Letters = "TAG"
            }, new Domain.Plate() { 
                    Id = Guid.Parse("DF81D7FC-319B-46A8-AB66-2574B4169C3D"),
                    Registration = "M44BEY",
                    PurchasePrice = 859.10m,
                    SalePrice = 8995.00m,
                    Numbers = 44,
                    Letters = "MAB"
            }, new Domain.Plate() {
                    Id = Guid.Parse("0E9C83BF-94E2-484A-97CB-A8B06E3410FD"),
                    Registration = "P777PER",
                    PurchasePrice = 1494.08m,
                    SalePrice = 4995.00m,
                    Numbers = 777,
                    Letters = "PYP"
                } 
            };
 
            // Create a mock DbContext.
            var dbContext = new Mock<ApplicationDbContext>();
 
            // Create a mock DbSet.
            var dbSet = MockDbSetFactory.Create(myEntities);
 
            // Set up the MyEntities property so it returns the mocked DbSet.
            dbContext.Setup(o => o.Plates).Returns(dbSet.Object);
 
            var platesController = new Catalog.API.Controllers.PlatesController(dbContext.Object);

            var actualPlates =  platesController.GetPlates();

            var expectedPlates = myEntities
                .OrderBy(x => x.Id)
				.Select(x => new
				{
					Registration = x.Registration,
					PurchasePrice = x.PurchasePrice,
					SalePrice = x.CalculateSalesPrice()
				});

			// assert
			var okResult = Assert.IsType<OkObjectResult>(actualPlates);
            Assert.NotNull(actualPlates);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(JsonSerializer.Serialize(expectedPlates), JsonSerializer.Serialize(okResult.Value));
            
            
            
            myEntities.Add(new Domain.Plate() {
                Id = Guid.Parse("7C88B586-AABA-400A-8EF2-AF2073FC0CB2"),
                Registration = "M66VEY",
                PurchasePrice = 469.26m,
                SalePrice = 5995.00m,
                Numbers = 66,
                Letters = "MCV"
            });


			expectedPlates = myEntities
                .OrderBy(x => x.Id)
				.Select(x => new
				{
					Registration = x.Registration,
					PurchasePrice = x.PurchasePrice,
					SalePrice = x.CalculateSalesPrice()
				});


			actualPlates =  platesController.GetPlates();
            
            // assert
            okResult = Assert.IsType<OkObjectResult>(actualPlates);
            Assert.NotNull(actualPlates);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(JsonSerializer.Serialize(expectedPlates), JsonSerializer.Serialize(okResult.Value));
        }

        [Fact]
        public void GetAllPlates_Paged_Take()
        {
            // Initialize a list of MyEntity objects to back the DbSet with.
            var myEntities = new List<Domain.Plate>();

            //Create 21 Plates
            for (int i = 0; i < 21; i++)
                myEntities.Add(new()
                {
                    Id = Guid.NewGuid(),
                });


			// Create a mock DbContext.
			var dbContext = new Mock<ApplicationDbContext>();

			// Create a mock DbSet.
			var dbSet = MockDbSetFactory.Create(myEntities);

			// Set up the MyEntities property so it returns the mocked DbSet.
			dbContext.Setup(o => o.Plates).Returns(dbSet.Object);

			var platesController = new Catalog.API.Controllers.PlatesController(dbContext.Object);

			var actualPlates = platesController.GetPlates();

			var expectedPlates = myEntities
			   .OrderBy(x => x.Id)
               .Take(20)
			   .Select(x => new
			   {
				   Registration = x.Registration,
				   PurchasePrice = x.PurchasePrice,
				   SalePrice = x.CalculateSalesPrice()
			   });

			// assert
			var okResult = Assert.IsType<OkObjectResult>(actualPlates);
			Assert.NotNull(actualPlates);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(JsonSerializer.Serialize(expectedPlates), JsonSerializer.Serialize(okResult.Value));
		}

		[Fact]
		public void GetAllPlates_Paged_Skip()
		{
			// Initialize a list of MyEntity objects to back the DbSet with.
			var myEntities = new List<Domain.Plate>();

			//Create 21 Plates
			for (int i = 0; i < 21; i++)
				myEntities.Add(new()
				{
					Id = Guid.NewGuid(),
				});


			// Create a mock DbContext.
			var dbContext = new Mock<ApplicationDbContext>();

			// Create a mock DbSet.
			var dbSet = MockDbSetFactory.Create(myEntities);

			// Set up the MyEntities property so it returns the mocked DbSet.
			dbContext.Setup(o => o.Plates).Returns(dbSet.Object);

			var platesController = new Catalog.API.Controllers.PlatesController(dbContext.Object);

			var actualPlates = platesController.GetPlates(1);

			var expectedPlates = myEntities
			   .OrderBy(x => x.Id)
               .Skip(20)
			   .Take(20)
			   .Select(x => new
			   {
				   Registration = x.Registration,
				   PurchasePrice = x.PurchasePrice,
				   SalePrice = x.CalculateSalesPrice()
			   });

			// assert
			var okResult = Assert.IsType<OkObjectResult>(actualPlates);
			Assert.NotNull(actualPlates);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(JsonSerializer.Serialize(expectedPlates), JsonSerializer.Serialize(okResult.Value));
		}


		[InlineData("id", true)]
		[InlineData("id", false)]
		[InlineData("registration", true)]
		[InlineData("registration", false)]
		[InlineData("purchaseprice", true)]
		[InlineData("purchaseprice", false)]
		[InlineData("saleprice", true)]
		[InlineData("saleprice", false)]
		[Theory]
		public void GetAllPlates_OrderBy(string orderBy, bool ascending)
        {
			// Initialize a list of MyEntity objects to back the DbSet with.
			var myEntities = new List<Domain.Plate>()
			{
				new Domain.Plate() {
					Id = Guid.Parse("0812851E-3EC3-4D12-BAF6-C9F0E6DC2F76"),
					Registration = "T44GUE",
					PurchasePrice = 2722.51m,
					SalePrice = 8995.00m,
					Numbers = 44,
					Letters = "TAG"
			}, new Domain.Plate() {
					Id = Guid.Parse("DF81D7FC-319B-46A8-AB66-2574B4169C3D"),
					Registration = "M44BEY",
					PurchasePrice = 859.10m,
					SalePrice = 8995.00m,
					Numbers = 44,
					Letters = "MAB"
			}, new Domain.Plate() {
					Id = Guid.Parse("0E9C83BF-94E2-484A-97CB-A8B06E3410FD"),
					Registration = "P777PER",
					PurchasePrice = 1494.08m,
					SalePrice = 4995.00m,
					Numbers = 777,
					Letters = "PYP"
				}
			};

			// Create a mock DbContext.
			var dbContext = new Mock<ApplicationDbContext>();

			// Create a mock DbSet.
			var dbSet = MockDbSetFactory.Create(myEntities);

			// Set up the MyEntities property so it returns the mocked DbSet.
			dbContext.Setup(o => o.Plates).Returns(dbSet.Object);

			var platesController = new Catalog.API.Controllers.PlatesController(dbContext.Object);

			var actualPlates = platesController.GetPlates(orderBy: orderBy, orderAscending: ascending);

			myEntities = OrderByGenerator(myEntities, orderBy, ascending);
			var expectedPlates = myEntities
				.Select(x => new
				{
					Registration = x.Registration,
					PurchasePrice = x.PurchasePrice,
					SalePrice = x.CalculateSalesPrice()
				});

			// assert
			var okResult = Assert.IsType<OkObjectResult>(actualPlates);
			Assert.NotNull(actualPlates);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(JsonSerializer.Serialize(expectedPlates), JsonSerializer.Serialize(okResult.Value));
		}


		[InlineData(-1, "")]
		[InlineData(-1, " ")] //Check trimming is working correctly
		[InlineData(-1, "	")]
		[InlineData(-1, "a")]
		[InlineData(1, "")]
		[InlineData(44, "")]
		[InlineData(-1, "TAG")]
		[InlineData(44, "TAG")]
		[InlineData(44, "TAG ")] //Check trimming is working correctly
		[InlineData(44, " TAG ")] //Check trimming is working correctly
		[Theory]
		public void GetAllPlates_Filter(int age, string initials)
		{
			// Initialize a list of MyEntity objects to back the DbSet with.
			var myEntities = new List<Domain.Plate>()
			{
				new Domain.Plate() {
					Id = Guid.Parse("0812851E-3EC3-4D12-BAF6-C9F0E6DC2F76"),
					Registration = "T44GUE",
					PurchasePrice = 2722.51m,
					SalePrice = 8995.00m,
					Numbers = 44,
					Letters = "TAG"
			}, new Domain.Plate() {
					Id = Guid.Parse("DF81D7FC-319B-46A8-AB66-2574B4169C3D"),
					Registration = "M44BEY",
					PurchasePrice = 859.10m,
					SalePrice = 8995.00m,
					Numbers = 44,
					Letters = "MAB"
			}, new Domain.Plate() {
					Id = Guid.Parse("0E9C83BF-94E2-484A-97CB-A8B06E3410FD"),
					Registration = "P777PER",
					PurchasePrice = 1494.08m,
					SalePrice = 4995.00m,
					Numbers = 777,
					Letters = "PYP"
				}
			};

			// Create a mock DbContext.
			var dbContext = new Mock<ApplicationDbContext>();

			// Create a mock DbSet.
			var dbSet = MockDbSetFactory.Create(myEntities);

			// Set up the MyEntities property so it returns the mocked DbSet.
			dbContext.Setup(o => o.Plates).Returns(dbSet.Object);

			var platesController = new Catalog.API.Controllers.PlatesController(dbContext.Object);

			var actualPlates = platesController.GetPlates(age: age, initials: initials);

			if (age > -1)
				myEntities = myEntities.Where(x => x.Numbers == age).ToList();

			if (!string.IsNullOrEmpty(initials.Trim()))
				myEntities = myEntities.Where(x => x.Letters.ToLower() == initials.Trim().ToLower()).ToList();

			var expectedPlates = myEntities
				.OrderBy(x => x.Id)
				.Select(x => new
				{
					Registration = x.Registration,
					PurchasePrice = x.PurchasePrice,
					SalePrice = x.CalculateSalesPrice()
				});

			// assert
			var okResult = Assert.IsType<OkObjectResult>(actualPlates);
			Assert.NotNull(actualPlates);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(JsonSerializer.Serialize(expectedPlates), JsonSerializer.Serialize(okResult.Value));
		}




		private List<Domain.Plate> OrderByGenerator(List<Domain.Plate> plates, string orderBy, bool asc)
		{
			return orderBy switch
			{
				"registration" => (asc ? plates.OrderBy(x => x.Registration) : plates.OrderByDescending(x => x.Registration)).ToList(),
				"purchaseprice" => (asc ? plates.OrderBy(x => x.PurchasePrice) : plates.OrderByDescending(x => x.PurchasePrice)).ToList(),
				"saleprice" => (asc ? plates.OrderBy(x => x.SalePrice) : plates.OrderByDescending(x => x.SalePrice)).ToList(),
				_ => (asc ? plates.OrderBy(x => x.Id) : plates.OrderByDescending(x => x.Id)).ToList(),
			};
		}
	}
}