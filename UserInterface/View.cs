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
        public event Action<User, bool, Command> ExCommand;
        public Action<User, bool, Answer> RegisterSending() => SendMessage;
        public Action<User> RegisterDelUser() => DeleteUser;
        
        private ITokenProvider provider;
        private IDictionary<User, SocketMessage> userSockets = new Dictionary<User, SocketMessage>();
        private IDictionary<ulong, User> users = new Dictionary<ulong, User>();

        public View(ITokenProvider provider)
        {
            this.provider = provider;
        }
        
        public void Run()
        {
            StartAsync().GetAwaiter().GetResult();
        }

        private async Task StartAsync()
        {
            client = new DiscordSocketClient();
            client.MessageReceived += CommandsHandler;
            client.Log += Log;

            var token = provider.GetToken();

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
            if (msg.Author.IsBot || msg.Content.First() != '!') return Task.CompletedTask;
            var stringsCommand = msg.Content.Remove(0, 1).Split();
            CommandType commandType;
            switch (stringsCommand.First())
            {
                case "rules":
                    commandType = CommandType.Rules;
                    break;
                case "vote":
                    commandType = CommandType.Vote;
                    break;
                case "reg":
                    // members.Add(msg.Author);
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
                    commandType = CommandType.CreateNewGame;
                    break;
                default:
                    msg.Channel.SendMessageAsync("Я не знаю такой команды");
                    return Task.CompletedTask;
            }
            var ctx = new Command(commandType, msg.MentionedUsers.Select(x => x.Username).ToImmutableArray());
            if (!users.Keys.Contains(msg.Author.Id))
            {
                var usr = new User(msg.Author.Id, msg.Channel.Id, msg.Author.Username);
                users[msg.Author.Id] = usr;
                userSockets[usr] = msg;
            }
            var user = users[msg.Author.Id];
            var isCommonChat = msg.Channel.GetType() == typeof(SocketTextChannel);
            ExCommand?.Invoke(user, isCommonChat, ctx);
            return Task.CompletedTask;
        }
        
        private void SendMessage(User user, bool isCommonChat, Answer answer)
        {
            if (isCommonChat)
                userSockets[user].Channel.SendMessageAsync(ParseAnswer(answer));
            else
                userSockets[user].Author.SendMessageAsync(ParseAnswer(answer));
        }

        private void DeleteUser(User user) => userSockets.Remove(user);
        
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
                AnswerType.YouArePeaceful => "Ты мирный! Земля тебе пухом...",
                AnswerType.YouAreMafia => "Ты мафия! не жалей никого...",
                _ => "Я не знаю такой команды;( Давай попробуем еще раз?"
            };
        }
    }
}