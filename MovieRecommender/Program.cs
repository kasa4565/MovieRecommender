using System;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace MovieRecommender
{
    class Program
    {
        static void Main(string[] args)
        {
            MLContext mlContext = new MLContext();
            (IDataView trainingDataView, IDataView testDataView) = LoadData(mlContext);
        }

        public static (IDataView training, IDataView test) LoadData(MLContext mlContext)
        {
            var trainingDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "recommendation-ratings-train.csv");
            var testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "recommendation-ratings-test.csv");

            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<MovieRating>(trainingDataPath, hasHeader: true, separatorChar: ',');
            IDataView testDataView = mlContext.Data.LoadFromTextFile<MovieRating>(testDataPath, hasHeader: true, separatorChar: ',');

            return (trainingDataView, testDataView);
        }
    }
}
