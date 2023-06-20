
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

            double[] dataX = new double[21];
            double[] data1 = new double[21];
            double[] data2 = new double[21];
            CharacterSheet foo = new CharacterSheet();
            for (int i = 0; i < 21; i++)
            {
                dataX[i] = i;
                foo.Stats.ST = i; foo.Stats.DX = i; foo.Stats.IQ = i; foo.Stats.HT = i;

                foo.Stats.Fitness = eFitness.Normal;
                foo.Stats.TL = 5;
                data1[i] = longevityTable.GetLongevity(foo);
                
                foo.Stats.Fitness = eFitness.VeryFit;
                foo.Stats.TL = 3;
                data2[i] = longevityTable.GetLongevity(foo);
            }

            samplePlot.Plot.AddScatter(dataX, data1,label:"Normal TL 5");
            samplePlot.Plot.AddScatter(dataX, data2,label:"Very Fit TL 3");
            samplePlot.Plot.Legend(true, Alignment.UpperLeft);
            samplePlot.Plot.XLabel("Starting HT");
            samplePlot.Plot.YLabel("Age at death");
            samplePlot.Refresh();

        }
    }
}
