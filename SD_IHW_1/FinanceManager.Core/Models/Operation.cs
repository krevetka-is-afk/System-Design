using System;

namespace FinanceManager.Core.Models
{
    public class Operation
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; }

        public Operation(Guid accountId, Guid categoryId, decimal amount, string description = "")
        {
            if (accountId == Guid.Empty)
                throw new ArgumentException("Account ID cannot be empty", nameof(accountId));
            
            if (categoryId == Guid.Empty)
                throw new ArgumentException("Category ID cannot be empty", nameof(categoryId));

            Id = Guid.NewGuid();
            AccountId = accountId;
            CategoryId = categoryId;
            Amount = amount;
            Description = description;
            Date = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"{Date:g} - {Amount:N2} - {Description}";
        }
    }
} 