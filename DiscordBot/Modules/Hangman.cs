using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class Hangman : ModuleBase<SocketCommandContext>
    {
        private SocketCommandContext m_Context = null;
        private new SocketCommandContext Context
        {
            get
            {
                return m_Context;
            }
        }

        private bool ReplayAsked = false;
        private bool WordAccident = false;
        private bool QuestionAsked = false;

        private int GuessesRemainging;

        private string Word;
        private string DiscoveredSoFar;
        private string WordGuessed = String.Empty;

        private List<char> GuessedLetters;

        private string m_Question;
        private string Question
        {
            get
            {
                return m_Question;
            }
            set
            {
                QuestionAsked = true;
                m_Question = value;
            }
        }

        //The list of words to select from.
        private static List<string> WordDictionary = new List<string>()
        {
            "able",
            "about",
            "account",
            "acid",
            "across",
            "act",
            "addition",
            "adjustment",
            "advertisement",
            "after",
            "again",
            "against",
            "agreement",
            "air",
            "all",
            "almost",
            "among",
            "amount",
            "amusement",
            "and",
            "android",
            "angle",
            "angry",
            "animal",
            "answer",
            "ant",
            "any",
            "apparatus",
            "apple",
            "approval",
            "arch",
            "argument",
            "arm",
            "army",
            "art",
            "as",
            "at",
            "attack",
            "attempt",
            "attention",
            "attraction",
            "authority",
            "automatic",
            "awake",
            "baby",
            "back",
            "bad",
            "bag",
            "balance",
            "ball",
            "band",
            "base",
            "basin",
            "basket",
            "bath",
            "be",
            "beautiful",
            "because",
            "bed",
            "bee",
            "before",
            "behaviour",
            "belief",
            "bell",
            "bent",
            "berry",
            "between",
            "bird",
            "birth",
            "bit",
            "bite",
            "bitter",
            "black",
            "blade",
            "blood",
            "blow",
            "blue",
            "board",
            "boat",
            "body",
            "boiling",
            "bone",
            "book",
            "boot",
            "bottle",
            "box",
            "boy",
            "brain",
            "brake",
            "branch",
            "brass",
            "bread",
            "breath",
            "brick",
            "bridge",
            "bright",
            "broken",
            "brother",
            "brown",
            "brush",
            "bucket",
            "building",
            "bulb",
            "burn",
            "burst",
            "business",
            "but",
            "butter",
            "button",
            "by",
            "cake",
            "camera",
            "canvas",
            "card",
            "care",
            "carriage",
            "cart",
            "cat",
            "cause",
            "certain",
            "chain",
            "chalk",
            "chance",
            "change",
            "cheap",
            "cheese",
            "chemical",
            "chest",
            "chief",
            "chin",
            "church",
            "circle",
            "clean",
            "clear",
            "clock",
            "cloth",
            "cloud",
            "coal",
            "coat",
            "cold",
            "collar",
            "colour",
            "comb",
            "come",
            "comfort",
            "committee",
            "common",
            "company",
            "comparison",
            "competition",
            "complete",
            "complex",
            "condition",
            "connection",
            "conscious",
            "control",
            "cook",
            "copper",
            "copy",
            "cord",
            "cork",
            "cotton",
            "cough",
            "country",
            "cover",
            "cow",
            "crack",
            "credit",
            "crime",
            "cruel",
            "crush",
            "cry",
            "cup",
            "cup",
            "current",
            "curtain",
            "curve",
            "cushion",
            "damage",
            "danger",
            "dark",
            "daughter",
            "day",
            "dead",
            "dear",
            "death",
            "debt",
            "decision",
            "deep",
            "degree",
            "delicate",
            "dependent",
            "design",
            "desire",
            "destruction",
            "detail",
            "development",
            "different",
            "digestion",
            "direction",
            "dirty",
            "discovery",
            "discussion",
            "disease",
            "disgust",
            "distance",
            "distribution",
            "division",
            "do",
            "dog",
            "door",
            "doubt",
            "down",
            "drain",
            "drawer",
            "dress",
            "drink",
            "driving",
            "drop",
            "dry",
            "dust",
            "ear",
            "early",
            "earth",
            "east",
            "edge",
            "education",
            "effect",
            "egg",
            "elastic",
            "electric",
            "end",
            "engine",
            "enough",
            "equal",
            "error",
            "even",
            "event",
            "ever",
            "every",
            "example",
            "exchange",
            "existence",
            "expansion",
            "experience",
            "expert",
            "eye",
            "face",
            "fact",
            "fall",
            "false",
            "family",
            "far",
            "farm",
            "fat",
            "father",
            "fear",
            "feather",
            "feeble",
            "feeling",
            "female",
            "fertile",
            "fiction",
            "field",
            "fight",
            "finger",
            "fire",
            "first",
            "fish",
            "fixed",
            "flag",
            "flame",
            "flat",
            "flight",
            "floor",
            "flower",
            "fly",
            "fold",
            "food",
            "foolish",
            "foot",
            "for",
            "force",
            "fork",
            "form",
            "forward",
            "fowl",
            "frame",
            "free",
            "frequent",
            "friend",
            "from",
            "front",
            "fruit",
            "full",
            "future",
            "garden",
            "general",
            "get",
            "girl",
            "give",
            "glass",
            "glove",
            "go",
            "goat",
            "gold",
            "good",
            "government",
            "grain",
            "grass",
            "great",
            "green",
            "grey",
            "grip",
            "group",
            "growth",
            "guide",
            "gun",
            "hair",
            "hammer",
            "hand",
            "hanging",
            "happy",
            "harbour",
            "hard",
            "harmony",
            "hat",
            "hate",
            "have",
            "he",
            "head",
            "healthy",
            "hear",
            "hearing",
            "heart",
            "heat",
            "help",
            "high",
            "history",
            "hole",
            "hollow",
            "hook",
            "hope",
            "horn",
            "horse",
            "hospital",
            "hour",
            "house",
            "how",
            "humour",
            "I",
            "ice",
            "idea",
            "if",
            "ill",
            "important",
            "impulse",
            "in",
            "increase",
            "industry",
            "ink",
            "insect",
            "instrument",
            "insurance",
            "interest",
            "invention",
            "iron",
            "island",
            "jelly",
            "jewel",
            "join",
            "journey",
            "judge",
            "jump",
            "keep",
            "kettle",
            "key",
            "kick",
            "kind",
            "kiss",
            "knee",
            "knife",
            "knot",
            "knowledge",
            "land",
            "language",
            "last",
            "late",
            "laugh",
            "law",
            "lead",
            "leaf",
            "learning",
            "leather",
            "left",
            "leg",
            "let",
            "letter",
            "level",
            "library",
            "lift",
            "light",
            "like",
            "limit",
            "line",
            "linen",
            "lip",
            "liquid",
            "list",
            "little",
            "living",
            "lock",
            "long",
            "look",
            "loose",
            "loss",
            "loud",
            "love",
            "low",
            "machine",
            "make",
            "male",
            "man",
            "manager",
            "map",
            "mark",
            "market",
            "married",
            "mass",
            "match",
            "material",
            "may",
            "meal",
            "measure",
            "meat",
            "medical",
            "meeting",
            "memory",
            "metal",
            "middle",
            "military",
            "milk",
            "mind",
            "mine",
            "minute",
            "mist",
            "mixed",
            "money",
            "monkey",
            "month",
            "moon",
            "morning",
            "mother",
            "motion",
            "mountain",
            "mouth",
            "move",
            "much",
            "muscle",
            "music",
            "nail",
            "name",
            "narrow",
            "nation",
            "natural",
            "near",
            "necessary",
            "neck",
            "need",
            "needle",
            "nerve",
            "net",
            "new",
            "news",
            "night",
            "no",
            "noise",
            "normal",
            "north",
            "nose",
            "not",
            "note",
            "now",
            "number",
            "nut",
            "observation",
            "of",
            "off",
            "offer",
            "office",
            "oil",
            "old",
            "on",
            "only",
            "open",
            "operation",
            "opinion",
            "opposite",
            "or",
            "orange",
            "order",
            "organization",
            "ornament",
            "other",
            "out",
            "oven",
            "over",
            "owner",
            "page",
            "pain",
            "paint",
            "paper",
            "parallel",
            "parcel",
            "part",
            "past",
            "paste",
            "payment",
            "peace",
            "pen",
            "pencil",
            "person",
            "physical",
            "picture",
            "pig",
            "pin",
            "pipe",
            "place",
            "plane",
            "plant",
            "plate",
            "play",
            "please",
            "pleasure",
            "plough",
            "pocket",
            "point",
            "poison",
            "polish",
            "political",
            "poor",
            "porter",
            "position",
            "possible",
            "pot",
            "potato",
            "powder",
            "power",
            "present",
            "price",
            "print",
            "prison",
            "private",
            "probable",
            "process",
            "produce",
            "profit",
            "property",
            "prose",
            "protest",
            "public",
            "pull",
            "pump",
            "punishment",
            "purpose",
            "push",
            "put",
            "quality",
            "question",
            "quick",
            "quiet",
            "quite",
            "rail",
            "rain",
            "range",
            "rat",
            "rate",
            "ray",
            "reaction",
            "reading",
            "ready",
            "reason",
            "receipt",
            "record",
            "red",
            "regret",
            "regular",
            "relation",
            "religion",
            "representative",
            "request",
            "respect",
            "responsible",
            "rest",
            "reward",
            "rhythm",
            "rice",
            "right",
            "ring",
            "river",
            "road",
            "rod",
            "roll",
            "roof",
            "room",
            "root",
            "rough",
            "round",
            "rub",
            "rule",
            "run",
            "sad",
            "safe",
            "sail",
            "salt",
            "same",
            "sand",
            "say",
            "scale",
            "school",
            "science",
            "scissors",
            "screw",
            "sea",
            "seat",
            "second",
            "secret",
            "secretary",
            "see",
            "seed",
            "seem",
            "selection",
            "self",
            "send",
            "sense",
            "separate",
            "serious",
            "servant",
            "sex",
            "shade",
            "shake",
            "shame",
            "sharp",
            "sheep",
            "shelf",
            "ship",
            "shirt",
            "shock",
            "shoe",
            "short",
            "shut",
            "side",
            "sign",
            "silk",
            "silver",
            "simple",
            "sister",
            "size",
            "skin",
            "skirt",
            "sky",
            "sleep",
            "slip",
            "slope",
            "slow",
            "small",
            "smash",
            "smell",
            "smile",
            "smoke",
            "smooth",
            "snake",
            "sneeze",
            "snow",
            "so",
            "soap",
            "society",
            "sock",
            "soft",
            "solid",
            "some",
            "son",
            "song",
            "sort",
            "sound",
            "soup",
            "south",
            "space",
            "spade",
            "special",
            "sponge",
            "spoon",
            "spring",
            "square",
            "stage",
            "stamp",
            "star",
            "start",
            "statement",
            "station",
            "steam",
            "steel",
            "stem",
            "step",
            "stick",
            "sticky",
            "stiff",
            "still",
            "stitch",
            "stocking",
            "stomach",
            "stone",
            "stop",
            "store",
            "story",
            "straight",
            "strange",
            "street",
            "stretch",
            "strong",
            "structure",
            "substance",
            "such",
            "sudden",
            "sugar",
            "suggestion",
            "summer",
            "sun",
            "support",
            "surprise",
            "sweet",
            "swim",
            "system",
            "table",
            "tail",
            "take",
            "talk",
            "tall",
            "taste",
            "tax",
            "teaching",
            "tendency",
            "test",
            "than",
            "that",
            "the",
            "then",
            "theory",
            "there",
            "thick",
            "thin",
            "thing",
            "this",
            "thought",
            "thread",
            "throat",
            "through",
            "through",
            "thumb",
            "thunder",
            "ticket",
            "tight",
            "till",
            "time",
            "tin",
            "tired",
            "to",
            "toe",
            "together",
            "tomorrow",
            "tongue",
            "tooth",
            "top",
            "touch",
            "town",
            "trade",
            "train",
            "transport",
            "tray",
            "tree",
            "trick",
            "trouble",
            "trousers",
            "true",
            "turn",
            "twist",
            "umbrella",
            "under",
            "unit",
            "up",
            "use",
            "value",
            "verse",
            "very",
            "vessel",
            "view",
            "violent",
            "voice",
            "waiting",
            "walk",
            "wall",
            "war",
            "warm",
            "wash",
            "waste",
            "watch",
            "water",
            "wave",
            "wax",
            "way",
            "weather",
            "week",
            "weight",
            "well",
            "west",
            "wet",
            "wheel",
            "when",
            "where",
            "while",
            "whip",
            "whistle",
            "white",
            "who",
            "why",
            "wide",
            "will",
            "wind",
            "window",
            "wine",
            "wing",
            "winter",
            "wire",
            "wise",
            "with",
            "woman",
            "wood",
            "wool",
            "word",
            "work",
            "worm",
            "wound",
            "writing",
            "wrong",
            "year",
            "yellow",
            "yes",
            "yesterday",
            "you",
            "young"
        };

        private string HangmanOutput
        {
            get
            {
                switch (GuessesRemainging)
                {
                    case 8:
                        return "```\n/ ---|\n|\n|\n|\n|\n```";
                    case 7:
                        return "```\n/ ---|\n|    o\n|\n|\n|\n```";
                    case 6:
                        return "```\n/ ---|\n|    o\n|    |\n|\n|\n```";
                    case 5:
                        return "```\n/ ---|\n|    o\n|   /|\n|\n|\n```";
                    case 4:
                        return "```\n/ ---|\n|    o\n|   /|\\\n|\n|\n```";
                    case 3:
                        return "```\n/ ---|\n|    o\n|   /|\\\n|\n|\n|\n```";
                    case 2:
                        return "```\n/ ---|\n|    o\n|   /|\\\n|    |\n|\n|\n```";
                    case 1:
                        return "```\n/ ---|\n|    o\n|   /|\\\n|    |\n|   /\n|\n```";
                    case 0:
                        return "```\n/ ---|\n|    o\n|   /|\\\n|    |\n|   / \\\n|\n```";
                }

                return String.Empty;
            }
        }

        public Hangman(SocketCommandContext context)
        {
            m_Context = context;
        }

        public async Task CreateNewGame()
        {
            ReplayAsked = false;
            WordAccident = false;
            QuestionAsked = false;

            GuessesRemainging = 9;

            Word = GetNewWord();
            DiscoveredSoFar = new string('-', Word.Length);
            WordGuessed = String.Empty;

            GuessedLetters = new List<char>();

            await Context.Channel.SendMessageAsync($"I Got a new word for you, its {Word.Length} Characters Long");
        }

        private void GameEnd()
        {
            Word = String.Empty;
            DiscoveredSoFar = String.Empty;
            GuessedLetters = new List<char>();
            GuessesRemainging = 0;
        }

        public static string GetNewWord()
        {
            Random Picker = new Random();
            int Pick = Picker.Next(0, WordDictionary.Count);
            return WordDictionary[Pick];
        }

        public async Task ResetGame()
        {
            Question = "Are you sure you want to reset the game? [!Yes/!No]";
            await Context.Channel.SendMessageAsync(Question);
            ReplayAsked = true;
        }

        public async Task Yes()
        {
            if (ReplayAsked)
            {
                await CreateNewGame();
                return;
            }

            if (WordAccident)
            {
                QuestionAsked = false;
                WordAccident = false;
                await GuessAWord(WordGuessed, true);
                return;
            }

            await Context.Channel.SendMessageAsync("Huh?");
        }

        public async Task No()
        {
            if (QuestionAsked)
            {
                ReplayAsked = false;
                QuestionAsked = false;
                WordAccident = false;
                await Context.Channel.SendMessageAsync("Okay!");
                return;
            }

            await Context.Channel.SendMessageAsync("Huh?");
        }

        public async Task GuessAWord(string guessedWord, bool overrideCheck = false)
        {
            if (GuessesRemainging <= 0)
            {
                await Context.Channel.SendMessageAsync("You do not have any guesses remainging, please [!ResetGame] or [!Quit] and start a different game.");
                return;
            }

            if (!overrideCheck)
            {
                if (QuestionAsked)
                {
                    await Context.Channel.SendMessageAsync("Please respond to the question before continuing...");
                    await Context.Channel.SendMessageAsync(Question);
                    return;
                }

                if (guessedWord.Length <= 1)
                {
                    Question = "Did you mean to guess a word? [!Yes/!No]";
                    await Context.Channel.SendMessageAsync(Question);
                    WordGuessed = guessedWord;
                    WordAccident = true;
                    return;
                }
            }

            if (guessedWord.ToLower() == Word)
            {
                await Context.Channel.SendMessageAsync($"Well done, you've correctly guessed the word, with {GuessesRemainging} guesses remaining.");
                GameEnd();
                await PlayAgain();
            }
            else
            {
                GuessesRemainging--;
                await Context.Channel.SendMessageAsync(HangmanOutput);
                await Context.Channel.SendMessageAsync($"No! You have {GuessesRemainging} guesses left.");
            }

            WordGuessed = String.Empty;
        }

        public async Task GuessALetter(char guessedLetter)
        {
            if (GuessesRemainging <= 0)
            {
                await Context.Channel.SendMessageAsync("You do not have any guesses remainging, please [!ResetGame] or [!Quit] and start a different game.");
                return;
            }

            if (QuestionAsked)
            {
                await Context.Channel.SendMessageAsync("Please respond to the question before continuing...");
                await Context.Channel.SendMessageAsync(Question);
                return;
            }

            if (GuessedLetters.Contains(guessedLetter))
            {
                await Context.Channel.SendMessageAsync($"The letter {guessedLetter} has already been guessed! Try again!");
                return;
            }

            GuessedLetters.Add(guessedLetter);
            GuessedLetters.Sort();
            DiscoveredSoFar = String.Empty;

            // Using I to mark the index point on where to add the letter if it was guessed right
            foreach (char CurrentWordCharacter in Word.ToCharArray())
            {
                if (GuessedLetters.Contains(CurrentWordCharacter))
                {
                    DiscoveredSoFar += CurrentWordCharacter;
                }
                else
                {
                    DiscoveredSoFar += "-";
                }
            }

            //Bool to switch off if there is still an unguessed character
            bool GameComplete = !DiscoveredSoFar.Contains("-");
            if (GameComplete)
            {
                await Context.Channel.SendMessageAsync($"Great Job! The word was {Word}");
                GameEnd();

                await PlayAgain();
            }

            //Return the letters guessed so far
            string GuessedSoFar = String.Join(", ", GuessedLetters);
            string SoFar = $"So far you have {DiscoveredSoFar} and you have already tried {GuessedSoFar}";

            //Return the Letters that were correctly guessed in their correct position.
            bool GuessedRight = Word.Contains(guessedLetter);
            if (GuessedRight)
            {
                await Context.Channel.SendMessageAsync($"You Got It! {SoFar}");
            }
            else
            {
                GuessesRemainging--;
                await Context.Channel.SendMessageAsync(HangmanOutput);

                if (GuessesRemainging <= 0)
                {
                    await Context.Channel.SendMessageAsync($"Game Over, The word was {Word}");
                    await PlayAgain();
                }
                else
                {
                    await Context.Channel.SendMessageAsync($"Sorry that was not a letter, try again! You have {GuessesRemainging} guesses remaining. " + SoFar);
                }
            }
        }

        public async Task PlayAgain()
        {
            Question = "Would you like to play again? [!Yes/!No]";
            await m_Context.Channel.SendMessageAsync(Question);
            ReplayAsked = true;
        }

        public async Task Help()
        {
            await Context.Channel.SendMessageAsync("```" + Environment.NewLine +
                                                   "!Guess X - Guesses a letter where X is the letter." + Environment.NewLine +
                                                   "!WordIs WORD - Guess the word where WORD is the word." + Environment.NewLine +
                                                   "!ResetGame - Restarts the game." + Environment.NewLine +
                                                   "!Quit - Quits the game." + Environment.NewLine +
                                                   "!Help - Shows this output." + Environment.NewLine +
                                                   "   !Yes - Response in the affirmative to a question." + Environment.NewLine +
                                                   "   !No - Responds in the negative to a question."+
                                                   "```");
        }
    }
}

