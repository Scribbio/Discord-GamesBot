using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class GameArena : ModuleBase<SocketCommandContext>
    {
        private static Dictionary<ISocketMessageChannel, IGame> ActiveGames = new Dictionary<ISocketMessageChannel, IGame>();
        public bool QuestionAsked
        {
            get
            {
                if(ActiveGames.ContainsKey(Context.Channel))
                {
                    return ActiveGames[Context.Channel].Question.Length > 0;
                }
                return false;
            }
        }

        #region Standard Commands
        [Command("Yes")]
        public async Task UserSaidYes()
        {
            if (ActiveGames.ContainsKey(Context.Channel))
            {
                await ActiveGames[Context.Channel].Yes();
                return;
            }

            await Context.Channel.SendMessageAsync("Huh? I didn't ask you anything.");
        }

        [Command("No")]
        public async Task UserSaidNo()
        {
            if (ActiveGames.ContainsKey(Context.Channel))
            {
                await ActiveGames[Context.Channel].No();
                return;
            }

            await Context.Channel.SendMessageAsync("Huh? I didn't ask you anything.");
        }

        [Command("ResetGame")]
        public async Task UserReset()
        {
            if (!ActiveGames.ContainsKey(Context.Channel))
            {
                await ReplyAsync($"You start a game before you can reset!");
                return;
            }

            if (ActiveGames.ContainsKey(Context.Channel))
            {
                await ActiveGames[Context.Channel].ResetGame();
                return;
            }
        }

        [Command("Quit")]
        public async Task UserQuit()
        {
            if (!ActiveGames.ContainsKey(Context.Channel))
            {
                await ReplyAsync($"You start a game before you can quit!");
                return;
            }

            if (ActiveGames.ContainsKey(Context.Channel))
            {
                ActiveGames.Remove(Context.Channel);
                return;
            }
        }

        [Command("Help")]
        public async Task Help()
        {
            if (!ActiveGames.ContainsKey(Context.Channel))
            {
                await ReplyAsync("```" + Environment.NewLine +
                                 "!PlayHangman - Start a game of hangman. Once a game is started use [!Help] to show the game specific commands.```");
                return;
            }

            if (ActiveGames.ContainsKey(Context.Channel))
            {
                await ActiveGames[Context.Channel].Help();
                return;
            }
        }

        #endregion

        #region Blackjack Stuff

        [Command("PlayBlackjack")]
        public async Task BlackjackGameAsync()
        {
            if (ActiveGames.ContainsKey(Context.Channel))
            {
                await ActiveGames[Context.Channel].ResetGame();
                return;
            }

            ActiveGames.Add(Context.Channel, new Blackjack(Context));
            await ActiveGames[Context.Channel].CreateNewGame();
        }

        [Command("Join")]
        public async Task JoinBlackjack()
        {
            Blackjack currentGame = ActiveGames[Context.Channel] as Blackjack;
            await currentGame.JoinTheGame(Context.User);
        }

        [Command("Start")]
        public async Task StartBlackjack()
        {
            Blackjack currentGame = ActiveGames[Context.Channel] as Blackjack;
            await currentGame.StartTheGame();
        }

        #endregion

        #region Hangman Stuff

        [Command("PlayHangman")]
        public async Task HangmanGameAsync()
        {
            if (ActiveGames.ContainsKey(Context.Channel))
            {
                await ActiveGames[Context.Channel].ResetGame();
                return;
            }

            ActiveGames.Add(Context.Channel, new Hangman(Context));
            await ActiveGames[Context.Channel].CreateNewGame();
        }

        [Command("Guess")]
        public async Task GuessALetter(string letter)
        {
            if (!ActiveGames.ContainsKey(Context.Channel) && ActiveGames[Context.Channel].GetType() != typeof(Hangman))
            {
                await ReplyAsync($"You must first start a game of hangman!");
                return;
            }

            if (letter.Length > 1)
            {
                await ReplyAsync($"You can only guess a single letter at a time! Try [!WordIs] to guess a word.");
                return;
            }

            if (QuestionAsked)
            {
                await Context.Channel.SendMessageAsync($"Please respond to the question before continuing... {ActiveGames[Context.Channel].Question}");
                return;
            }

            //TODO: Is there a better way to do this?
            Hangman currentGame = ActiveGames[Context.Channel] as Hangman;
            await currentGame.GuessALetter(letter.ToLower()[0]);
        }

        //If the users wants to guess the whole word.
        [Command("WordIs")]
        public async Task GuessTheWord(string guessedWord)
        {
            if (!ActiveGames.ContainsKey(Context.Channel))
            {
                await ReplyAsync($"You must first start a game of hangman!");
                return;
            }

            if (QuestionAsked)
            {
                await Context.Channel.SendMessageAsync($"Please respond to the question before continuing... {ActiveGames[Context.Channel].Question}");
                return;
            }

            //TODO: Is there a better way to do this?
            Hangman currentGame = ActiveGames[Context.Channel] as Hangman;
            await currentGame.GuessAWord(guessedWord);
        }

        #endregion
    }
}