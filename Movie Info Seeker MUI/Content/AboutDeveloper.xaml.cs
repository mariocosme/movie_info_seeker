using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
    /// Interaction logic for AboutDeveloper.xaml
    /// </summary>
    public partial class AboutDeveloper : UserControl
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        Boolean canSend = true;
        public AboutDeveloper()
        {
            InitializeComponent();
        }
        private void Hyperlink_RequestNavigatelinkedIn(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
        private void SendEmail()
        {
            if(!canSend)
            {
                makeDialog("Cannot send another e-mail so soon.");
            }
            else if(tbSenderEmail.Text == "")
            {
                makeDialog("Your e-mail is missing.");
            }
            else if(tbSubjet.Text == "")
            {
                MessageBoxButton btn = MessageBoxButton.YesNo;
                if (ModernDialog.ShowMessage("Send e-mail without subject?", "Information", btn) == MessageBoxResult.Yes)
                {
                    tbSubjet.Text = "No Subject";
                }
            }
            else if (tbEmailText.Text == "")
            {
                makeDialog("Please write something to send.");
            }
            else
            {
                MailMessage email = new MailMessage(tbSenderEmail.Text, "mariocosme@gmail.com", tbSubjet.Text, tbEmailText.Text);
                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential("movieinfoseeker@gmail.com", "indiaPorto43");
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;

                try
                {
                    client.Send(email);
                        dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                        dispatcherTimer.Interval = new TimeSpan(0,0,30);
                        dispatcherTimer.Start();
                        canSend = false;
                }
                catch (Exception ex)
                {
                    ModernDialog md = new ModernDialog();
                    md.Title = "Information";
                    md.Content = ex.Message.ToString();
                    md.Buttons = new Button[] { md.OkButton };
                    md.Show();
                }
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            canSend = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendEmail();
        }

        private void makeDialog(string parameter)
        {
            ModernDialog md = new ModernDialog();
            md.Title = "Information";
            md.Content = parameter;
            md.Buttons = new Button[] { md.OkButton };
            md.Show();
        }
    }
}
