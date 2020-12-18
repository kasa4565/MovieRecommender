// Authors: Sebastian Bobrowski (s17603), Katarzyna Czerwińska (s17098)
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MovieRecommender
{
    /// <summary>
    /// Class used to describe or identify the data
    /// </summary>
    public class DataDescriptor
    {
        /// <summary>
        /// List variables holding all users and videos loaded from csv files
        /// </summary>
        private readonly List<Movie> _Movies;
        private readonly List<User> _Users;
        private readonly List<Rating> _Ratings;

        /// <summary>
        /// Constructor of DataDescriptor class
        /// </summary>
        public DataDescriptor()
        {
            _Movies = GetMovies();
            _Users = GetUsers();
            _Ratings = GetRatings();
        }

        /// <summary>
        /// variable holding the amount of movies
        /// </summary>
        /// <returns>_Movies.Count</returns>
        public int GetMoviesCount() => _Movies.Count;

        /// <summary>
        /// method returning the name of the movie based on the id
        /// </summary>
        /// <param name="index"></param>
        /// <returns>movie.Title</returns>
        public List<float> GetMoviesWatchedByUser(float userId)
        {
            var ratingsForUser = _Ratings.Where(m => m.userId == userId).ToList();
            var moviesWatchedByuser = new List<float>();
           
            foreach(var rating in ratingsForUser)
            {
                moviesWatchedByuser.Add(rating.movieId);
            }

            return moviesWatchedByuser;
        }

        public string GetMovieTitleByIndex(float index)
        {
            var movie = _Movies.Where(m => m.Index == index).FirstOrDefault();
            return movie.Title;
        }

        /// <summary>
        /// method returning the name and surname of the user based on its id
        /// </summary>
        /// <param name="index"></param>
        /// <returns>user.FirstName, user.LastName</returns>
        public (string firstName, string lastName) GetUserNamesByIndex(float index)
        {
            var user = _Users.Where(u => u.Index == index).FirstOrDefault();
            return (user.FirstName, user.LastName);
        }

<<<<<<< HEAD
        /// <summary>
        /// Read movies from file and return List-type data
        /// </summary>
        /// <returns>records</returns>
=======
        private List<Rating> GetRatings()
        {
            var projectPath = Environment.CurrentDirectory;

            for (int i = 0; i < 3; i++)
                projectPath = Directory.GetParent(projectPath).ToString();


            var ratingsDataPath = Path.Combine(projectPath, "Data", "recommendation-ratings-train.csv");
            List<Rating> records = null;

            using (var reader = new StreamReader(ratingsDataPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = csv.GetRecords<Rating>().ToList();
            }

            return records;
        }

>>>>>>> origin/DisplayImprovement
        private List<Movie> GetMovies()
        {
            var projectPath = Environment.CurrentDirectory;

            for (int i = 0; i < 3; i++)
                projectPath = Directory.GetParent(projectPath).ToString();


            var moviesDataPath = Path.Combine(projectPath, "Data", "movies.csv");
            List<Movie> records = null;

            using (var reader = new StreamReader(moviesDataPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = csv.GetRecords<Movie>().ToList();
            }

            return records;
        }

        /// <summary>
        /// Read users from file and return List-type data
        /// </summary>
        /// <returns>records</returns>
        private List<User> GetUsers()
        {
            var projectPath = Environment.CurrentDirectory;

            for (int i = 0; i < 3; i++)
                projectPath = Directory.GetParent(projectPath).ToString();


            var usersDataPath = Path.Combine(projectPath, "Data", "users.csv");
            List<User> records = null;

            using (var reader = new StreamReader(usersDataPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = csv.GetRecords<User>().ToList();
            }

            return records;
        }
    }

    /// <summary>
    /// movie entity schema
    /// </summary>
    public class Movie
    {
        public float Index { get; set; }
        public string Title { get; set; }
    }

    /// <summary>
    /// user entity schema
    /// </summary>
    public class User
    {
        public float Index { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
<<<<<<< HEAD
}
=======

    public class Rating
    {
        public float userId { get; set; }
        public float movieId { get; set; }
        public float rating { get; set; }
        public float timestamp { get; set; }
    }
}
>>>>>>> origin/DisplayImprovement
