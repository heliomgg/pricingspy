using PricingSpy.Logic;
using PricingSpy.Logic.Models;
using PricingSpy.Notifier;
using System;
using System.IO;
using System.Text.Json;
using McMaster.Extensions.CommandLineUtils;

namespace PricingSpy.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandLineApplication = CreateCommandLineApplication();
            commandLineApplication.Execute(args);
        }

        private static CommandLineApplication CreateCommandLineApplication()
        {
            CommandLineApplication commandLineApplication = new CommandLineApplication(throwOnUnexpectedArg: false);

            CommandOption consoleOption = commandLineApplication.Option(
              "-console",
              "The result goes to the Console.",
              CommandOptionType.NoValue);

            CommandOption productsFileOption = commandLineApplication.Option(
                "-file <productsFileOption>",
                "Products file path.",
                CommandOptionType.SingleValue);

            CommandOption telegramOption = commandLineApplication.Option(
              "-telegram",
              "The result goes to the Telegram.",
              CommandOptionType.NoValue);

            CommandOption telegramChatIdOption = commandLineApplication.Option(
                "-chatid <telegramChatIdOption>",
                "ChatId in Telegram.",
                CommandOptionType.SingleValue);

            CommandOption telegramTokenOption = commandLineApplication.Option(
                "-token <telegramTokenOption>",
                "Token in Telegram.",
                CommandOptionType.SingleValue);

            commandLineApplication.HelpOption("-? | -h | --help");

            commandLineApplication.OnExecute(async () =>
            {
                if (!productsFileOption.HasValue())
                {
                    Console.WriteLine("Specify the file path with the Products to search!");
                    return 0;
                }

                if (!consoleOption.HasValue() && !telegramOption.HasValue())
                {
                    Console.WriteLine("Notifier type not found! Possible values: '-console' or '-telegram'.");
                    return 0;
                }

                var productsInfoRequest = JsonSerializer.Deserialize<ProductInfoRequest[]>(
                    File.ReadAllText(productsFileOption.Value()),
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });

                AppController appController = new AppController();
                var message = await appController.GetProductsInfoAsync(productsInfoRequest);

                if (!string.IsNullOrEmpty(message))
                {
                    INotificationManager notificationManager = null;
                    if (consoleOption.HasValue())
                    {
                        notificationManager = new ConsoleNotificationManager();
                    }

                    if (telegramOption.HasValue())
                    {
                        if (!telegramChatIdOption.HasValue() || !telegramTokenOption.HasValue())
                        {
                            Console.WriteLine($"Specify Telegram chatid and token values: '-chatid' and '-token'.");
                            return 0;
                        }

                        notificationManager = new TelegramNotificationManager(telegramChatIdOption.Value(), telegramTokenOption.Value());
                    }

                    if (notificationManager != null)
                        await notificationManager.SendMessage(message);
                }
                return 0;
            });
            
            return commandLineApplication;
        }
    }
}
