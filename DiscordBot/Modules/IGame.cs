using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    internal interface IGame
    {
        bool QuestionAsked
        {
            get;
        }

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