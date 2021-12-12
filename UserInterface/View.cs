using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
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
        public Action<ulong> RegisterDelUser() => DeleteUserById;
        
        private ITokenProvider provider;
        private IDictionary<User, SocketMessage> userSockets = new Dictionary<User, SocketMessage>();
        private IDictionary<ulong, User> users = new Dictionary<ulong, User>();
        private ISet<ulong> channels = new HashSet<ulong>();
        private IParserAnswers answers;

        public View(ITokenProvider provider)
        {
            this.provider = provider;
            answers = new DefaultAnswers();
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
            if (msg.Author.IsBot || !msg.Content.Any()) return Task.CompletedTask;
            if (msg.Content.First() != '!')
            {
                if (!channels.Contains(msg.Channel.Id))
                {
                    channels.Add(msg.Channel.Id);
                    msg.Channel.SendMessageAsync("Привет, я бот для игры в мафию, напишите !help");
                }
                return Task.CompletedTask;
            }
            var stringsCommand = msg.Content.Remove(0, 1).Split();
            CommandType commandType;
            switch (stringsCommand.First())
            {
                case "help":
                case "рудз":
                    msg.Channel.SendMessageAsync(answers.Help);
                    return Task.CompletedTask;
                case "vote":
                case "мщеу":
                    commandType = CommandType.Vote;
                    break;
                case "reg":
                case "куп":
                    commandType = CommandType.Reg;
                    break;
                case "kill":
                case "лшдд":
                    commandType = CommandType.Kill;
                    break;
                case "start":
                case "ыефке":
                    commandType = CommandType.Start;
                    break;
                case "createnew":
                case "скуфеутуц":
                    commandType = CommandType.CreateNewGame;
                    break;
                default:
                    msg.Channel.SendMessageAsync("Кажется, мы друг друга не поняли...Я таких команд не знаю:(");
                    return Task.CompletedTask;
            }
            if (!channels.Contains(msg.Channel.Id))
                channels.Add(msg.Channel.Id);
            var ctx = new Command(commandType, 
                msg.MentionedUsers.Select(x => x.Username).ToImmutableArray(), stringsCommand.Skip(1).ToList());
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
                userSockets[user].Channel.SendMessageAsync(answers.ParseAnswer(answer));
            else
                userSockets[user].Author.SendMessageAsync(answers.ParseAnswer(answer));
        }

        private void DeleteUserById(ulong userId)
        {
            var user = users[userId];
            users.Remove(userId);
            userSockets.Remove(user);
        }
    }
}