﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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

        public MainWindow()
        {
            InitializeComponent();
            GenerateColumns();
            
            //Bind to item source
            DataGrid.ItemsSource = MetaDatas;
            MetaDatas.CollectionChanged += TableChanged;

            //Get test data (id does not matter)
            TVMetaData data = APIAccessor.Program.GetByID("asdfasdf");
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
                }
            }
            
            if (e.OldItems != null)
            {
                foreach(TVMetaData meta in e.OldItems)
                {
                    ///TODO: Remove columns with no more entries exist
                }
            }
        }

        /// <summary>
        /// Load "Db" file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
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
        /// Check if column for this Genre already exists
        /// </summary>
        /// <param name="genre"></param>
        private void CheckForGenre(String genre)
        {
            if (!GenreCols.ContainsKey(genre))
            {
                DataGridCheckBoxColumn col = new DataGridCheckBoxColumn();
                col.Header = genre;
                var binding = new Binding();
                binding.Mode = BindingMode.OneWay;
                binding.Converter = new GenreCheckBoxConverter(genre);
                col.Binding = binding;

                col.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

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
                DataGridCheckBoxColumn col = new DataGridCheckBoxColumn();
                col.Header = wayToWatch;
                var binding = new Binding();
                binding.Mode = BindingMode.OneWay;
                binding.Converter = new WayToWatchCheckBoxConverter(wayToWatch);
                col.Binding = binding;
                col.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

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
    }
}
