using Microsoft.ML;
using Microsoft.ML.Trainers;
using System;
using System.IO;
using System.Linq;

namespace MovieRecommender
{
    public class MovieRatingPredictor
    {
        private readonly MLContext _MlContext;
        private ITransformer _Model;

        public MovieRatingPredictor()
        {
            _MlContext = new MLContext();
            PrepareModel();
        }

        public MovieRatingPrediction UseModelForSinglePrediction(float userId, float movieId)
        {
            Console.WriteLine("=============== Making a prediction ===============");
            var predictionEngine = _MlContext.Model.CreatePredictionEngine<MovieRating, MovieRatingPrediction>(_Model);
            var testInput = new MovieRating { userId = userId, movieId = movieId };

            var movieRatingPrediction = predictionEngine.Predict(testInput);

            return movieRatingPrediction;
        }

        private void PrepareModel()
        {
            (IDataView trainingDataView, IDataView testDataView) = LoadData();
            _Model = BuildAndTrainModel(trainingDataView);
            EvaluateModel(testDataView);
        }

        private (IDataView training, IDataView test) LoadData()
        {
            var trainingDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "recommendation-ratings-train.csv");
            var testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "recommendation-ratings-test.csv");

            IDataView trainingDataView = _MlContext.Data.LoadFromTextFile<MovieRating>(trainingDataPath, hasHeader: true, separatorChar: ',');
            IDataView testDataView = _MlContext.Data.LoadFromTextFile<MovieRating>(testDataPath, hasHeader: true, separatorChar: ',');

            return (trainingDataView, testDataView);
        }

        private ITransformer BuildAndTrainModel(IDataView trainingDataView)
        {
            IEstimator<ITransformer> estimator = _MlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "userIdEncoded", inputColumnName: "userId")
                .Append(_MlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "movieIdEncoded", inputColumnName: "movieId"));
            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "userIdEncoded",
                MatrixRowIndexColumnName = "movieIdEncoded",
                LabelColumnName = "Label",
                NumberOfIterations = 20,
                ApproximationRank = 100
            };

            var trainerEstimator = estimator.Append(_MlContext.Recommendation().Trainers.MatrixFactorization(options));

            Console.WriteLine("=============== Training the model ===============");
            ITransformer model = trainerEstimator.Fit(trainingDataView);

            return model;
        }

        private void EvaluateModel(IDataView testDataView)
        {
            Console.WriteLine("=============== Evaluating the model ===============");
            var prediction = _Model.Transform(testDataView);
            var metrics = _MlContext.Regression.Evaluate(prediction, labelColumnName: "Label", scoreColumnName: "Score");
            Console.WriteLine("Root Mean Squared Error : " + metrics.RootMeanSquaredError.ToString());
            Console.WriteLine("RSquared: " + metrics.RSquared.ToString());
        }
    }
}
