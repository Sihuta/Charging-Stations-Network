using ChargingStationsApp.Extensions;
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
        public static readonly Tariff Tariff = new Tariff
        {
            Id = 1,
            Name = "Standard",
            Price = 9.69M,
            StartDate = new DateTime(2023, 4, 18),
            EndDate = new DateTime(2024, 4, 18)
        };

        private static readonly List<Session> sessions = new List<Session>
        {
            new Session
            {
                Id = 1,
                UserId = 1,
                Station = StationService.Stations[0]
            },
            new Session
            {
                Id = 2,
                UserId = 1,
                Station = StationService.Stations[0]
            },
            new Session
            {
                Id = 3,
                UserId = 1,
                Station = StationService.Stations[0]
            },
        };

        private static readonly List<Transaction> transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = 1,
                Session = sessions[0],
                Tariff = Tariff,
                RequestedEnergy = 12,
                ChargedEnergy = 10.5,
                StartDateTime = new DateTime(2023, 4, 17, 16, 41, 0),
                EndDateTime = new DateTime(2023, 4, 17, 17, 41, 0)
            },
            new Transaction
            {
                Id = 2,
                Session = sessions[1],
                Tariff = Tariff,
                RequestedEnergy = 23,
                ChargedEnergy = 20.5,
                StartDateTime = new DateTime(2023, 4, 18, 16, 41, 0),
                EndDateTime = new DateTime(2023, 4, 18, 17, 41, 0)
            },
            new Transaction
            {
                Id = 3,
                Session = sessions[2],
                Tariff = Tariff,
                RequestedEnergy = 15.5,
                ChargedEnergy = 15.5,
                StartDateTime = new DateTime(2023, 4, 19, 16, 41, 0),
                EndDateTime = new DateTime(2023, 4, 19, 17, 41, 0)
            }
        };

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            transaction.Id = transactions.Last().Id + 1;
            transactions.Add(transaction);

            return await Task.FromResult(transaction);
        }

        public async Task<Transaction> GetTransactionAsync(int id)
        {
            return await Task.FromResult(
                transactions.Find(t => t.Id == id));
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

        public async Task<ICollection<Transaction>> GetTransactionsByStationAsync(string stationName)
        {
            var transactions = await GetTransactionsAsync();
            if (stationName is null)
            {
                return transactions;
            }

            return transactions
                .Where(tr => tr.Session.Station.Name.ContainsIgnoreCase(stationName))
                .ToList();
        }

        public async Task<ICollection<Transaction>> GetTransactionsByStationAsync(string stationName, DateTime dateFrom, DateTime dateTo)
        {
            var transactions = await GetTransactionsByStationAsync(stationName);
            return transactions
                .Where(tr => tr.StartDateTime.Date >= dateFrom && tr.EndDateTime.Date <= dateTo)
                .ToList();
        }

        public async Task<bool> UpdateTransactionAsync(Transaction transaction)
        {
            var updTransaction = transactions.Find(t => t.Id == transaction.Id);

            updTransaction.Tariff = transaction.Tariff;
            updTransaction.StartDateTime = transaction.StartDateTime;
            updTransaction.EndDateTime = transaction.EndDateTime;
            updTransaction.ChargedEnergy = transaction.ChargedEnergy;
            updTransaction.RequestedEnergy = transaction.RequestedEnergy;
            updTransaction.Session = updTransaction.Session;

            return await Task.FromResult(true);
        }
    }
}
