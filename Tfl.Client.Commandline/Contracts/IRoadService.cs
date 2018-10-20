using System.Collections.Generic;
using System.Threading.Tasks;
using Tfl.Client.Commandline.Dtos;
using Tfl.Client.Commandline.Dtos.Response.Road;

namespace Tfl.Client.Commandline.Contracts
{
    public interface IRoadService
    {
        Task<ApiResponse<List<RoadCorridor>>> GetRoadStatus(string id);
    }
}
