using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using CommonInteraction;

namespace UserInterface
{
    public class View 
    {
        DiscordSocketClient client;
        public event Action<Command> Notify;
        public Action<Answer> Register() => SendMessage;
        private HashSet<SocketUser> members = new();
        private SocketMessage message;

        public void Run()
        {
            StartAsync().GetAwaiter().GetResult();
        }

        private async Task StartAsync()
        {
            client = new DiscordSocketClient();
            client.MessageReceived += CommandsHandler;
            client.Log += Log;

            var token = Environment.GetEnvironmentVariable("MAFIATOKEN", EnvironmentVariableTarget.User);

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

           
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task CommandsHandler(SocketMessage msg)
        {
            message = msg; 
            if (message.Author.IsBot || message.Content.First() != '!') return Task.CompletedTask;
            var stringsCommand = message.Content.Remove(0, 1).Split();
            var commandType = CommandType.None;
            switch (stringsCommand.First())
            {
                case "rules":
                    commandType = CommandType.Rules;
                    break;
                case "vote":
                    commandType = CommandType.Vote;
                    break;
                case "reg":
                    members.Add(msg.Author);
                    commandType = CommandType.Reg;
                    break;
                case "kill" when msg.Channel.GetType() == typeof(SocketTextChannel):
                    msg.Channel.SendMessageAsync("Нельзя убивать в текстовом канале. Пойдём лучше в лс:)");
                    return Task.CompletedTask;
                case "kill":
                    commandType = CommandType.Kill;
                    break;
                case "start":
                    commandType = CommandType.Start;
                    break;
                case "startnew":
                    commandType = CommandType.StartNewGame;
                    break;
            }
            var ctx = new Command(commandType, message.MentionedUsers.Select(x => x.Username).ToImmutableArray(),
                message.Author.Username);
            Notify?.Invoke(ctx);
            return Task.CompletedTask;
        }
        
        private void SendMessage(Answer answer)
        {
            if (answer.NeedToInteract)
                message.Channel.SendMessageAsync(ParseAnswer(answer));
            if (answer.AnswerType is not AnswerType.GameStart) return;
            foreach (var name in answer.Dict.Keys)
                members.First(x => x.Username == name).SendMessageAsync($"Ты {answer.Dict[name]}");
        }

        private static string ParseAnswer(Answer answer)
        {
            return answer.AnswerType switch
            {
                AnswerType.GameStart => "Игра началась",
                AnswerType.GetRules => "Вот тебе правила",
                AnswerType.MafiaWins => $"Ой-ой...Кажется, {answer.Args[0]} и пистолета в руках не держал. " +
                                     "Стало очевидно, что это конец...",
                AnswerType.PeacefulWins => $"{answer.Args[0]} оказался мафией. Больше никто не умрет.", 
                AnswerType.SuccessfullyRegistered => $"{answer.Args[0]}, ты в игре!",
                AnswerType.NeedMorePlayer => "Ролей указано больше, чем игроков, перезапустите игру, " +
                                          "либо добавьте игроков.",
                AnswerType.UnsuccessfullyRegistered => $"{answer.Args[0]}, ты уже регистрировался...Позови друзей:(",
                AnswerType.SuccessfullyVoted => "Твой голос учтён!",
                AnswerType.UnsuccessfullyVoted => "Ты не можешь голосовать дважды",
                AnswerType.EndDay => $"{answer.Args[0]} был выгнан...Отведенная роль - {answer.Args[1]}.Наступает ночь",
                AnswerType.DayKill => "Нельзя убивать днем",
                AnswerType.NotMafia => "Я не могу этого допустить:( Ты не мафия",
                AnswerType.EndNight =>$"Ночь забрала с собой {answer.Args[0]}",
                AnswerType.NewGame => "И снова все по новой...Добро пожаловать!",
                _ => "Я не знаю такой команды;( Давай попробуем еще раз?"
            };
        }
    }
}