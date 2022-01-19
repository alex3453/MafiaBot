﻿using System;
using CommonInteraction;

namespace UserInterface
{
    public class BalabobaGenerator : IAnswerGenerator
    {
        private readonly Balaboba balaboba;

        private readonly string hi, algo, startGame, mafiaWins, peacefulWins, successfullyRegistered, 
            alreadyRegistered, successfullyVoted, alreadyVoted, endDay, endNight, dayKill, dayAllAlive, nightKill, 
            nightAllAlive, newGame, onlyInLocal, onlyInCommon, gameIsGoing,
            needMorePlayers, youAreNotInGame, youCantVoteThisPl, youCantKillThisPl, notTimeToVote, notTimeToKill, 
            enterNumber, incorrectNumber, youAreNotMafia, successfullyKilled, alreadyKilled, needToCreateGame, 
            mafiaKilling, incorrectVote, unknownCommand, tellRole;
        public BalabobaGenerator(Balaboba balaboba)
        {
            this.balaboba = balaboba;
            hi = "Привет, я *бот* для игры в *мафию*, и у меня есть следующие команды:\n";
            algo = "1. Все желающие поиграть должны зарегестрироваться, написав команду !reg\n" +
                   "2. Начните игру командой !start\n" +
                   "3. Играйте:)";
            mafiaWins = "Игра окончена. Мафия победила.";
            peacefulWins = "Игра окончена. На сей раз победа за мирными. " +
                           "Наконец-то в этом городе воцарил мир и спокойствие...";
            successfullyRegistered = "**{0}**, добро пожаловать в игру!";
            alreadyRegistered ="**{0}**, не пудри мне мозги. Я уже понял, что ты хочешь играть. Хватит регистрироваться.";
            successfullyVoted = "**{0}**, твой голос отдан за **{1}**.";
            alreadyVoted = "**{0}**, думал незаметно проголосовать второй раз? Увы и ах...";
            endDay = "Наступила ночь...";
            endNight = "Настал новый день.";
            dayKill = "**{0}** был повешен.";
            dayAllAlive = "Никого не повесили.";
            nightKill = "Ночью убили **{0}**.";
            nightAllAlive = "Неожиданно, но факт - никто не погиб этой ночью.";
            newGame = "Новая игра создана. Кто хочет поиграть?;)";
            tellRole = "Ваша роль: {0}";
            onlyInLocal = "Не обязательно орать об этом на весь город. Лучше написать личным письмом. " +
                          "Сохрани приватность информации...";
            onlyInCommon =  "Нет, тут это делать не будем. Если сильно хочется - можно уйти в общий чат.";
            needMorePlayers = "С друзьями играть интереснее. Тормошите и зовите их. Вас сейчас слишком мало.";
            gameIsGoing = "Игра уже идет, вы чего?";
            youAreNotInGame = "**{0}**, ты не в игре. Чо ннада?";
            youCantVoteThisPl = "**{0}**, хватит пытаться сделать невозможное. За **{1}** нельзя голосовать.";
            youCantKillThisPl = "Убить **{0}** не получится. Подумай насчет другой жертвы.";
            notTimeToVote = "Сейчас не время голосовать.";
            notTimeToKill = "Сейчас не время убивать...";
            enterNumber = "Я, вроде, просил ввести число?";
            incorrectNumber = "Ты что-то сделал не так. Число неверное.";
            youAreNotMafia = "Не борзей. Ты не мафия. Меня не одурачить.";
            successfullyKilled = "**{0}** - интересная жертва. Я учел.";
            alreadyKilled = "В следующую ночь выберешь другую жертву, успокойся. Поздно спохватился.";
            needToCreateGame = "Создйте новую игру, пожалуйста. Это правда нужно.";
            mafiaKilling = "Введи !kill {Номер того, кого хочешь убить}:\n";
            incorrectVote = "**{0}**, я не умею читать мысли. Попробуй все таки " +
                            "ввести имя - тогда мы сможем понять друг друга.";
            startGame = "Игра началась! Сейчас у вас __день__, игроки уже знают свои роли(они были " +
                        "высланы в личку). Можете спокойно общаться и пытаться вывести друг друга на " +
                        "чистую воду, но в первый день велика вероятность ошибиться... Для того чтобы " +
                        "игра продолжилась, необходимо проголосовать. Игрок вылетает из игры, если за " +
                        "него проголосовало не меньше половины. Поэтому рекомендую в первую ночь всем " +
                        "проголосовать за себя, чтобы не вылетел кто-то невинный...)";
            unknownCommand = "Кажется, мы друг друга не поняли...Я таких команд не знаю:(";
        }
        public override string GenerateAnswer(Answer answer)
        {
            return answer.AnswerType switch
            {
                AnswerType.GameStarted => startGame,
                AnswerType.MafiaWins => balaboba.GetAnswer(mafiaWins).Result,
                AnswerType.PeacefulWins => balaboba.GetAnswer(peacefulWins).Result,
                AnswerType.SuccessfullyRegistered => string.Format(successfullyRegistered, answer.Args[0]),
                AnswerType.AlreadyRegistered => string.Format(alreadyRegistered, answer.Args[0]),
                AnswerType.SuccessfullyVoted => string.Format(successfullyVoted, answer.Args[0], answer.Args[1]),
                AnswerType.AlreadyVoted => string.Format(alreadyVoted, answer.Args[0]),
                AnswerType.EndDay => balaboba.GetAnswer(endDay).Result,
                AnswerType.EndNight => balaboba.GetAnswer(endNight).Result,
                AnswerType.DayKill => balaboba.GetAnswer(string.Format(dayKill, answer.Args[0])).Result,
                AnswerType.DayAllAlive => balaboba.GetAnswer(dayAllAlive).Result,
                AnswerType.NightKill =>balaboba.GetAnswer(string.Format(nightKill, answer.Args[0])).Result,
                AnswerType.NightAllAlive => nightAllAlive,
                AnswerType.NewGame => newGame,
                AnswerType.TellRole => string.Format(tellRole, answer.Args[0]),
                AnswerType.OnlyInLocal => onlyInLocal,
                AnswerType.OnlyInCommon =>onlyInCommon,
                AnswerType.GameIsGoing => gameIsGoing,
                AnswerType.NeedMorePlayers => needMorePlayers,
                AnswerType.YouAreNotInGame => string.Format(youAreNotInGame, answer.Args[0]),
                AnswerType.YouCantVoteThisPl => string.Format(youCantVoteThisPl, answer.Args[0], answer.Args[1]),
                AnswerType.YouCantKillThisPl => string.Format(youCantKillThisPl, answer.Args[0]),
                AnswerType.NotTimeToVote => notTimeToVote,
                AnswerType.NotTimeToKill => notTimeToKill,
                AnswerType.EnterNumber => enterNumber,
                AnswerType.IncorrectNumber => incorrectNumber,
                AnswerType.YouAreNotMafia => youAreNotMafia,
                AnswerType.SuccessfullyKilled =>string.Format(successfullyKilled, answer.Args[1]),
                AnswerType.AlreadyKilled => alreadyKilled,
                AnswerType.NeedToCreateGame => needToCreateGame,
                AnswerType.IncorrectVote => string.Format(incorrectVote, answer.Args[0]),
                AnswerType.MafiaKilling => mafiaKilling + GenerateKillList(answer.Args),
                AnswerType.GetHelp => hi + answer.Args[0] + algo,
                AnswerType.Unknown => unknownCommand,
                AnswerType.ChangeMod => $"Режим: {answer.Args[0]}",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}