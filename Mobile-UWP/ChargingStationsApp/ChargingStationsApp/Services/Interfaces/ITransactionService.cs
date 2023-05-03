using ChargingStationsApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChargingStationsApp.Services.Interfaces
{
    internal interface ITransactionService
    {
        Task<ICollection<Transaction>> GetTransactionsAsync();
        Task<ICollection<Transaction>> GetTransactionsAsync(DateTime dateFrom, DateTime dateTo);
        Task<Transaction> GetTransactionAsync(int id);
    }
}
