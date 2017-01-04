# MessageServer #

Shawn Cowles, 2016

A framework for a message based websocket service. Runnable on both windows and linux (via mono-service).
Annotated for Typescript interface generation via TypeLite.

--------------------------------------------

Code licensed under the MIT License. See LICENSE.txt for details.



--------------------------------------------
MessageServer uses both SuperWebSocket and log4net, wic
Example configuration file.

<configuration>
    <configSections>
        <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
        <section name="superSocket" type="SuperSocket.SocketEngine.Configuration.SocketServiceConfig, SuperSocket.SocketEngine" />
    </configSections>
    <startup>
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5.2" />
    </startup>
    <runtime>
        <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
            <dependentAssembly>
                <assemblyIdentity name="log4net" publicKeyToken="669e0ddf0bb1aa2a" culture="neutral" />
                <bindingRedirect oldVersion="0.0.0.0-2.0.6.0" newVersion="2.0.6.0" />
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name="Newtonsoft.Json" publicKeyToken="30ad4fe6b2a6aeed" culture="neutral" />
                <bindingRedirect oldVersion="0.0.0.0-9.0.0.0" newVersion="9.0.0.0" />
            </dependentAssembly>
        </assemblyBinding>
    </runtime>
    <log4net>
        <appender name="ConsoleAppender" type="log4net.Appender.ConsoleAppender">
            <layout type="log4net.Layout.PatternLayout">
                <conversionPattern value="%date [%thread] %-5level %logger [%ndc] - %message%newline" />
            </layout>
        </appender>
        <appender name="FileAppender" type="log4net.Appender.FileAppender">
            <file value="log.txt" />
            <appendToFile value="true" />
            <layout type="log4net.Layout.PatternLayout">
                <conversionPattern value="%date [%thread] %-5level %logger [%ndc] - %message%newline" />
            </layout>
        </appender>
        <root>
            <level value="ALL" />
            <appender-ref ref="FileAppender" />
            <appender-ref ref="ConsoleAppender" />
        </root>
    </log4net>
    <superSocket>
        <servers>
            <server name="MessageServer" ip="Any" port="20014" maxRequestLength="8192">
            </server>
        
            <!-- For TLS -->
            <!-- <server name="MessageServer" ip="Any" port="20014" maxRequestLength="8192" security="tls">
                <certificate filePath="certificate.pfx" password="password" />
            </server> -->
        </servers>
    </superSocket>
</configuration>