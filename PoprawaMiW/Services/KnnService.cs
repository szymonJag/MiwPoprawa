using PoprawaMiW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoprawaMiW.Services
{
    public class KnnService : IKnnService
    {
        public delegate double MetricDelegate(NumericalFileData sampleTest, NumericalFileData sampleDataset);

        //classifying decision by taking K nearest distances
        public string FirstDecisionClassifier(List<NumericalFileData> dataSet, NumericalFileData sampleTest, int k, MetricDelegate metric)
        {
            var distances = CalculateDistances(dataSet, sampleTest, metric);
            var kNearestDistances = distances.Take(k).ToDictionary(x => x.Key, x => x.Value);
            var countedDecisions = kNearestDistances.GroupBy(x => x.Key.Decision).ToDictionary(x => x.Key, x => x.Count());
            var givenDecision = countedDecisions.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value).ElementAt(0).Key;

            return givenDecision;
        }
        //classifying decision by dividing decisions into group and returning decison by calculating sum of K nearest decision
        public string SecondDecisionClassifier(List<NumericalFileData> dataSet, NumericalFileData sampleTest, int k, MetricDelegate metric)
        {
            var decisions = GetAllDecisions(dataSet);
            var decisionDistances = GetDecisionDistances(dataSet, sampleTest, metric);
            var sumOfKNearestDistances = GetSumOfKNearestDistancesForDecisions(decisionDistances, k);
            var decision = sumOfKNearestDistances.ElementAt(0).Key;

            return decision;
        }
        private Dictionary<string, double> GetSumOfKNearestDistancesForDecisions(List<DecisonWithDistances> decisionWithDistances, int k)
        {
            var sumOfKNearestDistances = new Dictionary<string, double>();

            foreach (var decision in decisionWithDistances)
            {
                double sum = 0;

                for (int i = 0; i < k - 1; i++)
                {
                    sum += decision.Distances.ElementAt(i);
                }
                sumOfKNearestDistances.Add(decision.Decision, sum);
            }
            return sumOfKNearestDistances.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }
        private List<DecisonWithDistances> GetDecisionDistances(List<NumericalFileData> dataSet, NumericalFileData sampleTest, MetricDelegate metric)
        {
            var listOfDecisionWithDistances = new List<DecisonWithDistances>();
            var decisions = GetAllDecisions(dataSet);

            foreach (var decision in decisions)
            {
                var distances = CalculateDistancesForGivenDecision(dataSet, sampleTest, metric, decision);
                var decisionWithDistances = new DecisonWithDistances(decision, distances);
                listOfDecisionWithDistances.Add(decisionWithDistances);
            }

            return listOfDecisionWithDistances;
        }
        private List<double> CalculateDistancesForGivenDecision(List<NumericalFileData> dataSet, NumericalFileData sampleTest, MetricDelegate metric, string decision)
        {
            var distances = new List<double>();
            var dataSetsWithGivenDecision = dataSet.Where(x => x.Decision == decision);

            foreach (var data in dataSetsWithGivenDecision)
            {
                var distance = metric(sampleTest, data);
                distances.Add(distance);
            }

            return distances.OrderBy(x => x).ToList();
        }
        private List<string> GetAllDecisions(List<NumericalFileData> dataSet)
        {
            return dataSet.Select(x => x.Decision).Distinct().ToList();
        }
        private Dictionary<NumericalFileData, double> CalculateDistances(List<NumericalFileData> dataSet, NumericalFileData sampleTest, MetricDelegate metric)
        {
            var distances = new Dictionary<NumericalFileData, double>();

            foreach (var sample in dataSet)
            {
                var distance = metric(sampleTest, sample);
                distances.Add(sample, distance);
            }

            var sortedDistances = distances.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            return sortedDistances;
        }
        private double CalculateDistanceUsingManhattanMetric(NumericalFileData sampleTest, NumericalFileData sampleDataset)
        {
            var numberOfValues = sampleTest.NumericalValues.Count();
            var sampleTestValues = sampleTest.NumericalValues;
            var sampleDataestValues = sampleDataset.NumericalValues;
            double distance = 0;

            for (int i = 0; i < numberOfValues - 1; i++)
            {
                var substract = sampleTestValues[i] - sampleDataestValues[i];
                distance += Math.Abs(substract);
            }

            return Math.Round(distance, 3);
        }
        private double CalculateDistanceUsingEuclidesMetric(NumericalFileData sampleTest, NumericalFileData sampleDataset)
        {
            var numberOfValues = sampleTest.NumericalValues.Count();
            var sampleTestValues = sampleTest.NumericalValues;
            var sampleDataestValues = sampleDataset.NumericalValues;
            double distance = 0;

            for (int i = 0; i < numberOfValues - 1; i++)
            {
                var substract = sampleTestValues[i] - sampleDataestValues[i];
                var squaredSubstraction = Math.Pow(substract, 2);
                distance += squaredSubstraction;
            }
            return Math.Round(Math.Sqrt(distance), 3);
        }
        private double CalculateDistanceUsingCzebyszewMetric(NumericalFileData sampleTest, NumericalFileData sampleDataset)
        {
            var numberOfValues = sampleTest.NumericalValues.Count();
            var sampleTestValuesMax = sampleTest.NumericalValues.Select(x => x.Value).Max();
            var sampleDataestValuesMax = sampleDataset.NumericalValues.Select(x => x.Value).Max();

            double distance = Math.Abs(sampleTestValuesMax - sampleDataestValuesMax);

            return Math.Round(distance, 3);
        }
        private double CalculateDistanceUsingMinkowskiMetric(NumericalFileData sampleTest, NumericalFileData sampleDataset, double p)
        {
            var numberOfValues = sampleTest.NumericalValues.Count();
            var sampleTestValues = sampleTest.NumericalValues;
            var sampleDataestValues = sampleDataset.NumericalValues;
            double distance = 0;

            for (int i = 0; i < numberOfValues; i++)
            {
                var substract = sampleTestValues[i] - sampleDataestValues[i];
                var absoluteSubstraction = Math.Abs(substract);
                distance += Math.Pow(absoluteSubstraction, p);
            }

            var pow = 1 / p;

            return Math.Round(Math.Pow(distance, pow), 3);
        }
        private double CalculateDistanceUsignLogarithmMetric(NumericalFileData sampleTest, NumericalFileData sampleDataset)
        {
            var numberOfValues = sampleTest.NumericalValues.Count();
            var sampleTestValues = sampleTest.NumericalValues;
            var sampleDatasetValues = sampleDataset.NumericalValues;
            double distance = 0;

            for (int i = 0; i < numberOfValues; i++)
            {
                var sampleValue = Math.Log(sampleTestValues[i]);
                var sampleDatasetValue = Math.Log(sampleDatasetValues[i]);
                var substract = sampleValue - sampleDatasetValue;

                distance += Math.Abs(substract);
            }

            return Math.Round(distance, 3);
        }
        //K to liczba wszystkich probek wzorcowych
        public int FindSmallestKForFirstWay(List<NumericalFileData> fileDatas)
        {
            var decisions = fileDatas.Select(x => x.Decision).ToList().Count;

            return decisions;
        }

        //K to liczebnosc najmniejszej klasy decyzyjnej
        public int FindSmallestKForSecondWay(List<NumericalFileData> fileDatas)
        {
            var decisions = fileDatas.Select(x => x.Decision);
            var countedDecisions = decisions.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            var smallestK = countedDecisions.Select(x => x.Value).Min();

            return smallestK;
        }
    }
}
