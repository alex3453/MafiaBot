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
            if (msg.Author.IsBot) return Task.CompletedTask;
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
                    msg.Channel.SendMessageAsync(GetHelpMessage());
                    return Task.CompletedTask;
                case "vote":
                    commandType = CommandType.Vote;
                    break;
                case "reg":
                    commandType = CommandType.Reg;
                    break;
                case "kill":
                    commandType = CommandType.Kill;
                    break;
                case "start":
                    commandType = CommandType.Start;
                    break;
                case "createnew":
                    commandType = CommandType.CreateNewGame;
                    break;
                default:
                    msg.Channel.SendMessageAsync("Я не знаю такой команды");
                    return Task.CompletedTask;
            }
            if (!channels.Contains(msg.Channel.Id))
                channels.Add(msg.Channel.Id);
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

        private string GetHelpMessage()
        {
            return "Привет, я бот для игры в мафию, и у меня есть следующие команды:\n" +
                   "!help - выведет данное приветственное сообщение и покажет все команды, если вы вдруг забыли.\n" +
                   "!vote {имя игрока на сервере, лучше через @} - позволяет голосовать во время самой игры.\n" +
                   "!reg - позволяет зарегестрироваться на игру.\n" +
                   "!kill {номер игрока из отправленного вам списка} - позволяет мафии убивать игроков во время игры. " +
                   "Пишется только в личку боту.\n" +
                   "!start - позволяет начать игру.\n" +
                   "!createnew - создает для вас новую игру.\n\n" +
                   "Алгоритм действий следующий:\n" +
                   "1. Создайте новую игру командой !createnew\n" +
                   "2. Все желающие поиграть должны зарегестрироваться, написав команду !reg\n" +
                   "3. Начните игру командой !start\n" +
                   "4. Играйте:)";
        }
        
        private void SendMessage(User user, bool isCommonChat, Answer answer)
        {
            if (isCommonChat)
                userSockets[user].Channel.SendMessageAsync(ParseAnswer(answer));
            else
                userSockets[user].Author.SendMessageAsync(ParseAnswer(answer));
        }

        private void DeleteUserById(ulong userId)
        {
            var user = users[userId];
            users.Remove(userId);
            userSockets.Remove(user);
        }

        private static string ParseAnswer(Answer answer)
        {
            // return answer.AnswerType switch
            // {
            //     AnswerType.GameStarted => "Игра началась",
            //     AnswerType.GetRules => "Вот тебе правила",
            //     AnswerType.MafiaWins => $"Ой-ой...Кажется, {answer.Args[0]} и пистолета в руках не держал. " +
            //                          "Стало очевидно, что это конец...",
            //     AnswerType.PeacefulWins => $"{answer.Args[0]} оказался мафией. Больше никто не умрет.", 
            //     AnswerType.SuccessfullyRegistered => $"{answer.Args[0]}, ты в игре!",
            //     AnswerType.NeedMorePlayer => "Ролей указано больше, чем игроков, перезапустите игру, " +
            //                               "либо добавьте игроков.",
            //     AnswerType.AlreadyRegistered => $"{answer.Args[0]}, ты уже регистрировался...Позови друзей:(",
            //     AnswerType.SuccessfullyVoted => "Твой голос учтён!",
            //     AnswerType.AlreadyVoted => "Ты не можешь голосовать дважды",
            //     AnswerType.EndDay => $"{answer.Args[0]} был выгнан...Отведенная роль - {answer.Args[1]}.Наступает ночь",
            //     AnswerType.DayKill => "Нельзя убивать днем",
            //     AnswerType.EndNight =>$"Ночь забрала с собой {answer.Args[0]}",
            //     AnswerType.NewGame => "И снова все по новой...Добро пожаловать!",
            //     AnswerType.YouArePeaceful => "Ты мирный! Земля тебе пухом...",
            //     AnswerType.YouAreMafia => "Ты мафия! не жалей никого...",
            //     _ => "Я не знаю такой команды;( Давай попробуем еще раз?"
            // };
            return answer.AnswerType switch
            {
                AnswerType.GameStarted => "Игра началась! Сейчас у вас день, игроки уже знают свои роли(они были " +
                                          "высланы в личку). Можете спокойно общаться и пытаться вывести друг друга на " +
                                          "чистую воду, но в первый день велика вероятность ошибиться... Для того чтобы " +
                                          "игра продолжилась, необходимо проголосовать. Игрок вылетает из игры, если за " +
                                          "него проголосовало не меньше половины. Поэтому рекомендую в первую ночь всем " +
                                          "проголосовать за себя, чтобы не вылетел кто-то невинный...)",
                AnswerType.MafiaWins => "Мафия победила",
                AnswerType.PeacefulWins => "Победили мирные",
                AnswerType.SuccessfullyRegistered => $"{answer.Args[0]}, ты в игре!",
                AnswerType.AlreadyRegistered => $"{answer.Args[0]}, ты уже зарегестрировался, одного раза хватит!",
                AnswerType.SuccessfullyVoted => $"{answer.Args[0]}, ты успешно проголосовал за {answer.Args[1]}!",
                AnswerType.AlreadyVoted => $"{answer.Args[0]}, ты уже проголосовал, одного раза хватит!",
                AnswerType.EndDay => "День закончился, начинается ночь!",
                AnswerType.EndNight => "Ночь закончена, начинается день!",
                AnswerType.DayKill => $"{answer.Args[0]} был выгнан.",
                AnswerType.DayAllAlive => "Сегодня никого не выгнали",
                AnswerType.NightKill => $"{answer.Args[0]} умер этой ночью.",
                AnswerType.NightAllAlive => "Все остались живы.",
                AnswerType.NewGame => "Новая игра создана, регистрируйтесь!",
                AnswerType.YouAreMafia => "Ты мафия",
                AnswerType.YouArePeaceful => "Ты мирный",
                AnswerType.OnlyInLocal => "Это можно делать только в личку",
                AnswerType.OnlyInCommon => "Это можно делать только в общем чате",
                AnswerType.GameIsGoing => "Игра уже началась",
                AnswerType.NeedMorePlayers => "Чтобы начать игру нужно больше игроков",
                AnswerType.YouAreNotInGame => $"{answer.Args[1]}, ты уже выбыл из игры!",
                AnswerType.YouCantVoteThisPl => $"{answer.Args[0]}, ты не можешь проголосовать за {answer.Args[1]}!",
                AnswerType.YouCantKillThisPl => "Ты не можешь убить этого игрока!",
                AnswerType.NotTimeToVote => "Сейчас нельзя голосовать!",
                AnswerType.NotTimeToKill => "Сейчас нельзя убивать!",
                AnswerType.EnterNumber => "Введите число",
                AnswerType.IncorrectNumber => "Неверное число",
                AnswerType.YouAreNotMafia => "Вы не мафия!",
                AnswerType.SuccessfullyKilled => "Ваш голос учтён!",
                AnswerType.AlreadyKilled => "Вы уже выбрали свою жертву!",
                AnswerType.NeedToCreateGame => "Сначала надо создать игру",
                AnswerType.IncorrectVote => $"{answer.Args[0]}, нужно ввести имя человека за которого ты хочешь проголосовать!",
                AnswerType.MafiaKilling => "Введи !kill X, и вместо X номер того, кого хочешь убить:\n" +
                                           ParseKillList(answer.Args),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static string ParseKillList(IReadOnlyList<string> killList)
        {
            var res = new StringBuilder();
            for (var i = 0; i < killList.Count; i += 2)
            {
                res.Append(killList[i] + " - ");
                res.Append(killList[i + 1] + "\n");
            }

            return res.ToString();
        }
    }
}