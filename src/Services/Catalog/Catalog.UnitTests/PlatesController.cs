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

            var expectedPlates = myEntities.Select(x => new
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

            var expectedPlates = myEntities.Select(x => new
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


			expectedPlates = myEntities.Select(x => new
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
    }
}