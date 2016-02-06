using System.Collections.Generic;

namespace DotKafka.Prototype.Common.Config {
    public class SaslConfigs {
        public static readonly string SaslKerberosServiceName = "sasl.kerberos.service.name";

        public static readonly string SaslKerberosServiceNameDoc = "The Kerberos principal name that Kafka runs as. "
                                                                   +
                                                                   "This can be defined either in Kafka's JAAS config or in Kafka's config.";

        public static readonly string SaslKerberosKinitCmd = "sasl.kerberos.kinit.cmd";

        public static readonly string SaslKerberosKinitCmdDoc = "Kerberos kinit command path. "
                                                                + "Default is /usr/bin/kinit";

        public static readonly string DefaultKerberosKinitCmd = "/usr/bin/kinit";

        public static readonly string SaslKerberosTicketRenewWindowFactor = "sasl.kerberos.ticket.renew.window.factor";

        public static readonly string SaslKerberosTicketRenewWindowFactorDoc = "Login thread will sleep until the specified window factor of time from last refresh"
                                                                               +
                                                                               " to ticket's expiry has been reached, at which time it will try to renew the ticket.";

        public static readonly double DefaultKerberosTicketRenewWindowFactor = 0.80;

        public static readonly string SaslKerberosTicketRenewJitter = "sasl.kerberos.ticket.renew.jitter";

        public static readonly string SaslKerberosTicketRenewJitterDoc =
            "Percentage of random jitter added to the renewal time.";

        public static readonly double DefaultKerberosTicketRenewJitter = 0.05;

        public static readonly string SaslKerberosMinTimeBeforeRelogin = "sasl.kerberos.min.time.before.relogin";

        public static readonly string SaslKerberosMinTimeBeforeReloginDoc =
            "Login thread sleep time between refresh attempts.";

        public static readonly long DefaultKerberosMinTimeBeforeRelogin = 1*60*1000L;

        public static readonly string SaslKerberosPrincipalToLocalRules = "sasl.kerberos.principal.to.local.rules";

        public static readonly string SaslKerberosPrincipalToLocalRulesDoc =
            "A list of rules for mapping from principal names to short names (typically operating system usernames). " +
            "The rules are evaluated in order and the first rule that matches a principal name is used to map it to a short name. Any later rules in the list are ignored. " +
            "By default, principal names of the form <username>/<hostname>@<REALM> are mapped to <username>.";

        public static readonly IReadOnlyCollection<string> DefaultSaslKerberosPrincipalToLocalRules = new List<string> {
            "DEFAULT"
        };

        public static void AddClientSaslSupport(ConfigDef config) {
            config.Define(SaslKerberosServiceName, ConfigDef.Type.String, null, ConfigDef.Importance.Medium,
                SaslKerberosServiceNameDoc)
                .Define(SaslKerberosKinitCmd, ConfigDef.Type.String, DefaultKerberosKinitCmd, ConfigDef.Importance.Low,
                    SaslKerberosKinitCmdDoc)
                .Define(SaslKerberosTicketRenewWindowFactor, ConfigDef.Type.Double,
                    DefaultKerberosTicketRenewWindowFactor, ConfigDef.Importance.Low,
                    SaslKerberosTicketRenewWindowFactorDoc)
                .Define(SaslKerberosTicketRenewJitter, ConfigDef.Type.Double, DefaultKerberosTicketRenewJitter,
                    ConfigDef.Importance.Low, SaslKerberosTicketRenewJitterDoc)
                .Define(SaslKerberosMinTimeBeforeRelogin, ConfigDef.Type.Long, DefaultKerberosMinTimeBeforeRelogin,
                    ConfigDef.Importance.Low, SaslKerberosMinTimeBeforeReloginDoc);
        }
    }
}
