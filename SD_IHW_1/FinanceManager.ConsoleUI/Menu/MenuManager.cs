using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceManager.ConsoleUI.Menu
{
    public class MenuManager
    {
        private readonly Dictionary<string, MenuItem> _menuItems = new();
        private bool _isRunning = true;

        public void AddMenuItem(string key, MenuItem item)
        {
            _menuItems[key] = item;
        }

        public async Task RunAsync()
        {
            while (_isRunning)
            {
                DisplayMenu();
                var choice = Console.ReadLine()?.ToUpper();

                if (string.IsNullOrWhiteSpace(choice))
                    continue;

                if (choice == "Q")
                {
                    _isRunning = false;
                    continue;
                }

                if (_menuItems.TryGetValue(choice, out var menuItem))
                {
                    Console.Clear();
                    Console.WriteLine($"=== {menuItem.Name} ===\n");
                    try
                    {
                        await menuItem.Action();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\nError: {ex.Message}");
                    }
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                }

                Console.Clear();
            }
        }

        private void DisplayMenu()
        {
            Console.WriteLine("=== Finance Manager ===\n");
            foreach (var item in _menuItems)
            {
                Console.WriteLine($"{item.Key}. {item.Value.Description}");
            }
            Console.WriteLine("\nQ. Quit");
            Console.Write("\nEnter your choice: ");
        }
    }
} 