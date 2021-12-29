using System.Windows;
using System.Windows.Controls;

namespace SearchRankingApp.Views.UserControls
{
    /// <summary>
    /// Interaction logic for RankingListUserControl.xaml
    /// </summary>
    public partial class RankingListUserControl : UserControl
    {
        public RankingListUserControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SearchedDomainProperty = DependencyProperty.Register(
            "SearchedDomain", typeof(string),
            typeof(MainWindow)
            );

        public string SearchedDomain
        {
            get => (string)GetValue(SearchedDomainProperty);
            set => SetValue(SearchedDomainProperty, value);
        }
    }
}
