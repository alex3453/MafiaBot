using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace UserInterface
{
    public interface ITgErrorHandler
    {
        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken);
    }
}