using IntegrationEvents;
using MassTransit;

namespace Catalog.API.Consumers
{
    public class SellConsumer : IConsumer<SellPlate>
    {
        private ApplicationDbContext _dbContext;
        private readonly ILogger<SellConsumer> _logger;

        public SellConsumer(ApplicationDbContext dbContext, ILogger<SellConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SellPlate> context)
        {
            var plateId = context.Message.PlateId;
            var discountCode = context.Message.DiscountCode;
            var plate = _dbContext.Plates.FirstOrDefault(x => x.Id == plateId);

            if (plate == default)
            {
                _logger.LogInformation("Plate doesn't exist");
                return;
            }

            if (plate.Status == PlateStatus.Sold)
            {
                _logger.LogInformation("Plate is already sold");
                return;
            }

            var salePrice = plate.CalculateSalesPrice(discountCode);
            if (plate.IsPriceTooLow(salePrice))
            {
                _logger.LogInformation($"Code {discountCode} cannot be applied to this item");
                return;
            }

            plate.Status = PlateStatus.Sold;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to sell plate");
            }
        }
    }
}
