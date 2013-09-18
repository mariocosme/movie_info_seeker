
namespace Movie_Info_Seeker
{
    class Movie
    {
        private string title, description, genre, cover, trailer, duration, path;
        /// <summary>
        /// Movie constructor with all params
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="description">description</param>
        /// <param name="genre">genre</param>
        /// <param name="cover">cover</param>
        /// <param name="trailer">trailer</param>
        /// <param name="duration">duration</param>
        /// <param name="path">hash</param>
        public Movie(string title, string description, string genre, string cover, string trailer, string duration, string path)
        {
            this.title = title;
            this.description = description;
            this.genre = genre;
            this.cover = cover;
            this.trailer = trailer;
            this.duration = duration;
            this.path = path;
        }

        public Movie()
        {
            // TODO: Complete member initialization
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Genre
        {
            get { return genre; }
            set { genre = value; }
        }

        public string Cover
        {
            get { return cover; }
            set { cover = value; }
        }

        public string Trailer
        {
            get { return trailer; }
            set { trailer = value; }
        }

        public string Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public string Path
        {
            get { return path; }
            set { path = value; }
        }
    }
}
