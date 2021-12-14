using Mafia;

namespace App
{
    public interface IMafiaFactory
    {
        IMafia CreateMafiaFunc(IRoleDistribution roleDistribution);
    }
}