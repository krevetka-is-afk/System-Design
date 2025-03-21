using System;
using FinanceManager.Core.Models;

namespace FinanceManager.Core.Factories
{
    public interface IFinanceFactory
    {
        BankAccount CreateBankAccount(string name, string currency, decimal initialBalance = 0);
        Category CreateCategory(string name, CategoryType type, string description = "");
        Operation CreateOperation(Guid accountId, Guid categoryId, decimal amount, string description = "");
    }
} 