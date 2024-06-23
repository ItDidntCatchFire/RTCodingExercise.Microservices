using IntegrationEvents;
using MassTransit;

namespace Catalog.API.Consumers
{
    public class ReserveConsumer : IConsumer<ReservePlate>
    {
        private ApplicationDbContext _dbContext;
        private readonly ILogger<ReserveConsumer> _logger;

        public ReserveConsumer(ApplicationDbContext dbContext, ILogger<ReserveConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ReservePlate> context)
        {
            var plateId = context.Message.PlateId;
            var plate = _dbContext.Plates.FirstOrDefault(x => x.Id == plateId);

            if (plate == default)
            {
                _logger.LogInformation($"Plate {plateId} was not found");
                return;
            }

            if (plate.Status == PlateStatus.Reserved)
            {
                _logger.LogInformation($"Plate {plateId} is already reserved");
                return;
            }

            if (plate.Status == PlateStatus.Sold)
            {
                _logger.LogInformation($"Plate {plateId} is already sold");
                return;
            }

            plate.Status = PlateStatus.Reserved;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to reserve plate");
            }
        }
    }
}
