using System.Collections.Generic;
using System.Collections.Specialized;
using Tfl.Client.Commandline.Contracts;

namespace Tfl.Client.Commandline.Services
{
    public class ApplicationSettingsService : IApplicationSettingsService
    {
        public ApplicationSettingsService(IDictionary<string, string> keys)
        {
            ApplicationId = keys["app_id"];
            DeveloperKey = keys["developer_key"];
            BaseApiUrl = keys["base_api_url"];
            ApiUri = $"{BaseApiUrl}{{0}}?app_id={ApplicationId}&app_key={DeveloperKey}";
        }

        public string ApplicationId
        {
            get;
            private set;
        }

        public string DeveloperKey
        {
            get;
            private set;
        }

        public string BaseApiUrl
        {
            get;
            private set;
        }

        public string ApiUri
        {
            get;
            private set;
        }
    }
}