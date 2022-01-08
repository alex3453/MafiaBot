using System;

namespace UserInterface
{
    public class TgEnvVarTokenProvider : ITokenProvider
    {
        public string GetToken()
        {
            return Environment.GetEnvironmentVariable("MAFIATGTOKEN", EnvironmentVariableTarget.User);
        }
    }
}