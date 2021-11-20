using DeliveryServiceApi;
using DeliveryServiceApi.Data;
using DeliveryServiceApi.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryIntegrationApi.IntegrationTests
{
    [TestFixture]
    public class DeliveryControllerTests
    {
        //simple test
        [Test]
        public async Task CheckStatus_SendRequest_ReturnOk()
        {
            //arrange
            WebApplicationFactory<Startup> webApplication = new WebApplicationFactory<Startup>();
            var httpClient = webApplication.CreateClient();

            //act
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("api/delivery/check-status");

            //assert
            Assert.AreEqual(HttpStatusCode.OK, httpResponseMessage.StatusCode);
        }

        //change service
        [Test]
        public async Task SendOrder_DeliveryAvailable_ReturnNotFound()
        {
            //arrange
            WebApplicationFactory<Startup> webApplicationFactory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
                builder.ConfigureServices(services =>
                {
                    var orderService = services.SingleOrDefault(x => x.ServiceType == typeof(IOrderService));
                    services.Remove(orderService);
                    var moqService = new Mock<IOrderService>();
                    moqService.Setup(s => s.DeliveryAvailable()).Returns(false);
                    services.AddTransient(_ => moqService.Object);
                })
            );

            var httpClient = webApplicationFactory.CreateClient();

            //act
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("api/delivery/send-order", null);
            //assert
            Assert.AreEqual(HttpStatusCode.NotFound, httpResponseMessage.StatusCode);
        }


        //change dbcontext
        [Test]
        public async Task GetOrdersCount_SendRequest_ReturnMoreThanOne()
        {
            //arrange
            WebApplicationFactory<Startup> webApplicationFactory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
                builder.ConfigureServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<AppDbContext>));
                    services.Remove(dbContextDescriptor);
                    services.AddDbContext<AppDbContext>(opts =>
                    {
                        opts.UseInMemoryDatabase("in_memory_db");
                    });
                })
            );

            AppDbContext appDbContext = webApplicationFactory.Services.CreateScope().ServiceProvider.GetService<AppDbContext>();
            for (int i = 0; i < 10; i++)
            {
                await appDbContext.Orders.AddAsync(new Order() { DateTime = DateTime.Now });
            }
            await appDbContext.SaveChangesAsync();

            var httpClient = webApplicationFactory.CreateClient();

            //act
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("api/delivery/get-orders-count");
            var actualCount = int.Parse(await httpResponseMessage.Content.ReadAsStringAsync());
            //assert
            Assert.IsTrue(actualCount > 1);
        }
    }
}
