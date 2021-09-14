using System.Collections.Generic;
using Spectre.Console;

namespace Shops.Console_UI
{
    public class Asking
    {
        public string AskChoices(string message, IEnumerable<string> choices)
        {
            AnsiConsole.Write(message + "\n\n");
            return AnsiConsole.Prompt(new SelectionPrompt<string>()
                .AddChoices(choices));
        }

        public int AskChoices(string message, IEnumerable<int> choices)
        {
            AnsiConsole.Write(message + "\n\n");
            return AnsiConsole.Prompt(new SelectionPrompt<int>()
                .AddChoices(choices));
        }

        public int AskInt(string message)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<int>(message + "\n")
                    .Validate(value =>
                    {
                        return value switch
                        {
                            < 0 => ValidationResult.Error("[red]Value must be positive[/]"),
                            _ => ValidationResult.Success(),
                        };
                    }));
        }

        public string AskString(string message)
        {
            return AnsiConsole.Ask<string>(message + "\n");
        }
    }
}