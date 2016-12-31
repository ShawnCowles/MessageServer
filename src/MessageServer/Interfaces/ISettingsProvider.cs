using SuperSocket.SocketBase.Config;

namespace MessageServer.Interfaces
{
    public interface ISettingsProvider
    {
        string GetAppSetting(string key);

        IConfigurationSource GetWebsocketConfiguration();
    }
}
