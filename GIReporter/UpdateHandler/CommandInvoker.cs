using GIReporter.Commands.Interfaces;
using GIReporter.Models;
using GIReporter.Services;
using GIReporter.States;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GIReporter.UpdateHandler
{
    public class CommandInvoker
    {
        private readonly Dictionary<string, ICommand> _commands = new();
        private readonly ITelegramBotClient _botClient;
        private readonly UserService _userService;

        public CommandInvoker(
            ITelegramBotClient botClient,
            UserService userService,
            IEnumerable<ICommand> commands)
        {
            _botClient = botClient;
            _userService = userService;
            foreach (var command in commands)
                _commands.Add(command.CommandName, command);
        }

        public async Task CommandExexute(Message message)
        {
            ICommand? command;
            var userId = message.From.Id;
            var messageText = message.Text;
            var commandInterfaceType = typeof(ICommand);
            var userState = await _userService.GetUserStateAsync(userId);
            var userInProgressCommand = await _userService.GetInProcessCommand(userId);

            if (messageText == "/start")
            {
                await _commands["/start"].Execute(message);
                await SetBotCommands();
                return;
            }

            if (userInProgressCommand is not null)
                if (_commands.TryGetValue(userInProgressCommand, out command))
                {
                    await command.GetUpdate(message);
                    return;
                }

            if (userState is not State.Any)
            {
                var commands = _commands.Values
                    .Where(type => type.GetType()?
                        .GetCustomAttribute<UserStateAttribute>()?.State == userState);

                if (commands is null)
                {
                    await _botClient.SendTextMessageAsync(message.Chat.Id, "Команда не найдена!");
                    return;
                }

                foreach (var com in commands)
                    if (com.CommandName == messageText)
                    {
                        await com.Execute(message);
                        return;
                    }

                await _botClient.SendTextMessageAsync(message.Chat.Id, "Команда не найдена!");
                return;
            }

            if (_commands.TryGetValue(messageText, out command))
                await command.Execute(message);
            else
                await _botClient.SendTextMessageAsync(message.Chat.Id, "Команда не найдена!");
        }

        private async Task SetBotCommands()
        {
            var botCommands = _commands.Values
                    .Select(com => new BotCommand
                    {
                        Command = com.CommandName,
                        Description = com.Description
                    })
                    .OrderBy(com => com.Command.Length)
                    .ToList();

            await _botClient.SetMyCommandsAsync(botCommands);
        }
    }
}
