using Xunit;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Internal.Services.Movements.WebApi.Controllers;
using Internal.Services.Movements.Business.Manager;
using System.Collections.Generic;
using Internal.Services.Movements.ProxyClients;
using System.Linq;
using Internal.Services.Movements.IntegrationTests.Utilities;

namespace Internal.Services.Movements.IntegrationTests
{
    public class IntegrationTest : IClassFixture<TestStartup<Program>>
    { 

        private readonly TestStartup<Program> _factory;
        private readonly Utilities.MoqHelper _moq;
        private readonly HttpClient _client;
        
        public IntegrationTest(TestStartup<Program> factory)
        {
            _factory = factory;
            _moq = new Utilities.MoqHelper(factory.MovementMock);
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task test_fetchIncomingPayment_shouldReturnCorrectValue()
        {
            var response = await _client.GetAsync("/v1/GetMovements?movementType=Incoming&pageNumber=1&pageSize=10");
            var pagedMovements = await response.Content.ReadAsAsync<PagedMovements>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Single(pagedMovements.Movements);
            Assert.Single(pagedMovements.Movements.Where((x) =>
            x.MovementType == EnumMovementType.Unknown && 
            x.AccountTo == AccountHelper.CustomerAccount &&
            x.AccountFrom != AccountHelper.FiscalTransferAccount)
                .ToList());
        }

        [Fact]
        public async Task test_fetchFisicalTransfer_shouldReturnCorrectValue()
        {
            var response = await _client.GetAsync("/v1/GetMovements?movementType=FiscalTransfer&pageNumber=1&pageSize=10");
            var pagedMovements = await response.Content.ReadAsAsync<PagedMovements>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Single(pagedMovements.Movements);
            Assert.Single(pagedMovements.Movements.Where((x) =>
            x.MovementType == EnumMovementType.Unknown &&
            x.AccountFrom == AccountHelper.FiscalTransferAccount &&
            x.AccountTo == AccountHelper.CustomerAccount)
            .ToList());
        }

        [Fact]
        public async Task test_fetchInterestPayment_shouldReturnCorrectValue()
        {
            var response = await _client.GetAsync("/v1/GetMovements?movementType=Interest&pageNumber=1&pageSize=10");
            var pagedMovements = await response.Content.ReadAsAsync<PagedMovements>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Single(pagedMovements.Movements);
            Assert.Single(pagedMovements.Movements.Where((x) =>
            x.MovementType == EnumMovementType.Interest &&
            x.AccountTo == AccountHelper.CustomerAccount)
            .ToList());
        }
    }
}