using GIReporter.Services;
using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GIReporter.UpdateHandler;

public class UpdateHandlers
{
    private readonly ITelegramBotClient _botClient;
    private readonly UserService _userService;
    private readonly CommandInvoker _commandInvoker;

    public UpdateHandlers(ITelegramBotClient botClient, UserService userService, CommandInvoker commandExecutor)
    {
        _botClient = botClient;
        _userService = userService;
        _commandInvoker = commandExecutor;
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        var handler = update switch
        {
            { Message: { } message } => BotOnMessageReceived(update, cancellationToken),
            { MyChatMember: { } myChatMember } => BotOnChatMember(myChatMember, cancellationToken),
            _ => UnknownUpdateHandlerAsync()
        };

        await handler;
    }

    private async Task BotOnChatMember(ChatMemberUpdated myChatMember, CancellationToken cancellationToken)
    {
        if (myChatMember is null
            || myChatMember.NewChatMember.Status is ChatMemberStatus.Left
            || myChatMember.NewChatMember.Status is ChatMemberStatus.Kicked)
            return;

        var userIdFromAdd = myChatMember.From.Id;
        if (await _userService.IsAdminAsync(userIdFromAdd))
        {
            if (myChatMember.NewChatMember is ChatMemberAdministrator)
            {
                await _botClient.SendTextMessageAsync(
                       chatId: userIdFromAdd,
                       text: $"Меня добавили в `{myChatMember.Chat.Type}`\nНазвание: `{myChatMember.Chat.Title}`\nID: `{myChatMember.Chat.Id}`",
                       parseMode: ParseMode.Markdown,
                       cancellationToken: cancellationToken);
            }
        }
        else
        {
            if (myChatMember.Chat.Type is ChatType.Private)
            {
                return;
            }
            await _botClient.LeaveChatAsync(myChatMember.Chat.Id, cancellationToken);
        }
    }

    private async Task BotOnMessageReceived(Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;
        if (message?.Text is not { } messageText)
            return;

        if (message.Chat.Type is not ChatType.Private)
            return;

        if (message.From is null)
            return;

        if (!await _userService.IsAdminAsync(message.From.Id))
            return;

        await _botClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
        await _commandInvoker.CommandExexute(message);
    }

    private Task UnknownUpdateHandlerAsync() => Task.CompletedTask;
}
