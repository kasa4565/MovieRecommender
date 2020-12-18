// Authors: Sebastian Bobrowski (s17603), Katarzyna Czerwińska (s17098)
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace MovieRecommender
{
    class Program
    {
        private static DataDescriptor _DataDescriptor;
        private const float _UserId = 23;

        /// <summary>
        /// Start the program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            _DataDescriptor = new DataDescriptor();
            var movieRecommender = new MovieRecommender();
            var movieRecommendation = movieRecommender.GetMovieRecommendationForUser(_UserId);
            PrintResults(movieRecommendation);
        }

        /// <summary>
        /// Display prediction results to the console
        /// </summary>
        /// <param name="movieRecommendation"></param>
        private static void PrintResults((List<float> bestSevenMovies, List<float> worstSevenMovies) movieRecommendation)
        {
            Console.WriteLine("=============== Making a prediction ===============");
            var userNames = _DataDescriptor.GetUserNamesByIndex(_UserId);

            foreach (var id in movieRecommendation.bestSevenMovies)
            {
                var movieTitle = _DataDescriptor.GetMovieTitleByIndex(id);
                Console.WriteLine("Movie \"" + movieTitle + "\" is recommended for user " + userNames.firstName + " " + userNames.lastName);
            }

            foreach (var id in movieRecommendation.worstSevenMovies)
            {
                var movieTitle = _DataDescriptor.GetMovieTitleByIndex(id);
                Console.WriteLine("Movie \"" + movieTitle + "\" is not recommended for user " + userNames.firstName + " " + userNames.lastName);
            }
        }
    }
}
