using System.Collections.Generic;
using System.Configuration;

namespace Tfl.Client.Commandline.IntegrationTests
{
    public class BaseTests
    {
        protected readonly IDictionary<string, string> keys = new Dictionary<string, string>()
        {
            { "app_id", ConfigurationManager.AppSettings["app_id"] },
            { "developer_key", ConfigurationManager.AppSettings["developer_key"] },
            { "base_api_url", ConfigurationManager.AppSettings["base_api_url"] }
        };
    }
}