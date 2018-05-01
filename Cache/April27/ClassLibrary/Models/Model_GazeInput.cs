namespace ClassLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Model_GazeInput
    {
        private List<double> gazePoint;
        protected List<List<double>> GazeLocus { get; }

        /// <summary>
        /// Get gaze locus of current word & map gaze locus to keyboard & generate associated visual feedback/animation
        /// </summary>
        /// <returns></returns>
        protected void GetGazeLocus()
        {
            //GazeLocus.Add(gazePoint);
            GazeLocus.Add(new List<double>{ 1.1, 2.1});
            GazeLocus.Add(new List<double> { 2.1, 3.1 });
            GazeLocus.Add(new List<double> { 3.1, 4.1 });
            GazeLocus.Add(new List<double> { 4.1, 5.1 });
            GazeLocus.Add(new List<double> { 5.1, 6.1 });
            GazeLocus.Add(new List<double> { 6.1, 7.1 });
        }

        protected void ResetGazeLocus()
        {
            GazeLocus.Clear();
            //reset the starting point or other related animations
        }
    }
}
