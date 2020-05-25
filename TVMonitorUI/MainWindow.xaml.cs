using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public MetaDataProcessor Processor;
        public ObservableCollection<TVMetaData> MetaDatas = new ObservableCollection<TVMetaData>();

        public MainWindow()
        {
            InitializeComponent();

            TVMetaData data = APIAccessor.Program.GetByID("asdfasdf");

            Console.WriteLine(data);

            Console.ReadLine();

            MetaDatas.Add(data);

            DataGrid.ItemsSource = MetaDatas;

        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }


}
