using System;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Movie_Info_Seeker
{
    class Tmbd
    {
        public void OpenMoviePage(string id)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://api.themoviedb.org/3/search/movie?query=fight club&api_key=8e8a05c6a2073867606d3327274c3407");
            request.Method = "GET";
            request.Accept = "application/json";
            //request.Headers.Add("Accept", "application/json");
            request.ContentLength = 0;
            string responseContent;
            HttpWebResponse wr = (HttpWebResponse)request.GetResponse();

            using (var response = request.GetResponse() as System.Net.HttpWebResponse)
            {
                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    responseContent = reader.ReadToEnd();
                    //Console.WriteLine(responseContent);
                }
            }
            JObject o = JObject.Parse(responseContent);
            JArray sizes = (JArray)o["results"];
            Console.WriteLine(sizes[0]);
            string x = (string)sizes[0]["id"];
            Process.Start("http://www.themoviedb.org/movie/" + x);
        }

        public static Movie GetMovieByTitle(string title, string path)
        {
            string id = GetMovieById(title);
            if (id == null)
            {
                Movie x = null;
                return x;
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://api.themoviedb.org/3/movie/" + id + "?api_key=8e8a05c6a2073867606d3327274c3407");
            request.Method = "GET";
            request.Accept = "application/json";
            //request.Headers.Add("Accept", "application/json");
            request.ContentLength = 0;
            string responseContent;
            HttpWebResponse wr = (HttpWebResponse)request.GetResponse();

            using (var response = request.GetResponse() as System.Net.HttpWebResponse)
            {
                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    responseContent = reader.ReadToEnd();
                }
            }
            JObject o = JObject.Parse(responseContent);
            //titulo
            string titulo = (string)o["original_title"];
            //descrição
            string desc = (string)o["overview"];
            //generos
            JArray genre = (JArray)o["genres"];
            int genero = 0;
            StringBuilder sb = new StringBuilder();
            foreach (JObject x in genre)
            {
                genero++;
            }
            for (int i = 0; i < genero; i++)
            {
                sb.Append(genre[i]["name"] + " ");
            }
            //poster
            string poster = (string)"http://d3gtl9l2a4fn1j.cloudfront.net/t/p/w185" + o["poster_path"];
            string posterDisk = Movie_Info_Seeker.Properties.Settings.Default.appDir + "Posters\\" + (string)o["poster_path"];
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(poster, posterDisk);
            }
            poster = posterDisk;
            //duration
            string duration = (string)"Runtime: "+o["runtime"] + " Min";
            Movie m1 = new Movie(titulo, desc, sb.ToString(), poster, "TRAILER", duration, path);
            dbHelper.InsertMovie(m1);
            return m1;
        }

        public static string GetMovieById(string title)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://api.themoviedb.org/3/search/movie?query=" + title + "&api_key=8e8a05c6a2073867606d3327274c3407");
            request.Method = "GET";
            request.Accept = "application/json";
            //request.Headers.Add("Accept", "application/json");
            request.ContentLength = 0;
            string responseContent;
            HttpWebResponse wr = (HttpWebResponse)request.GetResponse();

            using (var response = request.GetResponse() as System.Net.HttpWebResponse)
            {
                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    responseContent = reader.ReadToEnd();
                }
            }
            JObject o = JObject.Parse(responseContent);
            JArray array = (JArray)o["results"];
            if (array.Count == 0)
            {
                return null;
            }
            return array[0]["id"].ToString();
        }
    }
}
