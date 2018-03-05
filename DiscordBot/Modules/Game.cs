using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class Game : ModuleBase<SocketCommandContext>
    {
        private static Dictionary<ISocketMessageChannel, Hangman> HangmanGames = new Dictionary<ISocketMessageChannel, Hangman>();

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
            if (!HangmanGames.ContainsKey(Context.Channel))
            {
                await ReplyAsync($"You must first start a game of hangman!");
                return;
            }

            if (letter.Length > 1)
            {
                await ReplyAsync($"You can only guess a single letter at a time! Try [!WordIs] to guess a word.");
                return;
            }

            await HangmanGames[Context.Channel].GuessALetter(letter[0]);            
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

            await HangmanGames[Context.Channel].GuessAWord(guessedWord);
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
                HangmanGames[Context.Channel].Help();
                return;
            }
        }
    }
}