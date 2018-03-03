//using Discord.Commands;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace DiscordBot.Modules
//{
//    public class Casino : ModuleBase<SocketCommandContext>
//    {
//        private static BlackJack BlackJackGame;
//        [Command("StartupBlackJack")]
//        public async Task BlackJackAsync()
//        {
//            await ReplyAsync("Starting a New game of Blackjack, ");
//        }
//        [Command("CountMeIn")]
//        public async Task AddPlayerAsync(SocketCommandContext message)
//        {
//            var Newplayer = new Player
//            {
//                Name = message.User.ToString(),
//                Chips = 1000
//            };
//            foreach (var player in BlackJackGame.players)
//            {
//                if (player.Name == Newplayer.Name)
//                {
//                    await ReplyAsync($"You are already in the game {player.Name} you have {player.Chips} Chips");
//                    if (player.Chips < 10)
//                    {
//                        player.Chips += 500;
//                        await ReplyAsync($"Fuck your broke... Here have some pity chips");
//                    }
//                }
//            }
//            await ReplyAsync(message.User + "Has been added to the game!");
//        }
//    }
//}
