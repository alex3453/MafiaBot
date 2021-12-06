using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Discord;
using Discord.WebSocket;
using Answers;

namespace Bot
{
    public class Bot
    {
        DiscordSocketClient client;
        private App application;
        private HashSet<SocketUser> members = new HashSet<SocketUser>();

        public async Task MainAsync()
        {
            client = new DiscordSocketClient();
            client.MessageReceived += CommandsHandler;
            client.Log += Log;
            application = new App();

            var token = Environment.GetEnvironmentVariable("MAFIATOKEN", EnvironmentVariableTarget.User);

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            Console.ReadLine();
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task CommandsHandler(SocketMessage msg)
        {
            if (msg.Author.IsBot || msg.Content.First() != '!') return Task.CompletedTask;
            var stringsCommand = msg.Content.Remove(0, 1).Split();
            var command = stringsCommand.First();
            ICommand commandClass = null;
            switch (command)
            {
                case "rules":
                    commandClass = new RulesCommand();
                    break;
                case "vote":
                    commandClass = new VoteCommand();
                    break;
                case "reg":
                    members.Add(msg.Author);
                    commandClass = new RegCommand();
                    break;
                case "kill" when msg.Channel.GetType() == typeof(SocketTextChannel):
                    msg.Channel.SendMessageAsync("Нельзя убивать в текстовом канале. Пойдём лучше в лс:)");
                    return Task.CompletedTask;
                case "kill":
                    commandClass = new KillCommand();
                    break;
                case "start":
                    commandClass = new StartCommand();
                    break;
            }
            var ctx = new CommandContext(commandClass, msg.Author.Id,
                msg.MentionedUsers.Select(x => x.Username).ToImmutableArray(),
                msg.Author.Username);
            SendMessage(application.ReproduceСommand(ctx), msg);
            return Task.CompletedTask;
        }
        
        private void SendMessage(Answer answer, SocketMessage msg)
        {
            if (answer.NeedToInteract)
                msg.Channel.SendMessageAsync(ParseAnswer(answer));
            if (!(answer.AnswerType is AnswerType.GameStart)) return;
            foreach (var str in answer.Args)
            {
                var info = str.Split();
                var role = info.Last();
                var name = string.Join(' ', info.Take(info.Length - 1));
                members.First(x => x.Username == name).SendMessageAsync($"Ты {role}");
            }
        }

        private static string ParseAnswer(Answer answer)
        {
            return answer.AnswerType switch
            {
                AnswerType.GameStart => "Игра началась",
                AnswerType.GetRules => "Вот тебе правила",
                AnswerType.MafiaWins => $"Ой-ой...Кажется, {answer.Args.First()} и пистолета в руках не держал. " +
                                     "Стало очевидно, что это конец...",
                AnswerType.PeacefulWins => $"{answer.Args.First()} оказался мафией. Больше никто не умрет.", 
                AnswerType.SuccessfullyRegistered => $"{answer.Args.First()}, ты в игре!",
                AnswerType.NeedMorePlayer => "Ролей указано больше, чем игроков, перезапустите игру, " +
                                          "либо добавьте игроков.",
                AnswerType.UnsuccessfullyRegistered => $"{answer.Args.First()}, ты уже регистрировался...Позови друзей:(",
                AnswerType.SuccessfullyVoted => "Твой голос учтён!",
                AnswerType.UnsuccessfullyVoted => "Ты не можешь голосовать дважды",
                AnswerType.EndDay => "Наступает ночь",
                AnswerType.DayKill => "Нельзя убивать днем",
                AnswerType.NotMafia => "Я не могу этого допустить:( Ты не мафия",
                _ => "Я не знаю такой команды;( Давай попробуем еще раз?"
            };
        }
    }
}