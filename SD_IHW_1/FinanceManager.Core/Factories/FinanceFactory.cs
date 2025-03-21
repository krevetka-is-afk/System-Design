using System;
using FinanceManager.Core.Models;

namespace FinanceManager.Core.Factories
{
    public class FinanceFactory : IFinanceFactory
    {
        public BankAccount CreateBankAccount(string name, string currency, decimal initialBalance = 0)
        {
            return new BankAccount(name, currency, initialBalance);
        }

        public Category CreateCategory(string name, CategoryType type, string description = "")
        {
            return new Category(name, type, description);
        }

        public Operation CreateOperation(Guid accountId, Guid categoryId, decimal amount, string description = "")
        {
            return new Operation(accountId, categoryId, amount, description);
        }
    }
} 