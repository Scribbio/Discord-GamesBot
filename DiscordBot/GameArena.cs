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
        private static Dictionary<ISocketMessageChannel, IGame> HangmanGames = new Dictionary<ISocketMessageChannel, IGame>();

        [Command("PlayHangman")]
        public async Task HangmanGameAsync()
        {
            if (HangmanGames.ContainsKey(Context.Channel))
            {
                await HangmanGames[Context.Channel].ResetGame();
                return;
            }

            HangmanGames.Add(Context.Channel, new Hangman(Context));
            await HangmanGames[Context.Channel].CreateNewGame();
        }

        [Command("Guess")]
        public async Task GuessALetter(string letter)
        {
            if (!HangmanGames.ContainsKey(Context.Channel) && HangmanGames[Context.Channel].GetType() != typeof(Hangman))
            {
                await ReplyAsync($"You must first start a game of hangman!");
                return;
            }

            if (letter.Length > 1)
            {
                await ReplyAsync($"You can only guess a single letter at a time! Try [!WordIs] to guess a word.");
                return;
            }

            if (HangmanGames[Context.Channel].QuestionAsked)
            {
                await Context.Channel.SendMessageAsync($"Please respond to the question before continuing... {HangmanGames[Context.Channel].Question}");
                return;
            }

            //TODO: Is there a better way to do this?
            Hangman currentGame = HangmanGames[Context.Channel] as Hangman;
            await currentGame.GuessALetter(letter.ToLower()[0]);
        }

        //If the users wants to guess the whole word.
        [Command("WordIs")]
        public async Task GuessTheWord(string guessedWord)
        {
            if (!HangmanGames.ContainsKey(Context.Channel))
            {
                await ReplyAsync($"You must first start a game of hangman!");
                return;
            }

            if (HangmanGames[Context.Channel].QuestionAsked)
            {
                await Context.Channel.SendMessageAsync($"Please respond to the question before continuing... {HangmanGames[Context.Channel].Question}");
                return;
            }

            //TODO: Is there a better way to do this?
            Hangman currentGame = HangmanGames[Context.Channel] as Hangman;
            await currentGame.GuessAWord(guessedWord);
        }

        [Command("Yes")]
        public async Task UserSaidYes()
        {
            if (HangmanGames.ContainsKey(Context.Channel))
            {
                await HangmanGames[Context.Channel].Yes();
                return;
            }
        }

        [Command("No")]
        public async Task UserSaidNo()
        {
            if (HangmanGames.ContainsKey(Context.Channel))
            {
                await HangmanGames[Context.Channel].No();
                return;
            }
        }

        [Command("ResetGame")]
        public async Task UserReset()
        {
            if (!HangmanGames.ContainsKey(Context.Channel))
            {
                await ReplyAsync($"You start a game before you can reset!");
                return;
            }

            if (HangmanGames.ContainsKey(Context.Channel))
            {
                await HangmanGames[Context.Channel].ResetGame();
                return;
            }
        }

        [Command("Quit")]
        public async Task UserQuit()
        {
            if (!HangmanGames.ContainsKey(Context.Channel))
            {
                await ReplyAsync($"You start a game before you can quit!");
                return;
            }

            if (HangmanGames.ContainsKey(Context.Channel))
            {
                HangmanGames.Remove(Context.Channel);
                return;
            }
        }

        [Command("Help")]
        public async Task Help()
        {
            if (!HangmanGames.ContainsKey(Context.Channel))
            {
                await ReplyAsync("```" + Environment.NewLine +
                                 "!PlayHangman - Start a game of hangman. Once a game is started use [!Help] to show the game specific commands.```");
                return;
            }

            if (HangmanGames.ContainsKey(Context.Channel))
            {
                await HangmanGames[Context.Channel].Help();
                return;
            }
        }
    }
}