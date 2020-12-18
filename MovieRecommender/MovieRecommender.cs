using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRecommender
{
    public class MovieRecommender
    {
        private readonly MovieRatingPredictor _MovieRatingPredictor;
        private readonly DataDescriptor _DataDescriptor;

        public MovieRecommender()
        {
            _MovieRatingPredictor = new MovieRatingPredictor();
            _DataDescriptor = new DataDescriptor();
        }

        public (List<float> bestSevenMovies, List<float> worstSevenMovies) GetMovieRecommendationForUser(float userIndex)
        {
            var bestSevenMoviesId = new List<float>();
            var worstSevenMoviesId = new List<float>();

            var moviesRatingList = GetMoviesRatingForUser(userIndex);
            moviesRatingList = moviesRatingList.OrderBy(l => l.Score).ToList();

            var bestSevenMovies = moviesRatingList.Take(7);
            foreach (var movie in bestSevenMovies)
                bestSevenMoviesId.Add(movie.MovieId);

            var worstSevenMovies = moviesRatingList.TakeLast(7);
            foreach (var movie in worstSevenMovies)
                worstSevenMoviesId.Add(movie.MovieId);

            return (bestSevenMoviesId, worstSevenMoviesId);
        }

        private List<MovieRatingData> GetMoviesRatingForUser(float userIndex)
        {
            var ratingPredictionList = new List<MovieRatingData>();

            for (int movieId = 0; movieId < _DataDescriptor.GetMoviesCount(); movieId++)
            {
                var prediction = _MovieRatingPredictor.UseModelForSinglePrediction(userIndex, movieId);
                var data = new MovieRatingData
                {
                    UserId = userIndex,
                    MovieId = movieId,
                    Score = prediction.Score
                };

                ratingPredictionList.Add(data);
            }

            return ratingPredictionList;
        }
    }
}
