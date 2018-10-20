using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Tfl.Client.Commandline.Contracts;
using Tfl.Client.Commandline.Dtos;
using Tfl.Client.Commandline.Dtos.Response.Road;

namespace Tfl.Client.Commandline.Services
{
    public class RoadService : IRoadService
    {
        private HttpClient httpClient;
        private IApplicationSettingsService applicationSettingsService;

        public RoadService(HttpMessageHandler httpMessageHandler, IApplicationSettingsService applicationSettingsService)
        {
            httpClient = new HttpClient(httpMessageHandler);
            this.applicationSettingsService = applicationSettingsService;
        }

        public RoadService(IApplicationSettingsService applicationSettingsService)
        {
            httpClient = new HttpClient();
            this.applicationSettingsService = applicationSettingsService;
        }

        public async Task<ApiResponse<List<RoadCorridor>>> GetRoadStatus(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException(nameof(id));

            var httpResponseMessage = await httpClient.GetAsync(string.Format(applicationSettingsService.ApiUri, $"Road/{id}"));

            return await httpResponseMessage.GetResponseObject<List<RoadCorridor>>();
        }
    }
}