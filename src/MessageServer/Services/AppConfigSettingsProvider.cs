using System.Configuration;
using MessageServer.Interfaces;
using SuperSocket.SocketBase.Config;

namespace MessageServer.Services
{
    /// <summary>
    /// Default implementation of ISettingsProvider that reads settings from the app.config file.
    /// </summary>
    public class AppConfigSettingsProvider : ISettingsProvider
    {
        /// <summary>
        /// Get an application setting from appSettings in app.config.
        /// </summary>
        /// <param name="key">The setting key.</param>
        /// <returns>The value of the setting.</returns>
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
