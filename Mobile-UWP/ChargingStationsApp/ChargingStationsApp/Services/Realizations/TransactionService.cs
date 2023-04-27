using ChargingStationsApp.Models;
using ChargingStationsApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChargingStationsApp.Services.Realizations
{
    internal class TransactionService : ITransactionService
    {
        private static readonly Tariff tariff = new Tariff
        {
            Id = 1,
            Name = "Tariff",
            Price = 1.69M,
            StartDate = new DateTime(2023, 4, 18),
            EndDate = new DateTime(2024, 4, 18)
        };

        private static readonly Session session = new Session
        {
            Id = 1,
            PaymentCardId = 1,
            Station = StationService.Stations[0]
        };

        private static readonly List<Transaction> transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = 1,
                Session = session,
                Tariff = tariff,
                ChargedEnergy = 10.5,
                StartDateTime = new DateTime(2023, 4, 17, 16, 41, 0),
                EndDateTime = new DateTime(2023, 4, 17, 17, 41, 0)
            },
            new Transaction
            {
                Id = 2,
                Session = session,
                Tariff = tariff,
                ChargedEnergy = 20.5,
                StartDateTime = new DateTime(2023, 4, 18, 16, 41, 0),
                EndDateTime = new DateTime(2023, 4, 18, 17, 41, 0)
            },
            new Transaction
            {
                Id = 3,
                Session = session,
                Tariff = tariff,
                ChargedEnergy = 15.5,
                StartDateTime = new DateTime(2023, 4, 19, 16, 41, 0),
                EndDateTime = new DateTime(2023, 4, 19, 17, 41, 0)
            }
        };

        public async Task<Transaction> GetTransactionAsync(int id)
        {
            return await Task.FromResult(transactions.Find(t => t.Id == id));
        }

        public async Task<ICollection<Transaction>> GetTransactionsAsync()
        {
            return await Task.FromResult(transactions);
        }

        public async Task<ICollection<Transaction>> GetTransactionsAsync(DateTime dateFrom, DateTime dateTo)
        {
            var transactions = await GetTransactionsAsync();
            return transactions
                .Where(tr => tr.StartDateTime.Date >= dateFrom && tr.EndDateTime.Date <= dateTo)
                .ToList();
        }
    }
}
