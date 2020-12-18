using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MovieRecommender
{
    public class DataDescriptor
    {
        private readonly List<Movie> _Movies;
        private readonly List<User> _Users;

        public DataDescriptor()
        {
            _Movies = GetMovies();
            _Users = GetUsers();
        }

        public string GetMovieTitleByIndex(float index)
        {
            var movie = _Movies.Where(m => m.Index == index).FirstOrDefault();
            return movie.Title;
        }

        public (string firstName, string lastName) GetUserNamesByIndex(float index)
        {
            var user = _Users.Where(u => u.Index == index).FirstOrDefault();
            return (user.FirstName, user.LastName);
        }

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

    public class Movie
    {
        public float Index { get; set; }
        public string Title { get; set; }
    }

    public class User
    {
        public float Index { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
