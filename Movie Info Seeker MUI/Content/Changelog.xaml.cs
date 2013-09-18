using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

namespace Movie_Info_Seeker.Content
{
    /// <summary>
    /// Interaction logic for Changelog.xaml
    /// </summary>
    public partial class Changelog : UserControl
    {
        public Changelog()
        {
            InitializeComponent();
            String dir = AppDomain.CurrentDomain.BaseDirectory;
            String x = null;
            try
            {
                using (StreamReader sr = new StreamReader(dir+"Resources/CHANGELOG.txt"))
                {
                    x = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                x = ("Error Message: " + e.Message);
            }
            tbChangelog.Text = x;
        }
    }
}
