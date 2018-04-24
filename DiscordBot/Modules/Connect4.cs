using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    //https://github.com/Kwoth/NadekoBot/blob/1.9/NadekoBot.Core/Modules/Games/Connect4Commands.cs

    public class Connect4 : ModuleBase<SocketCommandContext>, IGame
    {
        public string Question => throw new NotImplementedException();

        private new SocketCommandContext Context;
        readonly int maximumDepth;
        readonly Random random;
        readonly int difficultyLevel;
        char[,] GameState = new char[6, 7];


        private Dictionary<SocketUser, string> PlayerTokens = new Dictionary<SocketUser, string>();


        private List<string> Slots;


        public Connect4(SocketCommandContext context)
        {

            this.difficultyLevel = 0; //testing
            Context = context;

        }

        public async Task ChooseYourColor()
        {



        }

        public async Task CreateNewGame() //needs a difficulty level param
        {
            StringBuilder message = MakeBoard(GameState);

            await Context.Channel.SendMessageAsync(message.ToString());
            await Context.Channel.SendMessageAsync($"The Connect4 board has been set up. Use [!JoinConnect4] followed by your color to join the game.");

        }

        public async Task JoinTheGame(SocketUser user, string color)
        {
            if (PlayerTokens.ContainsKey(user))
            {
                await Context.Channel.SendMessageAsync($"{user.Mention} You're already in the game!");
                return;
            }

            PlayerTokens.Add(user, color.ToLower());

            await Context.Channel.SendMessageAsync($"{user.Mention} has joined the game using {color}! Type [!Drop] followed by the desired column number to drop a token.");
        }

        public async Task Drop(SocketUser user, int col)
        {
            // drop into col, return row or -1 if fail

            char token = ' ';

            string colour = PlayerTokens[user];


            if (colour == "red")
            {
                token = 'R';
            }
            else if (colour == "blue")
            {
                token = 'B';
            }

            for (int i = GameState.GetLength(0); i-- > 0;)
            {
                int row = i;

                if (GameState[row, col] != 'B' && 'R' != GameState[row, col])
                { 
                    GameState[row, col] = token;
                    break;
                }
            }

            await Context.Channel.SendMessageAsync($"{Context.User.Mention} has dropped {colour} into column {col}");
            StringBuilder message = MakeBoard(GameState);


            await Context.Channel.SendMessageAsync(message.ToString());

            if (HasWon(GameState))
            {
                await Context.Channel.SendMessageAsync("You win!");
            }

        }

        public static bool HasWon(char[,] GameState)
        {
            bool win = false;

            for (int i = 0; i < GameState.GetLength(0); i++)
            {
                for (int ix = 0; ix < GameState.GetLength(1); ix++)
                {
                    if (GameState[i, ix] != 0 && GameState[i, ix] == GameState[i, ix+1] 
                        && GameState[i, ix+1] == GameState[i, ix + 2] 
                        && GameState[i, ix+2] == GameState[i, ix + 3])
                    {
                        win = true;
                        break;
                    }
                }

            }

            return win;
        }

        public static StringBuilder MakeBoard(char[,] GameState)
        {
            int rows = GameState.GetLength(0);
            int columns = GameState.GetLength(1);

            StringBuilder sb = new StringBuilder();
            sb.Append("```");

            for (int i = 0; i < rows; i++)
            {
                sb.Append("|");

                for (int ix = 0; ix < columns; ix++)
                {
                    if (GameState[i, ix] != 'R' && GameState[i, ix] != 'B')
                    {
                        sb.Append('⚫');
                    }
                    else if (GameState[i, ix] == 'B')
                    {
                        sb.Append("🔵");
                    }
                    else if (GameState[i, ix] == 'R')
                    {
                        sb.Append("🔴");
                    }

                    sb.Append("|");
                }

                sb.AppendLine();
            }

            for (int i = 1; i < columns; i++)
            {
                sb.Append(" " + i + "  ");
            }

                sb.Append("```");

            return (sb);
        }








        //                    for (int i = Connect4Game.NumberOfRows; i > 0; i--)
        //            {
        //                for (int j = 0; j<Connect4Game.NumberOfColumns; j++)
        //                {
        //                    //Console.WriteLine(i + (j * Connect4Game.NumberOfRows) - 1);
        //                    var cur = game.GameState[i + (j * Connect4Game.NumberOfRows) - 1];

        //                    if (cur == Connect4Game.Field.Empty)
        //                        sb.Append("⚫"); //black circle
        //                    else if (cur == Connect4Game.Field.P1)
        //                        sb.Append("🔴"); //red circle
        //                    else
        //                        sb.Append("🔵"); //blue circle
        //                }
        //sb.AppendLine();
        //            }




        //internal sealed class AIEngine
        //{
        //    readonly int maximumDepth;
        //    readonly Random random;


        //    public AIEngine(DifficultyLevel difficultyLevel)
        //    {
        //        this.maximumDepth = (int)difficultyLevel;

        //        if (maximumDepth < (int)DifficultyLevel.Easy ||
        //            maximumDepth > (int)DifficultyLevel.Hard)
        //            throw new ArgumentOutOfRangeException("difficultyLevel");

        //        this.random = new Random(DateTime.Now.Millisecond);
        //    }

        //    public int GetBestMove(Board board, ActivePlayer player)
        //    {
        //        if (board == null)
        //            throw new ArgumentNullException("board");

        //        var node = new Node(board);
        //        var possibleMoves = getPossibleMoves(node);
        //        var scores = new double[possibleMoves.Count];
        //        Board updatedBoard;

        //        for (int i = 0; i < possibleMoves.Count; i++)
        //        {
        //            board.MakePlay(player, possibleMoves[i], out updatedBoard);
        //            var variant = new Node(updatedBoard);
        //            createTree(getOpponent(player), variant, 0);
        //            scores[i] = scoreNode(variant, player, 0);
        //        }

        //        double maximumScore = double.MinValue;
        //        var goodMoves = new List<int>();

        //        for (int i = 0; i < scores.Length; i++)
        //        {
        //            if (scores[i] > maximumScore)
        //            {
        //                goodMoves.Clear();
        //                goodMoves.Add(i);
        //                maximumScore = scores[i];
        //            }
        //            else if (scores[i] == maximumScore)
        //            {
        //                goodMoves.Add(i);
        //            }
        //        }

        //        return possibleMoves[goodMoves[random.Next(0, goodMoves.Count)]];
        //    }

        //}




        //public Task CreateNewGame()
        //{
        //    throw new NotImplementedException();
        //}

        public Task Help()
        {
            throw new NotImplementedException();
        }

        public Task No()
        {
            throw new NotImplementedException();
        }

        public Task ResetGame()
        {
            throw new NotImplementedException();
        }

        public Task Yes()
        {
            throw new NotImplementedException();
        }

        //Slots = new List<string>();

        //await Context.Channel.SendMessageAsync("Playing Connect4. Difficulty level: ");

        //int rowWidth = 3; //will be 3 * difficulty level 

        //var play = new StringBuilder();
        //play.AppendLine("```");
        //play.AppendLine("⚫🔵⚫⚫⚫");
        //play.AppendLine("⚫⚫🔵⚫⚫");
        //play.AppendLine("⚫⚫⚫🔵⚫");
        //play.AppendLine("⚫⚫⚫⚫🔵");
        //play.AppendLine("```");

    }
}
