using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tfl.Client.Commandline.Contracts;
using Tfl.Client.Commandline.Services;

namespace Tfl.Client.Commandline.UnitTests
{
    [TestClass()]
    public class RoadServiceTests : BaseTests
    {
        private IRoadService roadService;
        private Mock<HttpMessageHandler> mockHandler;

        [TestInitialize]
        public void OnTestInitialize()
        {
            mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            roadService = new RoadService(mockHandler.Object, new ApplicationSettingsService(keys));
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
            // Arrange
            SetupHttpMessageHandlerValidResponse(mockHandler, id);

            // Act
            var apiResponse = await roadService.GetRoadStatus(id);

            // Assert
            Assert.IsTrue(apiResponse.IsSuccess);
            Assert.IsNotNull(apiResponse.Response);
            Assert.IsNull(apiResponse.ApiError);
            Assert.IsTrue(apiResponse.Response.Count == 1);
            Assert.IsTrue(apiResponse.Response.Single().Id.Equals(id, StringComparison.CurrentCultureIgnoreCase));

            // Verify
            mockHandler.Protected().Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod()]
        [DataRow("A****3")]
        [DataRow("A****2")]
        public async Task GetRoadStatusWithNonExistentRoadIdShouldReturn404NotFoundError(string id)
        {
            // Arrange
            SetupHttpMessageHandlerErrorResponse(mockHandler, id);

            // Act
            var apiResponse = await roadService.GetRoadStatus(id);

            // Assert
            Assert.IsFalse(apiResponse.IsSuccess);
            Assert.IsNull(apiResponse.Response);
            Assert.IsNotNull(apiResponse.ApiError);
            Assert.IsTrue(apiResponse.ApiError.HttpStatusCode == HttpStatusCode.NotFound);

            // Verify
            mockHandler.Protected().Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }

        #region Helpers

        private static void SetupHttpMessageHandlerValidResponse(Mock<HttpMessageHandler> mockHandler, string id)
        {
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[{" +
                    "\"$type\":\"Tfl.Api.Presentation.Entities.RoadCorridor, Tfl.Api.Presentation.Entities\"," +
                    $"\"id\":\"" + id + "\"," +
                    "\"displayName\":\"" + id.ToUpper() + "\"," +
                    "\"statusSeverity\":\"Good\"," +
                    "\"statusSeverityDescription\":\"No Exceptional Delays\"," +
                    "\"bounds\":\"[[-0.32751,51.32598],[-0.12639,51.48473]]\"," +
                    "\"envelope\":\"[[-0.32751,51.32598],[-0.32751,51.48473],[-0.12639,51.48473],[-0.12639,51.32598],[-0.32751,51.32598]]\"," +
                    "\"url\":\"/Road/a3\"}]")
                }).Verifiable();
        }

        private static void SetupHttpMessageHandlerErrorResponse(Mock<HttpMessageHandler> mockHandler, string id)
        {
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("{" +
                    "\"$type\":\"Tfl.Api.Presentation.Entities.ApiError, Tfl.Api.Presentation.Entities\"," +
                    "\"timestampUtc\":\"2018-10-20T18:14:07.2976549Z\"," +
                    "\"exceptionType\":\"EntityNotFoundException\"," +
                    "\"httpStatusCode\":404," +
                    "\"httpStatus\":\"NotFound\"," +
                    "\"relativeUri\":\"/road/" + id.ToUpper() + "\"," +
                    "\"message\":\"The following road id is not recognised: A233\"}")
                }).Verifiable();
        }

        #endregion
    }
}