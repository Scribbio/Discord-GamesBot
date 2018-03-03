using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class Hangman : ModuleBase<SocketCommandContext>
    {
        private static Game NewGame;

        [Command("NewGame")]
        public async Task NewGameAsync()
        {
            //Probalby should of just done this in the constructor...
            NewGame = new Game
            {
                Word = GetNewWord(),
                SortedWord = new List<string>(),
                GuesedLetters = new List<string>(),
                DiscoveredSoFar = new List<string>(),
                GuessRemain = 8,
                ReplayAsked = false,
            };

            //Trying an Idea, Get all the Letters in the word then Put them in a list, This way I can print the ones I care about.
            foreach (var c in NewGame.Word)
            {
                NewGame.SortedWord.Add(c.ToString());
                //And I wanna blank out That SoFar String list while im at it to display placement.
                NewGame.DiscoveredSoFar.Add("-");
            }

            await ReplyAsync($"I Got a new word for you, its {NewGame.Word.Length} Characters Long");
        }
        [Command("Guessing")]
        public async Task GuessALetter(string c)
        {
            NewGame.GuesedLetters.Add(c);
            // Using I to mark the index point on where to add the letter if it was guessed right
            var i = 0;
            bool GuessedRight = false;
            foreach (var x in NewGame.SortedWord)
            {
                if (x.ToLower() == c.ToLower())
                {
                    NewGame.DiscoveredSoFar[i] = c.ToString();
                    GuessedRight = true;
                }
                i++;
            }


            //Bool to switch off if there is still an unguessed character
            bool GameComplete = !NewGame.DiscoveredSoFar.Contains("-");
            //Return the letters guessed so far
            string GuessedSoFar = "";
            for (int ix = 0; ix < NewGame.GuesedLetters.Count; ix++)
            {
                if (ix != NewGame.GuesedLetters.Count - 1)
                {
                    GuessedSoFar += NewGame.GuesedLetters[ix] + ", ";
                }
                else
                {
                    GuessedSoFar += NewGame.GuesedLetters[ix];
                }
            }
            //Return the Letters that were correctly guessed in their correct position.
            string returning = "";
            for (int ir = 0; ir < NewGame.DiscoveredSoFar.Count; ir++)
            {
                returning += NewGame.DiscoveredSoFar[ir];
            }
            if (GuessedRight)
            {
                await ReplyAsync($"You Got It! So far you have {returning} and you have already tried {GuessedSoFar}");
            }
            else
            {
                NewGame.GuessRemain--;
                if (NewGame.GuessRemain <= 0)
                {
                    await ReplyAsync("Game Over, The word was " + NewGame.Word);
                    await PlayAgain();
                }
                else
                {
                    await ReplyAsync($"Sorry that was not a letter, Try Again! you have {NewGame.GuessRemain} Guesses Remaining, So far you have {returning} and have tried {GuessedSoFar}");
                }
            }
            if (GameComplete)
            {
                await ReplyAsync("Great Job! The word was " + NewGame.Word);
                NewGame = null;
                await PlayAgain();
            }
        }
        public static string GetNewWord()
        {
            //Im Lazy, Here is just a list of words
            var NewWord = new List<string>()
            {
                "the",
                "thats",
                "pizza",
                "program",
                "dickbutt",
                "reddit",
                "rocky",
                "this",
                "guy",
                "fucks",
                "testy",
                "jazz"
            };
            var Picker = new Random();
            //ForTesting, I wanted to know what number the random was giving
            var Pick = Picker.Next(0, NewWord.Count);
            return NewWord[Pick];
        }

        [Command("I guess")]
        public async Task GuessTheWord(string c)
        {
            if (c == NewGame.Word)
            {
                await ReplyAsync("$Well done, you've correctly guessed the word, with {NewGame.GuessRemain} Guesses remaining");
                await PlayAgain();
            }

        }

        [Command("Yes")]
        public async Task UserWantsToPlayAgain(string c)
        {
            if (NewGame.ReplayAsked == true)
            {
                await NewGameAsync();
            }
        }

        public async Task PlayAgain()
        {
            await ReplyAsync("Would you like to play again?");
            NewGame = null;
            NewGame.ReplayAsked = true;
        }

    }

    public class Game
    {
        public string Word { get; set; }
        public List<string> SortedWord { get; set; }
        public List<string> DiscoveredSoFar { get; set; }
        public List<string> GuesedLetters { get; set; }
        public int GuessRemain { get; set; }
        public Boolean ReplayAsked { get; set; }
    }
}
