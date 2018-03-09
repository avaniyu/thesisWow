using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WobbrockLib.Controls;
using WobbrockLib.Types;
using WobbrockLib.Extensions;

namespace TextTest
{
    public partial class ResultsForm : Form
    {
        private Graph gphWpm;  // words per minute
        private Graph gphErr;  // error rates

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sd"></param>
        public ResultsForm(SessionData sd, string filename)
        {
            InitializeComponent();
            this.Text = filename;

            // create a graph for speed
            gphWpm = new Graph();
            gphWpm.Title = "Speed";
            gphWpm.XAxisName = "Trial No.";
            gphWpm.YAxisName = "WPM";
            gphWpm.XAxisTicks = (sd.NumTrials > 1) ? sd.NumTrials - 1 : 1;
            gphWpm.XAxisDecimals = 1;
            gphWpm.YAxisDecimals = 1;
            gphWpm.Legend = true;

            // create a graph for error rates
            gphErr = new Graph();
            gphErr.Title = "Error Rates";
            gphErr.XAxisName = "Trial No.";
            gphErr.YAxisName = "Error Rate";
            gphErr.XAxisTicks = (sd.NumTrials > 1) ? sd.NumTrials - 1 : 1;
            gphErr.XAxisDecimals = 1;
            gphErr.YAxisDecimals = 3;
            gphErr.Legend = true;

            // create each series we wish to graph
            Graph.Series sWpm = new Graph.Series("WPM", Color.Red, Color.Red, true, true);
            Graph.Series sAdj = new Graph.Series("AdjWPM", Color.Gray, Color.Gray, true, true);
            Graph.Series sTot = new Graph.Series("Total", Color.Red, Color.Red, true, true);
            Graph.Series sUnc = new Graph.Series("Uncorrected", Color.Gray, Color.Gray, true, true);
            Graph.Series sCor = new Graph.Series("Corrected", Color.LightGray, Color.LightGray, true, true);

            // use these lists to compute the means and stdevs for each series
            List<double> mWpm = new List<double>();
            List<double> sdWpm = new List<double>();
            List<double> mAdj = new List<double>();
            List<double> sdAdj = new List<double>();
            List<double> mTot = new List<double>();
            List<double> sdTot = new List<double>();
            List<double> mUnc = new List<double>();
            List<double> sdUnc = new List<double>();
            List<double> mCor = new List<double>();
            List<double> sdCor = new List<double>();

            // add the points for each series
            for (int i = 0; i < sd.NumTrials; i++)
            {
                if (sd[i].NumEntries > 0)
                {
                    sWpm.AddPoint(new PointR(i + 1, sd[i].WPM));
                    mWpm.Add(sd[i].WPM);
                    sdWpm.Add(sd[i].WPM);

                    sAdj.AddPoint(new PointR(i + 1, sd[i].AdjWPM));
                    mAdj.Add(sd[i].AdjWPM);
                    sdAdj.Add(sd[i].AdjWPM);

                    sTot.AddPoint(new PointR(i + 1, sd[i].TotalErrorRate));
                    mTot.Add(sd[i].TotalErrorRate);
                    sdTot.Add(sd[i].TotalErrorRate);

                    sUnc.AddPoint(new PointR(i + 1, sd[i].UncorrectedErrorRate));
                    mUnc.Add(sd[i].UncorrectedErrorRate);
                    sdUnc.Add(sd[i].UncorrectedErrorRate);

                    sCor.AddPoint(new PointR(i + 1, sd[i].CorrectedErrorRate));
                    mCor.Add(sd[i].CorrectedErrorRate);
                    sdCor.Add(sd[i].CorrectedErrorRate);
                }
            }

            // add the means and standard deviations to the series' names
            sWpm.Name = String.Format("{0} (µ={1:f1}, σ={2:f1})", sWpm.Name, StatsEx.Mean(mWpm.ToArray()), StatsEx.StdDev(sdWpm.ToArray()));
            sAdj.Name = String.Format("{0} (µ={1:f1}, σ={2:f1})", sAdj.Name, StatsEx.Mean(mAdj.ToArray()), StatsEx.StdDev(sdAdj.ToArray()));
            sTot.Name = String.Format("{0} (µ={1:f3}, σ={2:f3})", sTot.Name, StatsEx.Mean(mTot.ToArray()), StatsEx.StdDev(sdTot.ToArray()));
            sUnc.Name = String.Format("{0} (µ={1:f3}, σ={2:f3})", sUnc.Name, StatsEx.Mean(mUnc.ToArray()), StatsEx.StdDev(sdUnc.ToArray()));
            sCor.Name = String.Format("{0} (µ={1:f3}, σ={2:f3})", sCor.Name, StatsEx.Mean(mCor.ToArray()), StatsEx.StdDev(sdCor.ToArray()));

            // add the origin point to the graphs
            Graph.Series origin = new Graph.Series(String.Empty, Color.Black, Color.Black, false, false);
            origin.AddPoint(1.0, 0.0);
            gphWpm.AddSeries(origin);
            gphErr.AddSeries(origin);

            // finally, add the series
            gphWpm.AddSeries(sWpm);
            gphWpm.AddSeries(sAdj);
            gphErr.AddSeries(sTot);
            gphErr.AddSeries(sUnc);
            gphErr.AddSeries(sCor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResultsForm_Load(object sender, EventArgs e)
        {
            tblResults.SuspendLayout();
            tblResults.Controls.Add(gphWpm, 0, 0);
            tblResults.Controls.Add(gphErr, 0, 1);
            gphWpm.Dock = DockStyle.Fill;
            gphErr.Dock = DockStyle.Fill;
            tblResults.ResumeLayout(false);
        }
    }
}
