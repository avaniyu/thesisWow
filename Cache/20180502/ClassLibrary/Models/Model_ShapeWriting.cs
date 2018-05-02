namespace ClassLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ClassLibrary.Models;

    public class Model_ShapeWriting
    {
        // haven't limited the amount of predictorCandidates for now

        //public List<string> Predictors { get; private set; }

        private List<string> pruneTemplate(List<List<double>> gazeLocus)
        {
            var predictorsRd1 = new List<string>();
            return predictorsRd1;
        }

        public List<string> PredictWord(List<double>gazeLocus, List<string> predictorCandidates, string typedText)
        {
            List<string> predictorsRanked = new List<string>();
            var rankComposited = this.CompositeRank(gazeLocus, predictorCandidates, typedText);
            var rankCompositedSorted = rankComposited;
            rankCompositedSorted.Sort();
            foreach(var item in rankCompositedSorted)
            {
                var i = 0;
                for (; i < rankComposited.Count; i++)
                {
                    if (item == rankComposited[i])
                        break;
                };
                predictorsRanked.Add(predictorCandidates[i]);
            }
            return predictorsRanked;
        }

        private List<double> CompositeRank(List<double> gazeLocus, List<string> predictorCandidates, string typedText)
        {
            var rankByShape = this.RankByShape(gazeLocus, predictorCandidates);
            var rankByPosition = this.RankByPosition(gazeLocus, predictorCandidates);
            var rankByLanguageModel = this.RankByLanguageModel(predictorCandidates, typedText);
            var rankComposited = new List<double>();
            // calculated as an arismatic average for now
            for (var i = 0; i < rankByShape.Count; i++)
            {
                rankComposited.Add((rankByShape[i] + rankByPosition[i] + rankByLanguageModel[i]) / 3);
            }
            return rankComposited;
        }

        private List<double> RankByShape(List<double> gazeLocus, List<string> predictorCandidates)
        {
            var rank = new List<double>();
            return rank;
        }

        private List<double> RankByPosition(List<double> gazeLocus, List<string> predictorCandidates)
        {
            var rank = new List<double>();
            return rank;
        }

        private List<double> RankByLanguageModel(List<string> predictorCandidates, string typedText)
        {
            var rank = new List<double>();
            return rank;
        }



    }
}
