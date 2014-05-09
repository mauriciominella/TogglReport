using FirstFloor.ModernUI.Windows.Controls;
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
    public partial class MainWindow : Window
    {
        private double _totalRoundedHours = 0;

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
            this._Items = new ObservableCollection<TimeEntry>();
            TimeEntryRepository timeEntryCollection = new TimeEntryRepository();
            this._Items = timeEntryCollection.GetGroupingByDescAndDay();
            this.DataContext = this;
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ItemsGrid.DataContext = Items;
        }

        private void Today_Click(object sender, RoutedEventArgs e)
        {
            TimeEntryRepository timeEntryCollection = new TimeEntryRepository();
            this.Items.Clear();

            DateTime today = DateTime.Now;

            TimeEntryCollectionService timeentries = timeEntryCollection.GetGroupingByDescAndDayForToday();

            foreach (var item in timeentries)
	        {
                this.Items.Add(item);
	        }

            //this.lblTotalRoundedHours.Text = timeentries.TotalHoursRounded.ToString();
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

            foreach (var item in timeentries)
            {
                this.Items.Add(item);
            }

            
            //this.lblTotalRoundedHours.Text = timeentries.TotalHoursRounded.ToString();
        }
    }
}
