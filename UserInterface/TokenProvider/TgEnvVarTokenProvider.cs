using System;

namespace UserInterface
{
    public class TgEnvVarTokenProvider
    {
        public string GetToken()
        {
            return Environment.GetEnvironmentVariable("MAFIATGTOKEN", EnvironmentVariableTarget.User);
        }
    }
}