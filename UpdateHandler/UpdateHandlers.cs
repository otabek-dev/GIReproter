using GIReporter.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GIReporter.UpdateHandler;

public class UpdateHandlers
{
    private readonly ITelegramBotClient _botClient;
    private readonly UserService _userService;
    private readonly CommandExecutor _commandExecutor;

    public UpdateHandlers(
        ITelegramBotClient botClient,
        UserService userService,
        CommandExecutor commandExecutor)
    {
        _botClient = botClient;
        _userService = userService;
        _commandExecutor = commandExecutor;
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        var handler = update switch
        {
            { Message: { } message } => BotOnMessageReceived(update, cancellationToken),
            { MyChatMember: { } myChatMember } => BotOnChatMember(myChatMember, cancellationToken),
            _ => UnknownUpdateHandlerAsync(update, cancellationToken)
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
        if (_userService.IsAdmin(userIdFromAdd))
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

        if (!_userService.IsAdmin(message.From.Id))
            return;

        await _commandExecutor.GetUpdate(update);
    }

    private Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
