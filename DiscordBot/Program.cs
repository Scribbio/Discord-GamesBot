using System;
using System.Threading.Tasks;
using System.Reflection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using DiscordBot;


/*
 * This file contains the important code that links to the Discord.NET API
 * 
 * 
 */ 

public class Program
{
    private CommandService _commands; //C* encourages an underscore when referring to the fields (or properties) of an object
    private DiscordSocketClient _client;
    private IServiceProvider _services;

    //Main uses an annoymous function (=>) to make a new program instance and start the StartAsync() function and from which
    //we need to get the result. This makes Main an asyncronous method in sync with StartAsync()
    private static void Main(string[] args) => new Program().StartAsync().GetAwaiter().GetResult();

    public async Task StartAsync()
    {
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            //Define some settings of DiscordSocketConfig
            LogLevel = LogSeverity.Verbose
        });
        _client.Log += Log;

        _commands = new CommandService();

        // Avoid hard coding your token. Use an external source instead in your code.
        string token = Config.bot.token;
        if (token == "" || token == null) return;

        _services = new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_commands)
            .BuildServiceProvider();

        await InstallCommandsAsync();

        //log bot into Discord server
        await _client.LoginAsync(TokenType.Bot, token);
        //start bot
        await _client.StartAsync();

        await Task.Delay(-1); //Wait until the operation ends
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.Message);
        return Task.CompletedTask;
    }

    public async Task InstallCommandsAsync()
    {
        // Hook the MessageReceived Event into our Command Handler
        _client.MessageReceived += HandleCommandAsync;
        // Discover all of the commands in this assembly and load them.
        await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
    }


    //This method happens once the bot receives a message
    private async Task HandleCommandAsync(SocketMessage messageParam)
    {
        // Don't process the command if it was a System Message
        var message = messageParam as SocketUserMessage;
        if (message == null) return; //precaution to ensure that nothing "broken" has been parsed
        // Create a number to track where the prefix ends and the command begins
        int argPos = 0;
        // Determine if the message is a command, based on if it starts with '!' or a mention prefix
        if (!(message.HasStringPrefix(Config.bot.cmdPrefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;
        // Create a Command Context
        var context = new SocketCommandContext(_client, message);
        // Execute the command. (result does not indicate a return value, 
        // rather an object stating if the command executed successfully)
        var result = await _commands.ExecuteAsync(context, argPos, _services);
        if (!result.IsSuccess)
            await context.Channel.SendMessageAsync(result.ErrorReason);
    }
}