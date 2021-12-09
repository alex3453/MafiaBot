using System;

namespace UserInterface
{
    public class FromEnvVarProvider : ITokenProvider
    {
        public string GetToken()
        {
            return Environment.GetEnvironmentVariable("MAFIATOKEN", EnvironmentVariableTarget.User);
        }
    }
}