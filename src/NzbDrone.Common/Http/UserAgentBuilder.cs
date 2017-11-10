using NzbDrone.Common.EnvironmentInfo;

namespace NzbDrone.Common.Http
{
    public interface IUserAgentBuilder
    {
        string GetUserAgent(bool simplified = false);
    }

    public class UserAgentBuilder : IUserAgentBuilder
    {
        private readonly string _userAgentSimplified;
        private readonly string _userAgent;

        public string GetUserAgent(bool simplified)
        {
            if (simplified)
            {
                return _userAgentSimplified;
            }

            return _userAgent;
        }

        public UserAgentBuilder(IOsInfo osInfo)
        {
            var osName = osInfo.Name.ToLower();
            var osVersion = osInfo.Version.ToLower();

            _userAgent = $"Radarr/{BuildInfo.Version} ({osName} {osVersion})";
            _userAgentSimplified = $"Radarr/{BuildInfo.Version.ToString(2)}";
        }
    }
}
