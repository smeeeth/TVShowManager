using System;
using System.Collections.Generic;
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
using IMDB_API_Accessor;

namespace TVMonitorUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MetaDataProcessor Processor;
        public MainWindow()
        {
            InitializeComponent();

            var text = System.IO.File.ReadAllText("C:/Users/erics/Workspace/IMDB_API_Accessor/IMDB_API_Accessor/TextFile302.txt");

            var num = DateTime.Now.Millisecond;
            System.IO.File.WriteAllText($"C:/Users/erics/Workspace/IMDB_API_Accessor/IMDB_API_Accessor/TextFile{num}.txt", text);

            MetaData meta = JSONInterpreter.ReadMetaData(text);

            Console.WriteLine(meta);

            Console.ReadLine();

            Processor = new MetaDataProcessor();
            Processor.ProcessMetaDatas(new List<MetaData> { meta });


            MainWindow..MyGrid = Processor.MainTable.Select(x => new { Value = x }).ToList();
            dgvSelectedNode.Show();

        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }


}
