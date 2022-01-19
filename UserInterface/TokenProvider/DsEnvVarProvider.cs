using System;

namespace UserInterface
{
    public class DsEnvVarProvider
    {
        public string GetToken()
        {
            return Environment.GetEnvironmentVariable("MAFIATOKEN");
        }
    }
}