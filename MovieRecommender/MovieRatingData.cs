// Authors: Sebastian Bobrowski (s17603), Katarzyna Czerwińska (s17098)
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML.Data;

namespace MovieRecommender
{
    /// <summary>
    /// Specifies an input data
    /// </summary>
    public class MovieRating
    {
        [LoadColumn(0)]
        public float userId;
        [LoadColumn(1)]
        public float movieId;
        [LoadColumn(2)]
        public float Label;
    }
    
    /// <summary>
    /// Represents predicted results
    /// </summary>
    public class MovieRatingPrediction
    {
        public float Label;
        public float Score;
    }
    /// <summary>
    /// Specifies ratings
    /// </summary>
    public class MovieRatingData
    {
        public float UserId { get; set; }
        public float MovieId { get; set; }
        public float Score { get; set; }
    }
}
