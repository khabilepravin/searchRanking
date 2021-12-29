using APIProxy;
using APIProxy.ResposeModels;
using Flurl.Http;
using Prism.Commands;
using Prism.Mvvm;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace SearchRankingApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
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

        private string errorMessage = null;
        public string ErrorMessage
        {
            get => errorMessage;
            set => SetProperty<string>(ref errorMessage, value);
        }

        private DelegateCommand fetchRankingResultsCommand = null;
        public DelegateCommand FetchRankingResultsCommand => fetchRankingResultsCommand ?? (fetchRankingResultsCommand = new DelegateCommand(FetchRankingResultsCommandExeucte, FetchRankingResultsCommandCanExeucte));

        public async void FetchRankingResultsCommandExeucte()
        {

            FetchingResultVisibility = Visibility.Visible;
            try
            {
                var getRankingByHostTask = _searchRankingAPIProxy.GetRankingResultByHost(Domain, SearchTerm);
                var getAllRankingsTask = _searchRankingAPIProxy.GetAllRankingResults(searchTerm);

                await Task.WhenAll(getRankingByHostTask, getAllRankingsTask);

                CurrentRanking = $"{Domain} current search result ranking for `{searchTerm}` is {getRankingByHostTask.Result.Ranking}";
                AllRankings = getAllRankingsTask.Result;
                ErrorMessage = null;
            }
            catch(FlurlHttpException ex)
            {
                Log.Logger.Error(ex.Message);
                if(ex.StatusCode == 404)
                {
                    ErrorMessage = null;
                    CurrentRanking = $"{Domain} is not one of the top 100 results for search term: {SearchTerm}";
                    AllRankings = null;
                }
                else
                {
                    HandleErrorState(ex);
                }
            }
            catch (Exception ex)
            {
                HandleErrorState(ex);
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

        private void HandleErrorState(Exception ex)
        {
            Log.Logger.Error(ex.Message);
            ErrorMessage = $"Unexpected error occured when calling API: {ex.Message}";
            CurrentRanking = null;
            AllRankings = null;
        }       
    }
}
