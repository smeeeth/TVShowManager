﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using APIAccessor;
using APIAccessor.API;
using APIAccessor.Data;
using APIAccessor.FS;
using Microsoft.Win32;

namespace TVMonitorUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private static List<String> ColumnNames = new List<String> { "Id", "Title", "StartYear", "EndYear", "TotalSeasons", "NumEpisodes", "AverageRunningTimeMinutes", "Rating", "RatingCount", "Language", "Country", "Certificate" };

        private ObservableCollection<TVMetaData> MetaDatas = new ObservableCollection<TVMetaData>();

        private Dictionary<string, DataGridCheckBoxColumn> GenreCols = new Dictionary<string, DataGridCheckBoxColumn>();
        private DataGridColumn ColumnBeforeGenre;

        private Dictionary<string, DataGridCheckBoxColumn> WayToWatchCols = new Dictionary<string, DataGridCheckBoxColumn>();
        private DataGridColumn ColumnBeforeWayToWatch;

        private DBFileReader Reader = new DBFileReader("TVMonitorFile.tv");

        private string _IMDBName;
        public string IMDBName
        {
            get
            {
                return _IMDBName;
            }
            set
            {
                _IMDBName = value;
                NotifyPropertyChanged();
            }
        }
        private int _IMDBMaxCalls;
        public int IMDBMaxCalls
        {
            get
            {
                return _IMDBMaxCalls;
            }
            set
            {
                _IMDBMaxCalls = value;
                NotifyPropertyChanged();
            }
        }
        private int _IMDBCallsRemaining;
        public int IMDBCallsRemaining
        {
            get
            {
                return _IMDBCallsRemaining;
            }
            set
            {
                _IMDBCallsRemaining = value;
                NotifyPropertyChanged();
            }
        }


        private string _MovieDBName;
        public string MovieDBName
        {
            get
            {
                return _MovieDBName;
            }
            set
            {
                _MovieDBName = value;
                NotifyPropertyChanged();
            }
        }
        private int _MovieDBMaxCalls;
        public int MovieDBMaxCalls
        {
            get
            {
                return _MovieDBMaxCalls;
            }
            set
            {
                _MovieDBMaxCalls = value;
                NotifyPropertyChanged();
            }
        }
        private int _MovieDBCallsRemaining;
        public int MovieDBCallsRemaining
        {
            get
            {
                return _MovieDBCallsRemaining;
            }
            set
            {
                _MovieDBCallsRemaining = value;
                NotifyPropertyChanged();
            }
        }

        private string _ApiKey = APIManager.APIKEY;
        public string ApiKey
        {
            get
            {
                return _ApiKey;
            }
            set
            {
                _ApiKey = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(String propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();
            GenerateColumns();

            //Bind to item source
            DataGrid.ItemsSource = MetaDatas;
            MetaDatas.CollectionChanged += TableChanged;

            this.DataContext = this;

            IMDBName = nameof(IMDBAPI);
            MovieDBName = nameof(MovieDBAPI);

            APIManager.RequestMade += (sender, e) => {
                if (e.APIName.Equals(IMDBName))
                {
                    IMDBMaxCalls = e.MaxCalls;
                    IMDBCallsRemaining = e.CallsRemaining;
                } else if (e.APIName.Equals(MovieDBName))
                {
                    MovieDBMaxCalls = e.MaxCalls;
                    MovieDBCallsRemaining = e.CallsRemaining;

                }
            };
          
            //Get test data (id does not matter)
            TVMetaData data = APIManager.GetByID("tt0306414");
            MetaDatas.Add(data);

            NotifyPropertyChanged();
        }

        /// <summary>
        /// Creates columns based on ColumnNames
        /// </summary>
        public void GenerateColumns()
        {
            foreach (string name in ColumnNames)
            {
                var col = CreateTextColumn(name);
                DataGrid.Columns.Add(col);
            }
            ColumnBeforeGenre = DataGrid.Columns[8];
            ColumnBeforeWayToWatch = DataGrid.Columns[11];
        }

        /// <summary>
        /// Changes table when rows are added/removed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TableChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (TVMetaData meta in e.NewItems)
                {
                    foreach (string genre in meta.Genres)
                    {
                        CheckForGenre(genre);
                    }
                    foreach (OptionGroup way in meta.WayToWatch.Groups)
                    {
                        foreach (WatchOption option in way.Options)
                        {
                            CheckForWaysToWatch(option.Primary);
                        }
                    }
                    meta.PropertyChanged += RowChanged;
                }
            }
            
            if (e.OldItems != null)
            {
                foreach(TVMetaData meta in e.OldItems)
                {
                    ///TODO: Remove columns with no more entries exist
                    meta.PropertyChanged -= RowChanged;
                }
            }
        }

        private void RowChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine();
        }

        /// <summary>
        /// Load "Db" file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".tv";
            dlg.Filter = "TVShowManager DB Files (*.tv)|*.tv";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                Reader = new DBFileReader(filename);
                MetaDatas.Clear();
                List<TVMetaData> metas = Reader.Read();
                foreach (TVMetaData thisMeta in metas)
                {
                    MetaDatas.Add(thisMeta);
                }
            }
        }
       private void EnterApiKey_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InputDialog();
            if (dialog.ShowDialog() == true)
            {
                APIManager.APIKEY = dialog.ResponseText;
                ApiKey = APIManager.APIKEY;
            }
        }

                /// <summary>
                /// Close app
                /// </summary>
                /// <param name="sender"></param>
                /// <param name="e"></param>
                private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// Generic make Text Column function
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        static private DataGridTextColumn CreateTextColumn(string header)
        {
            DataGridTextColumn col = new DataGridTextColumn();
            col.Header = header;
            var binding = new Binding(header);
            binding.Mode = BindingMode.OneWay;
            col.Binding = binding;
            col.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            return col;
        }

        /// <summary>
        /// Generic make Checkable Column function
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        static private DataGridCheckBoxColumn CreateCheckableColumn(string header, IValueConverter converter)
        {
            DataGridCheckBoxColumn col = new DataGridCheckBoxColumn();
            col.Header = header;
            var binding = new Binding();
            binding.Mode = BindingMode.OneWay;
            binding.Converter = converter;
            col.Binding = binding;
            col.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            return col;
        }

        /// <summary>
        /// Check if column for this Genre already exists
        /// </summary>
        /// <param name="genre"></param>
        private void CheckForGenre(String genre)
        {
            if (!GenreCols.ContainsKey(genre))
            {
                DataGridCheckBoxColumn col = CreateCheckableColumn(genre, new GenreCheckBoxConverter(genre));
                GenreCols.Add(genre, col);
                //Add to the end of the "Genre" stretch of columns
                DataGrid.Columns.Insert((DataGrid.Columns.IndexOf(ColumnBeforeGenre) + GenreCols.Count), col);
            }
        }

        /// <summary>
        /// Check if column for this WayToWatch already exists
        /// </summary>
        /// <param name="wayToWatch"></param>
        private void CheckForWaysToWatch(String wayToWatch)
        {
            if (!WayToWatchCols.ContainsKey(wayToWatch))
            {
                DataGridCheckBoxColumn col = CreateCheckableColumn(wayToWatch, new WayToWatchCheckBoxConverter(wayToWatch));
                WayToWatchCols.Add(wayToWatch, col);
                //Add to end of the "Way to Watch" stretch of columns
                DataGrid.Columns.Insert((DataGrid.Columns.IndexOf(ColumnBeforeWayToWatch) + WayToWatchCols.Count), col);
            }
        }

        /// <summary>
        /// Checks if given row contains the genre for this column
        /// </summary>
        public class GenreCheckBoxConverter : IValueConverter
        {
            private string Genre;

            public GenreCheckBoxConverter(string genre)
            {
                Genre = genre;
            }

            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if (value != null && value is TVMetaData)
                {
                    TVMetaData meta = (TVMetaData)value;
                    if (meta.Genres.Contains(Genre))
                        return true;
                }
                return false;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Checks if given row contains the way to watch for this column
        /// </summary>
        public class WayToWatchCheckBoxConverter : IValueConverter
        {
            private string WayToWatch;

            public WayToWatchCheckBoxConverter(string wayToWatch)
            {
                WayToWatch = wayToWatch;
            }

            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if (value != null && value is TVMetaData)
                {
                    TVMetaData meta = (TVMetaData)value;
                    foreach (OptionGroup way in meta.WayToWatch.Groups)
                    {
                        foreach (WatchOption option in way.Options)
                        {
                            if (option.Primary.Equals(WayToWatch))
                                return true;
                        }
                    }
                }
                return false;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets selected row
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private static TVMetaData GetSender(object sender)
        {
            //Get the clicked MenuItem
            MenuItem menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;

            //Find the placementTarget
            var item = (DataGrid)contextMenu.PlacementTarget;

            //Get the underlying item, that you cast to your object that is bound
            //to the DataGrid (and has subject and state as property)
            var markWatched = (TVMetaData)item.SelectedCells[0].Item;

            return markWatched;
        }

        /// <summary>
        /// Updates shows info from API
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update(object sender, RoutedEventArgs e)
        {
            var meta = GetSender(sender);

            //Send get request for id meta.Id
            //update metadata object

        }

        /// <summary>
        /// Mark the show watched
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MarkWatched(object sender, RoutedEventArgs e)
        {
            var meta = GetSender(sender);
            meta.Watched = true;
        }

        /// <summary>
        /// Show messagebox with show details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeeDetails(object sender, RoutedEventArgs e)
        {
            var meta = GetSender(sender);

            //Show message box with details
            MessageBox.Show("Title: " + meta.Title);
        }
    }
    public class RGColorConverter : IValueConverter
    {
        private static int LOW_REMAINING_THRESHOLD = 20;
        private static Brush NORMAL_BRUSH = Brushes.Green;
        private static Brush LOW_BRUSH = Brushes.Red;
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            var remainingCalls = (int)value;
            if (remainingCalls > LOW_REMAINING_THRESHOLD)
                return NORMAL_BRUSH;
            else
                return LOW_BRUSH;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
