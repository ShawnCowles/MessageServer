using SuperSocket.SocketBase.Config;

namespace MessageServer.Interfaces
{
    /// <summary>
    /// A service that can provide application settings. Registered by default in 
    /// MessageServerModule to read from the app.config file.
    /// </summary>
    public interface ISettingsProvider
    {
        /// <summary>
        /// Get an application setting.
        /// </summary>
        /// <param name="key">The setting key.</param>
        /// <returns>The value of the setting.</returns>
        string GetAppSetting(string key);

        /// <summary>
        /// Get the configuration section for websockets.
        /// </summary>
        /// <returns></returns>
        IConfigurationSource GetWebsocketConfiguration();
    }
}
