using Discord.Commands;
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


        private List<string> Slots;


        public Connect4(SocketCommandContext context)
        {

            this.difficultyLevel = 0; //testing
            Context = context;

        }


 

        public async Task CreateNewGame() //needs a difficulty level param
        {


            StringBuilder message = MakeBoard(GameState);


            await Context.Channel.SendMessageAsync(message.ToString());
        }


        public async Task Drop(int col, string colour)
        {
            // drop into col, return row or -1 if fail

            char token = ' ';

            if (colour == "Red")
            {
                token = 'R';
            }
            else if (colour == "Green")
            {
                token = 'G';
            }


            for (int row = 0; row < GameState.GetLength(0); row++)
            {
                
                if (GameState[row, col] != 'G' && 'R' != GameState[row, col])
                { 
                    GameState[row, col] = token;
                    break;
                }
            }

            StringBuilder message = MakeBoard(GameState);


            await Context.Channel.SendMessageAsync(message.ToString());

        }
    


        public static StringBuilder MakeBoard(char[,] GameState)
        {
            int rows = GameState.GetLength(0);
            int columns = GameState.GetLength(1);

            StringBuilder sb = new StringBuilder();
            sb.Append("```");

            for (int i = 0; i < rows; i++)
            {
                for (int ix = 0; ix < columns; ix++)
                {

                    if (GameState[i, ix] != 'R' && GameState[i, ix] != 'G')
                    {
                        sb.Append('⚫');
                    }
                    else if (GameState[i, ix] == 'G')
                    {
                        sb.Append("🔵");
                    }

                }

                sb.AppendLine();
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
