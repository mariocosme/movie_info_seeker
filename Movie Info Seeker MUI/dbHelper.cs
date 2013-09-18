using System;
using System.Data;
using System.Data.SqlServerCe;

namespace Movie_Info_Seeker
{
    class dbHelper
    { // string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MyApp";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="movie"></param>
        public static void InsertMovie(Movie movie)
        {
            SqlCeConnection c = new SqlCeConnection("Data Source=" + Movie_Info_Seeker.Properties.Settings.Default.appDir + "Database1.sdf");
            c.Open();

            SqlCeCommand sqlComm = new SqlCeCommand("INSERT INTO movie (title, description, genre, cover, trailer, duration, path) VALUES (@title, @description, @genre, @cover, @trailer, @duration, @path)", c);
            sqlComm.Parameters.Add("@title", SqlDbType.NVarChar);
            sqlComm.Parameters["@title"].Value = movie.Title;
            sqlComm.Parameters.Add("@description", SqlDbType.NVarChar);
            sqlComm.Parameters["@description"].Value = movie.Description;
            sqlComm.Parameters.Add("@genre", SqlDbType.NVarChar);
            sqlComm.Parameters["@genre"].Value = movie.Genre;
            sqlComm.Parameters.Add("@cover", SqlDbType.NVarChar);
            sqlComm.Parameters["@cover"].Value = movie.Cover;
            sqlComm.Parameters.Add("@trailer", SqlDbType.NVarChar);
            sqlComm.Parameters["@trailer"].Value = movie.Trailer;
            sqlComm.Parameters.Add("@duration", SqlDbType.NVarChar);
            sqlComm.Parameters["@duration"].Value = movie.Duration;
            sqlComm.Parameters.Add("@path", SqlDbType.NVarChar);
            sqlComm.Parameters["@path"].Value = movie.Path;
            sqlComm.ExecuteNonQuery();
            c.Close();
        }
        /// <summary>
        /// Searches for a movie on the database by his title and returns a Movie object
        /// </summary>
        public static Movie GetMovieByTitle(String title)
        {
            Movie y = new Movie();
            SqlCeConnection c = new SqlCeConnection("Data Source=" + Movie_Info_Seeker.Properties.Settings.Default.appDir + "Database1.sdf");
            c.Open();
            SqlCeCommand cmd = new SqlCeCommand("SELECT * FROM movie WHERE title= @title", c);
            cmd.Parameters.Add("@title", SqlDbType.NVarChar);
            cmd.Parameters["@title"].Value = title;
            SqlCeDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                y.Title = rdr[1].ToString();
                y.Description = rdr[2].ToString();
                y.Genre= rdr[3].ToString();
                y.Cover = rdr[4].ToString();
                y.Trailer = rdr[5].ToString();
                y.Duration = rdr[6].ToString();
                y.Path = rdr[7].ToString();
            }
            c.Close();
            return y;
        }
        /// <summary>
        /// Searches for a movie on the database by his file hash and returns a Movie object
        /// </summary>
        public static Movie GetMovieByPath(String path)
        {
            Movie y = new Movie();
            SqlCeConnection c = new SqlCeConnection("Data Source=" + Movie_Info_Seeker.Properties.Settings.Default.appDir + "Database1.sdf");
            c.Open();
            SqlCeCommand cmd = new SqlCeCommand("SELECT * FROM movie WHERE path= @path", c);
            cmd.Parameters.Add("@path", SqlDbType.NVarChar);
            cmd.Parameters["@path"].Value = path;
            SqlCeDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                y.Title = rdr[1].ToString();
                y.Description = rdr[2].ToString();
                y.Genre = rdr[3].ToString();
                y.Cover = rdr[4].ToString();
                y.Trailer = rdr[5].ToString();
                y.Duration = rdr[6].ToString();
                y.Path = rdr[7].ToString();
            }
            c.Close();
            return y;
        }
        /// <summary>
        /// Deletes a movie from the database based on title and duration
        /// </summary>
        /// <param name="movie">Movie to search and delete</param>
        public static void DeleteMovie(Movie movie)
        {
            SqlCeConnection c = new SqlCeConnection("Data Source=" + Movie_Info_Seeker.Properties.Settings.Default.appDir + "Database1.sdf");
            c.Open();
            SqlCeCommand cmd = new SqlCeCommand("DELETE FROM movie WHERE title = @title AND duration = @duration", c);
            cmd.Parameters.Add("@title", SqlDbType.NVarChar);
            cmd.Parameters["@title"].Value = movie.Title;
            cmd.Parameters.Add("@duration", SqlDbType.NVarChar);
            cmd.Parameters["@duration"].Value = movie.Duration;
            cmd.ExecuteNonQuery();
            c.Close();
        }

        internal static Boolean CheckMovieExists(string path)
        {
            SqlCeConnection c = new SqlCeConnection("Data Source=" + Movie_Info_Seeker.Properties.Settings.Default.appDir + "Database1.sdf");
            c.Open();
            SqlCeCommand cmd = new SqlCeCommand("SELECT * FROM movie WHERE path= @path", c);
            cmd.Parameters.Add("@path", SqlDbType.NVarChar);
            cmd.Parameters["@path"].Value = path;
            SqlCeDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr[7].ToString() == path)
                {
                    c.Close();
                    return true;
                }
            }
            c.Close();
            return false;
        }
    }
}
