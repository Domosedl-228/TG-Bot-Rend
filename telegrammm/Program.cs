using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
namespace teleggg
{
    internal class Program
    {
        private static ITelegramBotClient _botClient;
        private static ReceiverOptions _receiverOptions;

        public static Task Main() => new Program().MainAsync();
        public async Task MainAsync()
        {
            _botClient = new TelegramBotClient("7074132153:AAHpzhhetBPKVJJybNoca1znltWgM8BaDyI");
            _receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[]
                {
                UpdateType.Message,
            },
                ThrowPendingUpdates = true,
            };
            using var cts = new CancellationTokenSource();
            _botClient.StartReceiving(UpdateHandler, ErrorHadler, _receiverOptions, cts.Token);
            var me = await _botClient.GetMeAsync();
            Console.WriteLine($"{me.FirstName} запущен!!!");
            await Task.Delay(-1);
        }
        private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        {
                            var message = update.Message;
                            var user = message.From;
                            Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение:{message.Text}");
                            var chat = message.Chat;
                            switch (message.Type)
                            {
                                case MessageType.Text:
                                    {
                                        if (message.Text == "/start")
                                        {

                                            await botClient.SendTextMessageAsync
                                                (chat.Id,
                                                "Добро пожаловать в данный проект,выберите действие:\n" +
                                                "/inline\n" +
                                                "/reply\n" +
                                                "/photo\n"
                                                );
                                            return;
                                        }
                                    }

                                    if (message.Text == "/inline")
                                    {
                                        var inlineKeyboard = new InlineKeyboardMarkup(
                                            new List<InlineKeyboardButton[]>()
                                            {
                                            new InlineKeyboardButton[]
                                            {
                                                InlineKeyboardButton.WithUrl("Это кнопка с сайтом", "https://github.com/Domosedl-228/TG-Bot-Rend"),
                                                InlineKeyboardButton.WithCallbackData("НЕ ЖМИМ ЭТУ КНОПКУ", "ХУУУУЙЙЙЙЙЙ"),

                                            },
                                            new InlineKeyboardButton[]
                                            {
                                                InlineKeyboardButton.WithPayment("Внесите оплату админу на мать!!!"),
                                                InlineKeyboardButton.WithCallbackData("Это кнопка","А ты джопка!"),
                                            },
                                            });
                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            "Это Inline клавиатура!",
                                            replyMarkup: inlineKeyboard);
                                        return;

                                    }
                                    if (message.Text == "/reply")
                                    {
                                        var replyKeyboard = new ReplyKeyboardMarkup(
                                            new List<KeyboardButton[]>()
                                            {

                                    new KeyboardButton[]
                                    {
                                        new KeyboardButton("Позвони мне!"),
                                    },
                                    new KeyboardButton[]
                                    {
                                        new KeyboardButton("Напиши моему соседу!"),
                                    },


                                            })
                                        {
                                            ResizeKeyboard = true,
                                        };
                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            "Это Reply клавиатура!",
                                            replyMarkup: replyKeyboard);
                                        return;
                                    }
                                    return;
                            }
                            object value = default;
                            {
                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    "Используй только текст!");
                                return;

                            }
                        }



                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private static Task ErrorHadler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };
            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }


    }
}

