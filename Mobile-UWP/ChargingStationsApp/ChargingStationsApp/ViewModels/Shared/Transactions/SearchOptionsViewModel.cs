using ChargingStationsApp.Enums;
using ChargingStationsApp.Localization;
using ChargingStationsApp.Models;
using ChargingStationsApp.Services;
using ChargingStationsApp.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ChargingStationsApp.ViewModels.Shared.Transactions
{
    class SearchOptionsViewModel : BaseViewModel
    {
        private LocalizedOption sortBy;
        private LocalizedOption sortOrder;

        private int sortByInd;
        private int sortOrderInd;

        private DateTime dateFrom;
        private DateTime dateTo;

        private string stationName;

        private readonly SearchOptions searchOptions;
        private readonly ITransactionService transactionService;

        public SearchOptionsViewModel()
        {
            searchOptions = SessionInfo.TransactionSearchOptions;
            transactionService = DependencyService.Get<ITransactionService>();

            CancelAllCommand = new Command(async (_) => await OnClearAllAsync());
            ApplyCommand = new Command(
                async (_) => await OnApplyAsync(), ValidateApply);
            PropertyChanged +=
                (_, __) => ApplyCommand.ChangeCanExecute();
        }

        public Command CancelAllCommand { get; }
        public Command ApplyCommand { get; }

        public LocalizedOption[] SortOptions
        {
            get => GetLocalizedOptions(typeof(SortBy));
        }

        public LocalizedOption[] SortOrderOptions
        {
            get => GetLocalizedOptions(typeof(SortOrder));
        }

        public DateTime MaxDate { get; private set; }

        public int SortByInd
        {
            get => sortByInd;
            set => SetProperty(ref sortByInd, value);
        }

        public int SortOrderInd
        {
            get => sortOrderInd;
            set => SetProperty(ref sortOrderInd, value);
        }

        public LocalizedOption SortBy
        {
            get => sortBy;
            set => SetProperty(ref sortBy, value);
        }
        public LocalizedOption SortOrder
        {
            get => sortOrder;
            set => SetProperty(ref sortOrder, value);
        }

        public DateTime DateFrom
        {
            get => dateFrom;
            set => SetProperty(ref dateFrom, value);
        }

        public DateTime DateTo
        {
            get => dateTo;
            set => SetProperty(ref dateTo, value);
        }

        public string StationName
        {
            get => stationName;
            set => SetProperty(ref stationName, value);
        }

        public async Task OnAppearing()
        {
            await LoadDateConstraints();
            LoadSearchOptions();
        }

        private bool ValidateApply(object _)
        {
            return sortBy != null
                && sortOrder != null
                && dateFrom != null
                && dateTo != null;
        }

        private async Task OnApplyAsync()
        {
            searchOptions.SortBy = (SortBy)sortByInd;
            searchOptions.SortOrder = (SortOrder)sortOrderInd;

            searchOptions.DateFrom = DateFrom;
            searchOptions.DateTo = DateTo;

            searchOptions.StationName = StationName;
            searchOptions.Apply = true;

            await Shell.Current.GoToAsync("..");
        }

        private async Task OnClearAllAsync()
        {
            searchOptions.Apply = false;
            await Shell.Current.GoToAsync("..");
        }

        private async Task LoadDateConstraints()
        {
            var transactions = await transactionService.GetTransactionsAsync();

            DateFrom = transactions.Min(t => t.StartDateTime);
            DateTo = transactions.Max(t => t.EndDateTime);
            MaxDate = DateTime.Now;
        }

        private void LoadSearchOptions()
        {
            if (searchOptions.Apply)
            {
                var sortByKey = searchOptions.SortBy.ToString();
                var sortOrderKey = searchOptions.SortOrder.ToString();

                sortBy = new LocalizedOption
                {
                    Key = sortByKey,
                    Value = TranslateExtension.GetValue(sortByKey)
                };
                sortOrder = new LocalizedOption
                {
                    Key = sortOrderKey,
                    Value = TranslateExtension.GetValue(sortOrderKey)
                };

                SortByInd = (int)searchOptions.SortBy;
                SortOrderInd = (int)searchOptions.SortOrder;

                DateFrom = searchOptions.DateFrom;
                DateTo = searchOptions.DateTo;

                StationName = searchOptions.StationName;
            }
        }

        private LocalizedOption[] GetLocalizedOptions(Type optionsType)
            => Enum.GetNames(optionsType)
                .Select(opt => new LocalizedOption
                {
                    Key = opt,
                    Value = TranslateExtension.GetValue(opt)
                }).ToArray();
    }
}
