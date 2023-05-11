using ChargingStationsApp.Models;
using ChargingStationsApp.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System;
using Xamarin.Forms;
using ChargingStationsApp.Views.Shared.Transactions;
using ChargingStationsApp.Services;
using ChargingStationsApp.Enums;
using System.Linq;
using System.Collections.Generic;

namespace ChargingStationsApp.ViewModels.Shared.Transactions
{
    internal class TransactionsViewModel : BaseViewModel
    {
        private Transaction selectedTransaction;
        private readonly SearchOptions searchOptions;
        private readonly ITransactionService transactionService;

        public TransactionsViewModel()
        {
            searchOptions = SessionInfo.TransactionSearchOptions;
            transactionService = DependencyService.Get<ITransactionService>();

            Transactions = new ObservableCollection<Transaction>();

            LoadTransactionsCommand = new Command(
                async () => await LoadTransactionsAsync());
            OpenSearchOptionsCommand = new Command(
                async () => await OnOpenSearchOptionsAsync());

            TransactionTapped = new Command<Transaction>(
                async (transaction) => await OnTransactionTappedAsync(transaction));
        }

        public ObservableCollection<Transaction> Transactions { get; }
        public Command LoadTransactionsCommand { get; }
        public Command OpenSearchOptionsCommand { get; }
        public Command<Transaction> TransactionTapped { get; }

        public bool ListIsEmpty { get => !Transactions.Any(); }

        public Transaction SelectedTransaction
        {
            get => selectedTransaction;
            set => SetProperty(ref selectedTransaction, value);
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedTransaction = null;
        }

        private async Task LoadTransactionsAsync()
        {
            IsBusy = true;

            try
            {
                Transactions.Clear();
                var transactions = await GetSearchedTransactionsAsync();

                foreach (var trans in transactions)
                {
                    Transactions.Add(trans);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<ICollection<Transaction>> GetSearchedTransactionsAsync()
        {
            if (searchOptions.Apply)
            {
                var transactions = await transactionService
                    .GetTransactionsByStationAsync(searchOptions.StationName, searchOptions.DateFrom, searchOptions.DateTo);

                switch (searchOptions.SortBy)
                {
                    case SortBy.Date:
                        return searchOptions.SortOrder == SortOrder.Ascending
                            ? transactions.OrderBy(tr => tr.StartDateTime).ToList()
                            : transactions.OrderByDescending(tr => tr.StartDateTime).ToList();
                    case SortBy.Energy:
                        return searchOptions.SortOrder == SortOrder.Ascending
                            ? transactions.OrderBy(tr => tr.ChargedEnergy).ToList()
                            : transactions.OrderByDescending(tr => tr.ChargedEnergy).ToList();
                    case SortBy.Pay:
                        return searchOptions.SortOrder == SortOrder.Ascending
                            ? transactions.OrderBy(tr => (decimal)tr.ChargedEnergy * tr.Tariff.Price).ToList()
                            : transactions.OrderByDescending(tr => (decimal)tr.ChargedEnergy * tr.Tariff.Price).ToList();
                }
            }

            return await transactionService
                .GetTransactionsByStationAsync(searchOptions.StationName);
        }

        private async Task OnOpenSearchOptionsAsync()
        {
            await Shell.Current.GoToAsync(nameof(SearchOptionsPage));
        }

        private async Task OnTransactionTappedAsync(Transaction transaction)
        {
            if (transaction is null)
            {
                return;
            }

            await Shell.Current.GoToAsync(
                $"{nameof(TransactionDetailsPage)}?{nameof(TransactionDetailsViewModel.TransactionId)}={transaction.Id}");
        }
    }
}
