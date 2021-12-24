using APIProxy;
using APIProxy.ResposeModels;
using Prism.Commands;
using Prism.Mvvm;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace SearchRankingApp.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        private readonly ISearchRankingAPIProxy _searchRankingAPIProxy;

        public MainWindowViewModel(ISearchRankingAPIProxy searchRankingAPIProxy)
        {
            _searchRankingAPIProxy = searchRankingAPIProxy;
        }

        private string domain = string.Empty;                
        public string Domain
        {
            get => domain;
            set 
            { 
                SetProperty<string>(ref domain, value);
                FetchRankingResultsCommand.RaiseCanExecuteChanged();
            }
        }

        private string searchTerm = string.Empty;
        public string SearchTerm
        {
            get => searchTerm;
            set  
            { 
                SetProperty<string>(ref searchTerm, value);
                FetchRankingResultsCommand.RaiseCanExecuteChanged();
            }
        }
                
        private string currentRanking = string.Empty;
        public string CurrentRanking
        {
            get => currentRanking;
            set => SetProperty<string>(ref currentRanking, value);
        }

        private Visibility fetchingResultVisibility = Visibility.Collapsed;
        public Visibility FetchingResultVisibility
        {
            get => fetchingResultVisibility;
            set => SetProperty<Visibility>(ref fetchingResultVisibility, value);
        }

        private IEnumerable<SearchRankingResult> allRankings = null;
        public IEnumerable<SearchRankingResult> AllRankings
        {
            get => allRankings;
            set => SetProperty<IEnumerable<SearchRankingResult>>(ref allRankings, value);
        }
       
        private DelegateCommand fetchRankingResultsCommand = null;
        public DelegateCommand FetchRankingResultsCommand => fetchRankingResultsCommand ?? (fetchRankingResultsCommand = new DelegateCommand(FetchRankingResultsCommandExeucte, FetchRankingResultsCommandCanExeucte));

        private async void FetchRankingResultsCommandExeucte()
        {

            FetchingResultVisibility = Visibility.Visible;
            try
            {
                var getRankingByHostTask = _searchRankingAPIProxy.GetRankingResultByHost(Domain, SearchTerm);
                var getAllRankingsTask = _searchRankingAPIProxy.GetAllRankingResults(searchTerm);

                await Task.WhenAll(getRankingByHostTask, getAllRankingsTask);

                CurrentRanking = $"{Domain} current search result ranking for `{searchTerm}` is {getRankingByHostTask.Result.Ranking}";
                AllRankings = getAllRankingsTask.Result;
            }
            catch(Exception ex)
            {
                Log.Logger.Error(ex.Message);
                MessageBox.Show($"Unexpected error occured when calling API: {ex.Message}");
            }
            finally
            {
                FetchingResultVisibility = Visibility.Collapsed;
            }            
        }

        private bool FetchRankingResultsCommandCanExeucte()
        {
            return (string.IsNullOrWhiteSpace(SearchTerm) || string.IsNullOrWhiteSpace(Domain) ? false : true);            
        }
    }
}
