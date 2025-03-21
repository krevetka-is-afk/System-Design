using System;

namespace FinanceManager.Core.Models
{
    public class BankAccount
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal Balance { get; private set; }
        public string? Currency { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }

        // Parameterless constructor for EF Core
        protected BankAccount() { }

        public BankAccount(string name, string currency, decimal balance = 0)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Account name cannot be empty", nameof(name));
            
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency cannot be empty", nameof(currency));

            Id = Guid.NewGuid();
            Name = name;
            Currency = currency;
            Balance = balance;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateBalance(decimal amount)
        {
            Balance += amount;
            LastModifiedAt = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"{Name} ({Currency}): {Balance:N2}";
        }
    }
} 