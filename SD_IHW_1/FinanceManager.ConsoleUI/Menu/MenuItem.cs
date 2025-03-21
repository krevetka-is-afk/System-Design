using System;
using System.Threading.Tasks;

namespace FinanceManager.ConsoleUI.Menu
{
    public class MenuItem
    {
        public string Name { get; }
        public string Description { get; }
        public Func<Task> Action { get; }

        public MenuItem(string name, string description, Func<Task> action)
        {
            Name = name;
            Description = description;
            Action = action;
        }
    }
} 