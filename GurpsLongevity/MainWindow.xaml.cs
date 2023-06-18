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
using static System.Net.Mime.MediaTypeNames;

namespace GurpsLongevity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LongevityTable longevityTable;
        public MainWindow()
        {
            longevityTable = new LongevityTable();
            longevityTable.GetLongevity(20, 20, 20, 20, false, 0);
            InitializeComponent();
        }

        private void doTest(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(longevityTable.GetLongevity(5, 7, 5, 10, false, 0).ToString());
        }
    }
}
