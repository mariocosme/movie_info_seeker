using FirstFloor.ModernUI.Windows.Controls;
using Movie_Info_Seeker;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Movie_Info_Seeker_MUI.Pages
{
    /// <summary>
    /// Interaction logic for BasicPage1.xaml
    /// </summary>
    public partial class MoviePage : UserControl
    {
        Movie x = new Movie();
        public MoviePage()
        {
            InitializeComponent();
            checkForAppDir();
            checkForDb();
            getDefaultFolder();
            populateListBox();
        }
        private void checkForDb()
        {
            string conString = "Data Source=" + Movie_Info_Seeker.Properties.Settings.Default.appDir + "Database1.sdf";
            if (!File.Exists(Movie_Info_Seeker.Properties.Settings.Default.appDir + "Database1.sdf"))
            {
                SqlCeEngine en = new SqlCeEngine(conString);
                en.CreateDatabase();
                SqlCeConnection con = new SqlCeConnection(conString);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string sql = "CREATE TABLE movie ("
                + "id INT NOT NULL IDENTITY, "
                + "title NVARCHAR(1000) NULL, "
                + "description NVARCHAR(4000) NULL, "
                + "genre NVARCHAR(1000) NULL, "
                + "cover NVARCHAR(1000) NULL, "
                + "trailer NVARCHAR(1000) NULL, "
                + "duration NVARCHAR(1000) NULL, "
                + "path NVARCHAR(2000) NULL, "
                + "CONSTRAINT PK_movie PRIMARY KEY (id) )";
                SqlCeCommand cmd = new SqlCeCommand(sql, con);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlCeException sqlexception)
                {
                    makeInfoDialog(sqlexception.Message);
                }
                catch (Exception ex)
                {
                    makeInfoDialog(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        /// <summary>
        /// Shows an information dialog with an Ok button.
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="text">Dialog text</param>
        private void makeInfoDialog(string text)
        {
            ModernDialog md = new ModernDialog();
            md.Title = "Information";
            md.Content = text;
            md.Buttons = new Button[] { md.OkButton };
            md.Show();
        }

        private void checkForAppDir()
        {
            string a = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Movie Info Seeker\\";
            string b = a + "Posters";
            if (!Directory.Exists(a))
            {
                Movie_Info_Seeker.Properties.Settings.Default.appDir = a;
                Movie_Info_Seeker.Properties.Settings.Default.Save();
                Directory.CreateDirectory(b);
            }
            else
            {
                Movie_Info_Seeker.Properties.Settings.Default.appDir = a;
                Movie_Info_Seeker.Properties.Settings.Default.Save();
            }
        }

        private Boolean checkForInternetConnection()
        {
            try
            {
                Uri myUri = new Uri("http://www.google.com/");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(myUri);
                request.Timeout = 5000;
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                { return true; }
                else { return false; }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void getDefaultFolder()
        {
            string x = Movie_Info_Seeker.Properties.Settings.Default.selectedFolder;
            if (x != "")
            {
                tbFolderPath.Text = x;
            }
            else
            {
                tbFolderPath.Text = "";
            }
        }

        private void populateListBox()
        {
            List<String> filepath = new List<string>();
            string x = Movie_Info_Seeker.Properties.Settings.Default.selectedFolder;
            if (x != "")
            {
                foreach (string d in Directory.GetDirectories(x))
                {
                    foreach (string f in Directory.GetFiles(d))
                    {
                        if (f.ToString().Contains("sample") || f.ToString().Contains("Sample"))
                        {
                            Console.WriteLine("Sample");
                        }
                        else if (f.ToString().EndsWith(".mp4") || f.ToString().EndsWith(".mkv") || f.ToString().EndsWith(".avi") || f.ToString().EndsWith(".m4v"))
                        {
                            filepath.Add(f.ToString());
                        }
                    }
                }
                List<String> filename = new List<string>();
                foreach (string file in filepath)
                {
                    filename.Add(System.IO.Path.GetFileName(file));
                }
                foreach (string y in filename)
                {
                    listBox.Items.Add(y.ToString());
                }
            }
        }

        static string UppercaseWords(string value)
        {
            char[] array = value.ToCharArray();
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.HasItems)
            {
                string p = Movie_Info_Seeker.Properties.Settings.Default.selectedFolder + listBox.SelectedItem.ToString();
                if (dbHelper.CheckMovieExists(p))
                {
                    x = dbHelper.GetMovieByPath(p);
                    tbDescription.Text = x.Description;
                    tbTitle.Text = x.Title;
                    tbDuration.Text = x.Duration;
                    iPoster.Source = new BitmapImage(new Uri(x.Cover));
                    tbMovieNameArranjado.Text = x.Title;
                    tbGenre.Text = x.Genre;
                }
                else
                {
                    string curItem = listBox.SelectedItem.ToString();
                    tbMovieNameArranjado.Text = UppercaseWords(curItem.Replace('.', ' '));
                    string myString = tbMovieNameArranjado.Text;
                    string alphabet = String.Empty;
                    string digit = String.Empty;
                    Match regexMatch = Regex.Match(myString, "\\d");
                    if (regexMatch.Success)
                    {
                        int digitStartIndex = regexMatch.Index;
                        alphabet = myString.Substring(0, digitStartIndex);
                        tbMovieNameArranjado.Text = alphabet;
                    }
                    string dialogText = "Movie " + alphabet + " not available. Please download it.";
                    makeInfoDialog(dialogText);
                }
            }
        }

        private void bLoadDir_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog fbd = new VistaFolderBrowserDialog();
            fbd.ShowDialog();
            listBox.Items.Clear();
            Movie_Info_Seeker.Properties.Settings.Default.selectedFolder = fbd.SelectedPath;
            Movie_Info_Seeker.Properties.Settings.Default.Save();
            if (Movie_Info_Seeker.Properties.Settings.Default.selectedFolder == "")
            {
                string dialogText = "No movies were found. Please select folder again.";
                makeInfoDialog(dialogText);
                Movie_Info_Seeker.Properties.Settings.Default.selectedFolder = "";
                Movie_Info_Seeker.Properties.Settings.Default.Save();
            }
            else
            {
                populateListBox();
                getDefaultFolder();
            }
        }

        private void bTeste_Click(object sender, RoutedEventArgs e)
        {
            if (tbMovieNameArranjado.Text == "")
            {
                makeInfoDialog("Nothing to search. Please write movie title or choose movie from above.");
            }
            else if (tbMovieNameArranjado.Text != "" && checkForInternetConnection() == true)
            {
                x = Tmbd.GetMovieByTitle(tbMovieNameArranjado.Text, Movie_Info_Seeker.Properties.Settings.Default.selectedFolder + listBox.SelectedItem.ToString());
                if (x != null)
                {
                    tbDescription.Text = x.Description;
                    tbTitle.Text = x.Title;
                    tbDuration.Text = x.Duration;
                    iPoster.Source = new BitmapImage(new Uri(x.Cover));
                    tbGenre.Text = x.Genre;
                }
                else
                {
                    string dialogText = "No movies were found with: " + tbMovieNameArranjado.Text;
                    makeInfoDialog(dialogText);
                }
            }
            else if (checkForInternetConnection() == false)
            {
                string dialogText = "No internet connection available. Please check you connection and retry.";
                makeInfoDialog(dialogText);
            }
        }

        private void bTrailer_Click(object sender, RoutedEventArgs e)
        {
            if (tbMovieNameArranjado.Text != "")
            {
                Process.Start("http://www.youtube.com/results?search_query=" + tbMovieNameArranjado.Text + " trailer");
            }
        }
    }
}