using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class Blackjack : ModuleBase<SocketCommandContext>, IGame
    {
        private new SocketCommandContext Context;
        private Deck CardDeck = new Deck();
        private Dictionary<SocketUser, List<Deck.Card>> DealtHands = new Dictionary<SocketUser, List<Deck.Card>>();
        private bool GameStarted = false;
        private bool ResetGameAsked = false;
        private int CurrentPlayer = 0;

        public string Question
        {
            get; protected set;
        }

        public Blackjack(SocketCommandContext context)
        {
            Context = context;
        }

        public async Task CreateNewGame()
        {
            DealtHands.Clear();
            CardDeck.Shuffle();
            await Context.Channel.SendMessageAsync($"I've started a new game of blackjack and shuffled the deck! Use [!Join] to join the game.");
        }

        public async Task JoinTheGame(SocketUser user)
        {
            if (DealtHands.ContainsKey(user))
            {
                await Context.Channel.SendMessageAsync($"{user.Mention} You're already in the game! Use [!Start] to start the game.");
                return;
            }

            DealtHands.Add(user, new List<Deck.Card>());

            await Context.Channel.SendMessageAsync($"{user.Mention} has joined the game! Type [!Start] to start the game.");
        }

        public async Task StartTheGame(SocketSelfUser botUser)
        {
            if (DealtHands.Count == 0)
            {
                await Context.Channel.SendMessageAsync($"No one has joined the game, please use [!Join] to join the game.");
                return;
            }
            else if (GameStarted)
            {
                await Context.Channel.SendMessageAsync($"The game has already started. Use [!ResetGame] to restart the game or [!Quit] to play a different game.");
                return;
            }

            DealtHands.Add(botUser, new List<Deck.Card>());

            StringBuilder output = CardsToString();
            output.AppendLine(NextPlayerHitOrStay());
            await Context.Channel.SendMessageAsync(output.ToString());
            GameStarted = true;
        }

        public async Task Yes()
        {
            Question = String.Empty;

            if (ResetGameAsked)
            {
                await CreateNewGame();
                return;
            }

            await Context.Channel.SendMessageAsync("Huh? I didn't ask you anything.");
        }

        public async Task No()
        {
            await Context.Channel.SendMessageAsync("Huh?");
        }

        public async Task ResetGame()
        {
            Question = "Are you sure you want to reset the game? [!Yes/!No]";
            await Context.Channel.SendMessageAsync(Question);
            ResetGameAsked = true;
        }

        public async Task Help()
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("```");
            output.AppendLine("!Join - Allows you to join the game before it starts.");
            output.AppendLine("!Start - Allows you to join the game before it starts.");
            output.AppendLine("!HitMe - Responds to the bot to add a card to your hand.");
            output.AppendLine("!Stay - Completes your turn.");
            output.AppendLine("!ResetGame - Restarts the game.");
            output.AppendLine("!Quit - Quits the game.");
            output.AppendLine("!Help - Shows this output.");
            output.AppendLine("   !Yes - Response in the affirmative to a question.");
            output.AppendLine("   !No - Responds in the negative to a question.");
            output.AppendLine("```");

            await Context.Channel.SendMessageAsync(output.ToString());
        }

        public async Task Hit(SocketUser user)
        {
            if (GameStarted)
            {
                await Context.Channel.SendMessageAsync($"First start the game using [!Start]");
            }

            KeyValuePair<SocketUser, List<Deck.Card>> currentHand = DealtHands.ElementAt(CurrentPlayer);

            if (currentHand.Key.Username != user.Username)
            {
                await Context.Channel.SendMessageAsync($"{user.Mention} you're not allowed to make a decision for another player!");
                return;
            }

            currentHand.Value.Add(CardDeck.DrawACard());
            int currentValue = currentHand.Value.Sum(x => x.Value);
            if (currentValue > 21)
            {
                await Context.Channel.SendMessageAsync($"{currentHand.Key.Mention} your new hand is {String.Join(", ", currentHand.Value)} ({currentValue}) - Unfortunately you've busted.");
                await MoveToNextPlayer();
            }
            else
            {
                Question = $"{DealtHands.ElementAt(CurrentPlayer).Key.Mention} your new hand is {String.Join(", ", currentHand.Value)} ({currentValue}) - Would you like to [!Hit] or [!Stay]?";
                await Context.Channel.SendMessageAsync(Question);
            }
        }

        private async Task MoveToNextPlayer()
        {
            CurrentPlayer++;
            if (CurrentPlayer == DealtHands.Count - 1)
            {
                await Context.Channel.SendMessageAsync("This is where I would do stuff...");
                KeyValuePair<SocketUser, List<Deck.Card>> currentHand = DealtHands.ElementAt(CurrentPlayer);
                int currentValue = currentHand.Value.Sum(x => x.Value);
                string currentValueString = currentValue > 21 ? "BUSTED" : currentValue.ToString();
            }
            else
            {
                await Context.Channel.SendMessageAsync(NextPlayerHitOrStay());
            }
        }

        private string NextPlayerHitOrStay()
        {
            Question = $"{DealtHands.ElementAt(CurrentPlayer).Key.Mention} - Would you like to [!Hit] or [!Stay]?";
            return Question;
        }

        private StringBuilder CardsToString()
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("```");
            output.AppendLine("Current hands:");

            foreach (KeyValuePair<SocketUser, List<Deck.Card>> currentHand in DealtHands)
            {
                currentHand.Value.Add(CardDeck.DrawACard());
                currentHand.Value.Add(CardDeck.DrawACard());

                int currentValue = currentHand.Value.Sum(x => x.Value);
                string currentValueString = currentValue > 21 ? "BUSTED" : currentValue.ToString();

                output.AppendLine($"{currentHand.Key.Username} - {String.Join(", ", currentHand.Value)} ({currentValueString})");
            }

            output.AppendLine("```");
            return output;
        }
    }
}

