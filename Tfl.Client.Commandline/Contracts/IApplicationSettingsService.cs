namespace Tfl.Client.Commandline.Contracts
{
    public interface IApplicationSettingsService
    {
        string ApplicationId { get; }
        string DeveloperKey { get; }
        string BaseApiUrl { get; }
        string ApiUri { get; }
    }
}
