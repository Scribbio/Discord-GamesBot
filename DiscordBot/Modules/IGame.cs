using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    internal interface IGame
    {
        string Question
        {
            get;
        }

        Task CreateNewGame();

        Task ResetGame();

        Task Yes();

        Task No();

        Task Help();
    }
}