using System.Collections.Concurrent;
using System.Collections.Generic;
using CommonInteraction;

namespace App
{
    public interface IDictionaryProvider
    {
        public GameTeam GetTeam(ICommandInfo info);
    }
}