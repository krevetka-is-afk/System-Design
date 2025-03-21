using System;

namespace FinanceManager.Core.Models
{
    public enum CategoryType
    {
        Income,
        Expense
    }

    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CategoryType Type { get; set; }
        public DateTime CreatedAt { get; set; }

        public Category(string name, CategoryType type, string description = "")
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name cannot be empty", nameof(name));

            Id = Guid.NewGuid();
            Name = name;
            Type = type;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"{Name} ({Type})";
        }
    }
} 