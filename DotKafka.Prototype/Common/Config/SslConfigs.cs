namespace DotKafka.Prototype.Common.Config {
    public class SslConfigs {
        public static readonly string PrincipalBuilderClassConfig = "principal.builder.class";

        public static readonly string PrincipalBuilderClassDoc =
            "The fully qualified name of a class that implements the PrincipalBuilder interface, " +
            "which is currently used to build the Principal for connections with the SSL SecurityProtocol. " +
            "Default is DefaultPrincipalBuilder.";

        public static readonly string DefaultPrincipalBuilderClass =
            "org.apache.kafka.common.security.auth.DefaultPrincipalBuilder";

        public static readonly string SslProtocolConfig = "ssl.protocol";

        public static readonly string SslProtocolDoc = "The SSL protocol used to generate the SSLContext. "
                                                       + "Default setting is TLS, which is fine for most cases. "
                                                       +
                                                       "Allowed values in recent JVMs are TLS, TLSv1.1 and TLSv1.2. SSL, SSLv2 and SSLv3 "
                                                       +
                                                       "may be supported in older JVMs, but their usage is discouraged due to known security vulnerabilities.";

        public static readonly string DefaultSslProtocol = "TLS";

        public static readonly string SslProviderConfig = "ssl.provider";

        public static readonly string SslProviderDoc =
            "The name of the security provider used for SSL connections. Default value is the default security provider of the JVM.";

        public static readonly string SslCipherSuitesConfig = "ssl.cipher.suites";

        public static readonly string SslCipherSuitesDoc = "A list of cipher suites. This is a named combination of authentication, encryption, MAC and key exchange algorithm used to negotiate the security settings for a network connection using TLS or SSL network protocol."
                                                           + "By default all the available cipher suites are supported.";

        public static readonly string SslEnabledProtocolsConfig = "ssl.enabled.protocols";

        public static readonly string SslEnabledProtocolsDoc = "The list of protocols enabled for SSL connections. "
                                                               + "TLSv1.2, TLSv1.1 and TLSv1 are enabled by default.";

        public static readonly string DefaultSslEnabledProtocols = "TLSv1.2,TLSv1.1,TLSv1";

        public static readonly string SslKeystoreTypeConfig = "ssl.keystore.type";

        public static readonly string SslKeystoreTypeDoc = "The file format of the key store file. "
                                                           + "This is optional for client. Default value is JKS";

        public static readonly string DefaultSslKeystoreType = "JKS";

        public static readonly string SslKeystoreLocationConfig = "ssl.keystore.location";

        public static readonly string SslKeystoreLocationDoc = "The location of the key store file. "
                                                               +
                                                               "This is optional for client and can be used for two-way authentication for client.";

        public static readonly string SslKeystorePasswordConfig = "ssl.keystore.password";

        public static readonly string SslKeystorePasswordDoc = "The store password for the key store file."
                                                               +
                                                               "This is optional for client and only needed if ssl.keystore.location is configured. ";

        public static readonly string SslKeyPasswordConfig = "ssl.key.password";

        public static readonly string SslKeyPasswordDoc = "The password of the private key in the key store file. "
                                                          + "This is optional for client.";

        public static readonly string SslTruststoreTypeConfig = "ssl.truststore.type";

        public static readonly string SslTruststoreTypeDoc = "The file format of the trust store file. "
                                                             + "Default value is JKS.";

        public static readonly string DefaultSslTruststoreType = "JKS";

        public static readonly string SslTruststoreLocationConfig = "ssl.truststore.location";
        public static readonly string SslTruststoreLocationDoc = "The location of the trust store file. ";

        public static readonly string SslTruststorePasswordConfig = "ssl.truststore.password";
        public static readonly string SslTruststorePasswordDoc = "The password for the trust store file. ";

        public static readonly string SslKeymanagerAlgorithmConfig = "ssl.keymanager.algorithm";

        public static readonly string SslKeymanagerAlgorithmDoc = "The algorithm used by key manager factory for SSL connections. "
                                                                  +
                                                                  "Default value is the key manager factory algorithm configured for the Java Virtual Machine.";

        //ToDo: Look for KeyManagerFactory equivalent
        public static readonly string DefaultSslKeymangerAlgorithm = "Pending to implement";
            //KeyManagerFactory.getDefaultAlgorithm();

        public static readonly string SslTrustmanagerAlgorithmConfig = "ssl.trustmanager.algorithm";

        public static readonly string SslTrustmanagerAlgorithmDoc = "The algorithm used by trust manager factory for SSL connections. "
                                                                    +
                                                                    "Default value is the trust manager factory algorithm configured for the Java Virtual Machine.";

        //ToDo: Look for TrustManagerFactory equivalent
        public static readonly string DefaultSslTrustmanagerAlgorithm = "Pending to implement";
            //TrustManagerFactory.getDefaultAlgorithm();

        public static readonly string SslEndpointIdentificationAlgorithmConfig = "ssl.endpoint.identification.algorithm";

        public static readonly string SslEndpointIdentificationAlgorithmDoc =
            "The endpoint identification algorithm to validate server hostname using server certificate. ";

        public static readonly string SslClientAuthConfig = "ssl.client.auth";

        public static readonly string SslClientAuthDoc = "Configures kafka broker to request client authentication."
                                                         + " The following settings are common: "
                                                         + " <ul>"
                                                         +
                                                         " <li><code>ssl.want.client.auth=required</code> If set to required"
                                                         + " client authentication is required."
                                                         +
                                                         " <li><code>ssl.client.auth=requested</code> This means client authentication is optional."
                                                         +
                                                         " unlike requested , if this option is set client can choose not to provide authentication information about itself"
                                                         +
                                                         " <li><code>ssl.client.auth=none</code> This means client authentication is not needed.";

        public static void AddClientSslSupport(ConfigDef config) {
            config.Define(SslProtocolConfig, ConfigDef.Type.String, DefaultSslProtocol,
                ConfigDef.Importance.Medium, SslProtocolDoc)
                .Define(SslProviderConfig, ConfigDef.Type.String, null, ConfigDef.Importance.Medium,
                    SslProviderDoc)
                .Define(SslCipherSuitesConfig, ConfigDef.Type.List, null, ConfigDef.Importance.Low,
                    SslCipherSuitesDoc)
                .Define(SslEnabledProtocolsConfig, ConfigDef.Type.List, DefaultSslEnabledProtocols,
                    ConfigDef.Importance.Medium, SslEnabledProtocolsDoc)
                .Define(SslKeystoreTypeConfig, ConfigDef.Type.String, DefaultSslKeystoreType,
                    ConfigDef.Importance.Medium, SslKeystoreTypeDoc)
                .Define(SslKeystoreLocationConfig, ConfigDef.Type.String, null, ConfigDef.Importance.High,
                    SslKeystoreLocationDoc)
                .Define(SslKeystorePasswordConfig, ConfigDef.Type.Password, null, ConfigDef.Importance.High,
                    SslKeystorePasswordDoc)
                .Define(SslKeyPasswordConfig, ConfigDef.Type.Password, null, ConfigDef.Importance.High,
                    SslKeyPasswordDoc)
                .Define(SslTruststoreTypeConfig, ConfigDef.Type.String, DefaultSslTruststoreType,
                    ConfigDef.Importance.Medium, SslTruststoreTypeDoc)
                .Define(SslTruststoreLocationConfig, ConfigDef.Type.String, null, ConfigDef.Importance.High,
                    SslTruststoreLocationDoc)
                .Define(SslTruststorePasswordConfig, ConfigDef.Type.Password, null, ConfigDef.Importance.High,
                    SslTruststorePasswordDoc)
                .Define(SslKeymanagerAlgorithmConfig, ConfigDef.Type.String,
                    DefaultSslKeymangerAlgorithm, ConfigDef.Importance.Low,
                    SslKeymanagerAlgorithmDoc)
                .Define(SslTrustmanagerAlgorithmConfig, ConfigDef.Type.String,
                    DefaultSslTrustmanagerAlgorithm, ConfigDef.Importance.Low,
                    SslTrustmanagerAlgorithmDoc)
                .Define(SslEndpointIdentificationAlgorithmConfig, ConfigDef.Type.String, null,
                    ConfigDef.Importance.Low, SslEndpointIdentificationAlgorithmDoc);
        }
    }
}
