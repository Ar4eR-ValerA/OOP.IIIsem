using System;

namespace Shops.Ui.Tools
{
    public class Command
    {
        public Command(string title, Action action)
        {
            Action = action;
            Title = title;
        }

        public Command(string title)
        {
            Action = () => { };
            Title = title;
        }

        public string Title { get; }
        public Action Action { get; }

        public override string ToString()
        {
            return Title;
        }
    }
}