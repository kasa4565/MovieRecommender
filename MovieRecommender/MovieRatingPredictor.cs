// Authors: Sebastian Bobrowski (s17603), Katarzyna Czerwińska (s17098)
using Microsoft.ML;
using Microsoft.ML.Trainers;
using System;
using System.IO;
using System.Linq;

namespace MovieRecommender
{
    public class MovieRatingPredictor
    {
        /// <summary>
        /// Instances of classes that create the context of ML operations and model processing
        /// </summary>
        private readonly MLContext _MlContext;
        private ITransformer _Model;

        /// <summary>
        /// MovieRatingPredictor constructor
        /// </summary>
        public MovieRatingPredictor()
        {
            _MlContext = new MLContext();
            PrepareModel();
        }

        /// <summary>
        /// Make prediction with the built model
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="movieId"></param>
        /// <returns>movieRatingPrediction</returns>
        public MovieRatingPrediction UseModelForSinglePrediction(float userId, float movieId)
        {
            var predictionEngine = _MlContext.Model.CreatePredictionEngine<MovieRating, MovieRatingPrediction>(_Model);
            var testInput = new MovieRating { userId = userId, movieId = movieId };

            var movieRatingPrediction = predictionEngine.Predict(testInput);

            return movieRatingPrediction;
        }
        
        /// <summary>
        /// Call members to build and evaluate model
        /// </summary>
        private void PrepareModel()
        {
            (IDataView trainingDataView, IDataView testDataView) = LoadData();
            _Model = BuildAndTrainModel(trainingDataView);
            EvaluateModel(testDataView);
        }

        /// <summary>
        /// Load the training data set and format it according to the model
        /// </summary>
        /// <returns>trainingDataView, testDataView</returns>
        private (IDataView training, IDataView test) LoadData()
        {
            var trainingDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "recommendation-ratings-train.csv");
            var testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "recommendation-ratings-test.csv");

            IDataView trainingDataView = _MlContext.Data.LoadFromTextFile<MovieRating>(trainingDataPath, hasHeader: true, separatorChar: ',');
            IDataView testDataView = _MlContext.Data.LoadFromTextFile<MovieRating>(testDataPath, hasHeader: true, separatorChar: ',');

            return (trainingDataView, testDataView);
        }

        /// <summary>
        /// Train the model
        /// </summary>
        /// <param name="trainingDataView"></param>
        /// <returns>model</returns>
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
        
        /// <summary>
        /// Evaluate the model performance
        /// </summary>
        /// <param name="testDataView"></param>
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
