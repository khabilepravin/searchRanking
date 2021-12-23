using APIProxy;
using Prism.Commands;
using Prism.Mvvm;

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
            set
            {
                SetProperty<string>(ref currentRanking, value);
            }
        }

        private DelegateCommand fetchRankingResultsCommand = null;
        public DelegateCommand FetchRankingResultsCommand => fetchRankingResultsCommand ?? (fetchRankingResultsCommand = new DelegateCommand(FetchRankingResultsCommandExeucte, FetchRankingResultsCommandCanExeucte));

        private async void FetchRankingResultsCommandExeucte()
        {
            //System.Windows.MessageBox.Show("Command fired");
            var result = await _searchRankingAPIProxy.GetAllRankingResults(searchTerm);

            var rankingResult = await _searchRankingAPIProxy.GetRankingResultByHost(Domain, SearchTerm);

            CurrentRanking = $"{Domain} current search result ranking for `{searchTerm}` is {rankingResult.Ranking}";
        }

        private bool FetchRankingResultsCommandCanExeucte()
        {
            if(string.IsNullOrWhiteSpace(SearchTerm) || string.IsNullOrWhiteSpace(Domain))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
