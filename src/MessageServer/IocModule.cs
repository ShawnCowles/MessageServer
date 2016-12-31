using Autofac;
using MessageServer.Services;

namespace MessageServer
{
    public static class IocModule
    {
        public static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<AppConfigSettingsProvider>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<WebSocketConnectionService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<MessageBus>().As<MessageBus>();
        }
    }
}
