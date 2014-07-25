using FirstFloor.ModernUI.Windows.Controls;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TogglReport.Domain.Model;
using TogglReport.Domain.Repository;
using TogglReport.Domain.Services;

namespace TogglReport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private double _totalRoundedHours = 0;

        private DateTime _currentQueryDate;

        public DateTime CurrentQueryDate
        {
            get { return _currentQueryDate; }
            set { _currentQueryDate = value; }
        }

        private ObservableCollection<TimeEntry> _Items;

        public ObservableCollection<TimeEntry> Items
        {
            get { return _Items; }
            set { _Items = value; }
        }

        public double TotalRoundedHours
        {
            set
            {
                _totalRoundedHours = value;
            }
            get
            {
                return _totalRoundedHours;
            }
        }

        public MainWindow()
        {
            this.CurrentQueryDate = DateTime.Now.AddDays(-1);
            LoadDataFromDB();

            this.DataContext = this;
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }


        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ItemsGrid.DataContext = Items;
        }


        private void LoadDataFromDB()
        {
            this._Items = new ObservableCollection<TimeEntry>();
            TimeEntryRepository timeEntryCollection = new TimeEntryRepository();
            this._Items = timeEntryCollection.GetGroupingByDescAndDay();
        }


        private void Today_Click(object sender, RoutedEventArgs e)
        {
            

            TimeEntryRepository timeEntryCollection = new TimeEntryRepository();
            this.Items.Clear();

            DateTime today = DateTime.Now;
            this.CurrentQueryDate = today;

            TimeEntryCollectionService timeentries = timeEntryCollection.GetGroupingByDescAndDayForToday();

            foreach (var item in timeentries)
	        {
                this.Items.Add(item);
	        }

        }

        private void All_Click(object sender, RoutedEventArgs e)
        {
            TimeEntryRepository timeEntryCollection = new TimeEntryRepository();
            this.Items.Clear();

            TimeEntryCollectionService timeentries = timeEntryCollection.GetGroupingByDescAndDay();

            foreach (var item in timeentries)
            {
                this.Items.Add(item);
            }

        }

        private void Yesterday_Click(object sender, RoutedEventArgs e)
        {
            TimeEntryRepository timeEntryCollection = new TimeEntryRepository();
            this.Items.Clear();

            TimeEntryCollectionService timeentries = timeEntryCollection.GetGroupingByDescAndDayForYesterday();

            DateTime yesterday = DateTime.Now.AddDays(-1);
            this.CurrentQueryDate = yesterday;

            foreach (var item in timeentries)
            {
                this.Items.Add(item);
            }
        }

        private void DayBefore_Click(object sender, RoutedEventArgs e)
        {
            CurrentQueryDate = CurrentQueryDate.AddDays(-1);
        }

        private void DayAfter_Click(object sender, RoutedEventArgs e)
        {
            CurrentQueryDate = CurrentQueryDate.AddDays(1);
        }

        private void ReloadData_Click(object sender, RoutedEventArgs e)
        {
            this.LoadDataFromDB();
        }

    }
}
