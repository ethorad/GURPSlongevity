
using System.Windows;
using ScottPlot;

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
            InitializeComponent();

            longevityTable = new LongevityTable();

            double[] dataX = new double[20];
            double[] data1 = new double[20];
            double[] data2 = new double[20];
            CharacterSheet foo = new CharacterSheet();
            foo.TL = 8;
            for (int i = 1; i <= 20; i++)
            {
                dataX[i - 1] = i;
                foo.ST = i; foo.DX = i; foo.IQ = i; foo.HT = i;

                foo.ExtendedLifespan = 0;
                data1[i - 1] = longevityTable.GetLongevity(foo);

                foo.ExtendedLifespan = 1;
                data2[i - 1] = longevityTable.GetLongevity(foo);
            }

            samplePlot.Plot.AddScatter(dataX, data1,label:"Normal");
            samplePlot.Plot.AddScatter(dataX, data2,label:"Extended");
            samplePlot.Plot.Legend(true, Alignment.UpperLeft);
            samplePlot.Plot.XLabel("Starting HT");
            samplePlot.Plot.YLabel("Age at death");
            samplePlot.Refresh();

        }
    }
}
