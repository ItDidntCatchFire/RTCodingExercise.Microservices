using Catalog.Domain;
using IntegrationEvents;
using MassTransit;
using System.Numerics;

namespace Catalog.API.Consumers
{
    public class CreateConsumer : IConsumer<CreatePlate>
    {
        private ApplicationDbContext _dbContext;
        private readonly ILogger<CreateConsumer> _logger;

        public CreateConsumer(ApplicationDbContext dbContext, ILogger<CreateConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CreatePlate> context)
        {
            var plate = new Plate()
            {
                Registration = context.Message.Registration,
                PurchasePrice = context.Message.PurchasePrice,
            };

            //validate Plate
            if (string.IsNullOrWhiteSpace(plate.Registration))
            {
                _logger.LogInformation("Registration is invalid");
                return;

            }

            if (plate.PurchasePrice <= 0)
            {
                _logger.LogInformation("Purchase price not supplied");
                return;
            }

            var regDigits = string.Concat(plate.Registration.Where(Char.IsDigit));
            if (regDigits != string.Empty)
                plate.Numbers = int.Parse(regDigits);

            plate.Letters = string.Concat(plate.Registration.Where(x => !Char.IsDigit(x)));

            //if valid
            _dbContext.Plates.Add(plate);
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Registration is invalid");
            }

            await context.RespondAsync(plate);
        }
    }
}
