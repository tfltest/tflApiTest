using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tfl.Client.Commandline.Contracts;
using Tfl.Client.Commandline.Services;

namespace Tfl.Client.Commandline.IntegrationTests
{
    [TestClass()]
    public class RoadServiceTests : BaseTests
    {
        private IRoadService roadService;

        [TestInitialize]
        public void OnTestInitialize()
        {
            roadService = new RoadService(new ApplicationSettingsService(keys));
        }

        [TestCleanup]
        public void OnTestCleanup()
        {
        }

        [TestMethod()]
        [DataRow("")]
        [DataRow("   ")]
        [DataRow(null)]
        public async Task GetRoadStatusWithInvalidInputThrowsArgumentException(string id)
        {
            var argumentExcpetion = await Assert.ThrowsExceptionAsync<ArgumentException>(() => { return roadService.GetRoadStatus(id); });
            Assert.IsTrue(argumentExcpetion.Message == nameof(id));
        }

        [TestMethod()]
        [DataRow("A3")]
        [DataRow("A2")]
        public async Task GetRoadStatusWithValidInputShouldReturnRoadStatus(string id)
        {
            // Act
            var apiResponse = await roadService.GetRoadStatus(id);

            // Assert
            Assert.IsTrue(apiResponse.IsSuccess);
            Assert.IsNotNull(apiResponse.Response);
            Assert.IsNull(apiResponse.ApiError);
            Assert.IsTrue(apiResponse.Response.Count == 1);
            Assert.IsTrue(apiResponse.Response.Single().Id.Equals(id, StringComparison.CurrentCultureIgnoreCase));
        }

        [TestMethod()]
        [DataRow("A****3")]
        [DataRow("A****2")]
        public async Task GetRoadStatusWithNonExistentRoadIdShouldReturn404NotFoundError(string id)
        {
            // Act
            var apiResponse = await roadService.GetRoadStatus(id);

            // Assert
            Assert.IsFalse(apiResponse.IsSuccess);
            Assert.IsNull(apiResponse.Response);
            Assert.IsNotNull(apiResponse.ApiError);
            Assert.IsTrue(apiResponse.ApiError.HttpStatusCode == HttpStatusCode.NotFound);
        }
    }
}