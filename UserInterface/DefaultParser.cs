﻿using System;
using System.Collections.Generic;
using System.Text;
using CommonInteraction;

namespace UserInterface
{
    public class DefaultAnswers : IParserAnswers
    {
        private string help, startGame, mafiaWins, peacefulWins, successfullyRegistered, alreadyRegistered,
            successfullyVoted, alreadyVoted, endDay, endNight, dayKill, dayAllAlive, nightKill, nightAllAlive,
            newGame, youAreMafia, youArePeaceful, onlyInLocal, onlyInCommon, gameIsGoing, needMorePlayers,
            youAreNotInGame, youCantVoteThisPl, youCantKillThisPl, notTimeToVote, notTimeToKill, enterNumber,
            incorrectNumber, youAreNotMafia, successfullyKilled, alreadyKilled, needToCreateGame, mafiaKilling,
            incorrectVote, unknownCommand;
        public DefaultAnswers()
        {
            help = "Привет, я *бот* для игры в *мафию*, и у меня есть следующие команды:\n" +
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
            mafiaWins = "Игра окончена. Мафия победила. Да будет пир в честь павших и победивших.";
            peacefulWins = "Игра окончена. На сей раз победа за мирными. " +
                           "Наконец-то в этом городе воцарил мир и спокойствие...";
            successfullyRegistered = "**{0}**, добро пожаловать в игру!";
            alreadyRegistered ="**{0}**, не пудри мне мозги. Я уже понял, что ты хочешь играть. Хватит регистрироваться.";
            successfullyVoted = "**{0}**, твой голос отдан за **{1}**. Ты точно хорошо подумал?";
            alreadyVoted = "**{0}**, думал незаметно проголосовать второй раз? Увы и ах...";
            endDay = "Наступила __ночь__ и все затихло... Перестали петь птицы, погасли огни в домах... " +
                     "И только слышно иногда, как где-то раздаются глухие выстрелы...";
            endNight = "С появлением солнца на небе новый __день__ сменил кровавую ночь, " +
                       "и тот, кто еще смог открыть утром глаза, радостно пялится на солнышко " +
                       "и благодарит Бога за то, что его не коснулись события прошедшей ночи...";
            dayKill = "**{0}** был изгнан. У него есть последнее слово и возможность рассказать всем, " +
                      "какая роль в этой жизни была ему отведена.";
            dayAllAlive = "Судьба осталась благосклонна ко всем - город ночует прежним составом. " +
                          "Изменится ли что-то после ночи?";
            nightKill = "Злодейка ночь забрала с собой **{0}**. У кого-то есть идеи, чьих рук дело?";
            nightAllAlive = "Неожиданно, но факт - никто не погиб этой ночью.";
            newGame = "Новая игра создана. Кто хочет поиграть?;)";
            youAreMafia = "Ты *мафия*. Определенно знаешь, чего хочешь от этой жизни.";
            youArePeaceful = "Ты *мирный*...Ох как я тебе не завидую...";
            onlyInLocal = "Не обязательно орать об этом на весь город. Лучше написать личным письмом. " +
                          "Сохрани приватность информации...";
            onlyInCommon =  "Нет, тут это делать не будем. Если сильно хочется - можно уйти в общий чат.";
            needMorePlayers = "С друзьями играть интереснее. Тормошите и зовите их. Вас сейчас слишком мало.";
            gameIsGoing = "Игра уже идет, вы чего?";
            youAreNotInGame = "**{0}**, мертвым слова не давали.";
            youCantVoteThisPl = "**{0}**, хватит пытаться сделать невозможное. За **{1}** нельзя голосовать.";
            youCantKillThisPl = "Убить **{0}** не получится. Подумай насчет другой жертвы.";
            notTimeToVote = "Под покровом ночи не проголосуешь.";
            notTimeToKill = "Убивать у всех на виду? Слабо, я ожидал большего.";
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

        public string ParseAnswer(Answer answer)
        {
            return answer.AnswerType switch
            {
                AnswerType.GameStarted => startGame,
                AnswerType.MafiaWins => mafiaWins,
                AnswerType.PeacefulWins => peacefulWins,
                AnswerType.SuccessfullyRegistered => string.Format(successfullyRegistered, answer.Args[0]),
                AnswerType.AlreadyRegistered => string.Format(alreadyRegistered, answer.Args[0]),
                AnswerType.SuccessfullyVoted => string.Format(successfullyVoted, answer.Args[0], answer.Args[1]),
                AnswerType.AlreadyVoted => string.Format(alreadyVoted, answer.Args[0]),
                AnswerType.EndDay => endDay,
                AnswerType.EndNight => endNight,
                AnswerType.DayKill => string.Format(dayKill, answer.Args[0]),
                AnswerType.DayAllAlive => dayAllAlive,
                AnswerType.NightKill =>string.Format(nightKill, answer.Args[0]),
                AnswerType.NightAllAlive => nightAllAlive,
                AnswerType.NewGame => newGame,
                AnswerType.YouAreMafia => youAreMafia,
                AnswerType.YouArePeaceful => youArePeaceful,
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
                AnswerType.SuccessfullyKilled =>string.Format(successfullyKilled, answer.Args[0]),
                AnswerType.AlreadyKilled => alreadyKilled,
                AnswerType.NeedToCreateGame => needToCreateGame,
                AnswerType.IncorrectVote => string.Format(incorrectVote, answer.Args[0]),
                AnswerType.MafiaKilling => mafiaKilling + ParseKillList(answer.Args),
                AnswerType.GetHelp => help,
                AnswerType.Unknown => unknownCommand,
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