using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    internal interface IGame
    {
        Task CreateNewGame();

        Task ResetGame();

        Task Yes();

        Task No();

        Task GuessAWord(string guessedWord, bool overrideCheck = false);

        Task GuessALetter(char guessedLetter);

        Task Help();
    }
}