using Autofac;
using MessageServer.Services;

namespace MessageServer
{
    /// <summary>
    /// Autofac registration module for MessageServer. Registers the basic services that
    /// MessageServer provides.
    /// </summary>
    public static class MessageServerModule
    {
        /// <summary>
        /// Register services with autofac.
        /// </summary>
        /// <param name="builder">The ContainerBuilder to register services in.</param>
        public static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<AppConfigSettingsProvider>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<WebSocketConnectionService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<MessageSender>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<MessageBus>()
                .As<MessageBus>()
                .SingleInstance();
        }
    }
}
