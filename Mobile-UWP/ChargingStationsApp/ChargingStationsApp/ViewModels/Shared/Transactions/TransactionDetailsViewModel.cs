using ChargingStationsApp.Models;
using ChargingStationsApp.Services;
using ChargingStationsApp.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ChargingStationsApp.ViewModels.Shared.Transactions
{
    [QueryProperty(nameof(TransactionId), nameof(TransactionId))]
    class TransactionDetailsViewModel : BaseViewModel
    {
        private int transactionId;
        private Session session;
        private Tariff tariff;
        private double requestedEnergy;
        private double chargedEnergy;
        private decimal pay;
        private DateTime startDateTime;
        private DateTime endDateTime;

        private readonly ITransactionService transactionService;

        public TransactionDetailsViewModel()
        {
            transactionService = DependencyService.Get<ITransactionService>();
        }

        public int TransactionId
        {
            get => transactionId;
            set => transactionId = value;
        }

        public Session Session
        {
            get => session;
            set => SetProperty(ref session, value);
        }

        public Tariff Tariff
        {
            get => tariff;
            set => SetProperty(ref tariff, value);
        }

        public double RequestedEnergy
        {
            get => requestedEnergy;
            set => SetProperty(ref requestedEnergy, value);
        }

        public double ChargedEnergy
        {
            get => chargedEnergy;
            set => SetProperty(ref chargedEnergy, value);
        }

        public DateTime StartDateTime
        {
            get => startDateTime;
            set => SetProperty(ref startDateTime, value);
        }

        public DateTime EndDateTime
        {
            get => endDateTime;
            set => SetProperty(ref endDateTime, value);
        }

        public decimal Pay
        {
            get => pay;
            set => SetProperty(ref pay, value);
        }

        public async Task OnAppearing()
        {
            await LoadTransaction();
        }

        private async Task LoadTransaction()
        {
            var transaction = transactionId > 0
                ? await transactionService.GetTransactionAsync(transactionId)
                : SessionInfo.LastTransaction;

            Session = transaction.Session;
            Tariff = transaction.Tariff;
            RequestedEnergy = transaction.RequestedEnergy;
            ChargedEnergy = transaction.ChargedEnergy;
            StartDateTime = transaction.StartDateTime;
            EndDateTime = transaction.EndDateTime;

            Pay = (decimal) ChargedEnergy * Tariff.Price;
        }
    }
}
