using IntegrationEvents;
using MassTransit;
using System.Linq.Expressions;

namespace Catalog.API.Consumers
{
    public class SearchConsumer : IConsumer<SearchEvent>
    {
        private const int NUMBER_OF_PLATES = 20;
        public ApplicationDbContext _dbContext;

        public SearchConsumer(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<SearchEvent> context)
        {
            var values = context.Message;

            var plates =
            (values.OrderAscending ? _dbContext.Plates.OrderBy(SortBy(values.OrderBy)) : _dbContext.Plates.OrderByDescending(SortBy(values.OrderBy)))
            .ThenBy(x => x.Id)
            .Where(FilterBy(values.Age, values.Initials))
            .Where(x => x.Status != PlateStatus.Reserved)
            .Skip(values.PageNumber * NUMBER_OF_PLATES)
            .Take(NUMBER_OF_PLATES)
            .Select(x => new Plate
            {
                Id = x.Id,
                Registration = x.Registration,
                PurchasePrice = x.PurchasePrice,
                SalePrice = x.CalculateSalesPrice(values.DiscountCode),
                Letters = x.Letters,
                Numbers = x.Numbers,
                Status = x.Status,
            });

            await context.RespondAsync(plates.ToArray());
        }

        private Expression<Func<Plate, object>> SortBy(string property)
        {
            return property.ToLower() switch
            {
                "registration" => (x => x.Registration),
                "purchaseprice" => (x => x.PurchasePrice),
                "saleprice" => (x => x.SalePrice),
                _ => (x => x.Id)
            };
        }

        private Expression<Func<Plate, bool>> FilterBy(int age, string initials)
        {
            if (age > -1 && initials != null && !string.IsNullOrEmpty(initials.Trim()))
                return (x => x.Numbers == age && x.Letters.ToLower() == initials.Trim().ToLower());

            if (age > -1)
                return (x => x.Numbers == age);

            if (initials != null && !string.IsNullOrEmpty(initials.Trim()))
                return (x => x.Letters.ToLower() == initials.Trim().ToLower());

            return (x => true);
        }
    }
}