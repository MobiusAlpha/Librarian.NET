using System.Text;

namespace Librarian.Core
{
    public class ConnectionStringInfo
    {
        public ConnectionStringInfo(string serverAddress, string initialCatalog, ConnectionAuthType authType, string failoverPartner = "", string domain = "", string username = "", string password = "", string instanceName = "", bool multipleActiveResultSets = true)
        {
            ServerAddress = serverAddress;
            FailoverPartner = failoverPartner;
            InitialCatalog = initialCatalog;
            AuthType = authType;
            Domain = domain;
            Username = username;
            Password = password;
            InstanceName = instanceName;
            MultipleActiveResultSets = multipleActiveResultSets;
        }

        public string ServerAddress { get; set; }
        public string FailoverPartner { get; set; }
        public string InitialCatalog { get; set; }
        public ConnectionAuthType AuthType { get; set; }
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string InstanceName { get; set; }
        public bool MultipleActiveResultSets { get; set; }
        
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append($"Server={ServerAddress}" + (!string.IsNullOrWhiteSpace(InstanceName) ? $"\\{InstanceName}" : string.Empty) + ";");
            builder.Append($"Initial Catalog={InitialCatalog};");

            switch (AuthType)
            {
                case ConnectionAuthType.Trusted:
                    builder.Append($"Integrated Security=True;");
                    break;
                case ConnectionAuthType.UnPw:
                    builder.Append($"User Id={Username};Password={Password};");
                    break;
            }

            if (MultipleActiveResultSets)
            {
                builder.Append("MultipleActiveResultSets=true;");
            }

            if (!string.IsNullOrWhiteSpace(FailoverPartner))
            {
                builder.Append($"Failover Partner={FailoverPartner};");
            }

            return builder.ToString();
        }
    }
}