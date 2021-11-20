using DeliveryServiceApi.Data;
using System;

namespace DeliveryServiceApi.Services
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly AppDbContext appDbContext;

        public OrderRepository(AppDbContext appDbContext) => this.appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(AppDbContext));

        public Order Get(int id)
        {
            return appDbContext.Find<Order>(id);
        }
    }
}
