using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
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
using APIAccessor;

namespace TVMonitorUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static List<String> ColumnNames = new List<String> { "Id", "Title", "StartYear", "EndYear", "TotalSeasons", "NumEpisodes", "AverageRunningTimeMinutes", "Rating", "RatingCount", "Language", "Country", "Certificate" };

        private ObservableCollection<TVMetaData> MetaDatas = new ObservableCollection<TVMetaData>();

        private Dictionary<string, DataGridCheckBoxColumn> GenreCols = new Dictionary<string, DataGridCheckBoxColumn>();
        private DataGridColumn ColumnBeforeGenre;

        private Dictionary<string, DataGridCheckBoxColumn> WayToWatchCols = new Dictionary<string, DataGridCheckBoxColumn>();
        private DataGridColumn ColumnBeforeWayToWatch;
        private int NextWayToWatchColumnIndex = 12;

        private TVMetaData m;

        public MainWindow()
        {
            InitializeComponent();
            GenerateColumns();

            TVMetaData data = APIAccessor.Program.GetByID("asdfasdf");
            m = data;

            Console.WriteLine(data);

            Console.ReadLine();

            DataGrid.ItemsSource = MetaDatas;
            MetaDatas.CollectionChanged += TableChanged;

            MetaDatas.Add(data);
        }

        public void GenerateColumns()
        {
            foreach (string str in ColumnNames)
            {
                DataGridTextColumn col = new DataGridTextColumn();
                col.Header = str;
                var binding = new Binding(str);
                binding.Mode = BindingMode.OneWay;
                col.Binding = binding;
                col.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                DataGrid.Columns.Add(col);
            }
            ColumnBeforeGenre = DataGrid.Columns[8];
            ColumnBeforeWayToWatch = DataGrid.Columns[11];
        }

        private void TableChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null) {
                foreach(TVMetaData meta in e.NewItems)
                {
                    foreach(string genre in meta.Genres)
                    {
                        CheckForGenre(genre);
                    }
                    foreach(OptionGroup way in meta.WayToWatch.Groups)
                    {
                        foreach (WatchOption option in way.Options)
                        {
                            CheckForWaysToWatch(option.Primary);
                        }
                    }
                }
            }
        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            MetaDatas.Add(m);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void CheckForGenre(String genre)
        {
            if (!GenreCols.ContainsKey(genre))
            {
                DataGridCheckBoxColumn col = new DataGridCheckBoxColumn();
                col.Header = genre;
                var binding = new Binding();
                binding.Mode = BindingMode.OneWay;
                col.Binding = binding;
                col.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

                GenreCols.Add(genre, col);

                //Add to the end of the "Genre" stretch of columns
                DataGrid.Columns.Insert((DataGrid.Columns.IndexOf(ColumnBeforeGenre)+GenreCols.Count), col);
            }
        }

        private void CheckForWaysToWatch(String wayToWatch)
        {
            if (!WayToWatchCols.ContainsKey(wayToWatch))
            {
                DataGridCheckBoxColumn col = new DataGridCheckBoxColumn();
                col.Header = wayToWatch;
                var binding = new Binding();
                binding.Mode = BindingMode.OneWay;
                col.Binding = binding;
                col.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

                WayToWatchCols.Add(wayToWatch, col);

                //Add to end of the "Way to Watch" stretch of columns
                DataGrid.Columns.Insert((DataGrid.Columns.IndexOf(ColumnBeforeWayToWatch) + WayToWatchCols.Count), col);
            }

        }
    }
}
