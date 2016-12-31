using System.Configuration;
using MessageServer.Interfaces;
using SuperSocket.SocketBase.Config;

namespace MessageServer.Services
{
    public class AppConfigSettingsProvider : ISettingsProvider
    {
        public string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// Get the websocket configuration section. 
        /// </summary>
        /// <returns> The websocket configuration section.</returns>
        public IConfigurationSource GetWebsocketConfiguration()
        {
            return ConfigurationManager.GetSection("superSocket") as IConfigurationSource;
        }
    }
}
