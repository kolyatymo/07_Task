using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace _07_task
{
    /// <summary>
    /// Interaction logic for Window_User.xaml
    /// </summary>
    public partial class Window_User : Window
    {
        public string Login { get; set; }
        public string Password { get; set; }

        private List<MimeMessage> Messages = new List<MimeMessage>();
        public Window_User(string login, string password)
        {
            InitializeComponent();

            UserLogin.Text = login;
            Login = login;
            Password = password;
        }

        private void Button_Answer(object sender, RoutedEventArgs e)
        {
            if (ListMessage.SelectedIndex == -1)
            {
                MessageBox.Show("select Message");
                return;
            }

            if (AnswerText.Text == "")
            {
                MessageBox.Show("write text");
                return;
            }

            try
            {
                var original = Messages[ListMessage.SelectedIndex];

                MimeMessage message = new MimeMessage();

                message.From.Add(new MailboxAddress("Kolya", Login));
                message.To.AddRange(original.From);

                message.Subject = original.Subject;

                message.Body = new TextPart()
                {
                    Text = AnswerText.Text
                };
                MessageBox.Show("message sent!!!");

                AnswerText.Clear();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ListMessage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListMessage.SelectedIndex;

            var message = Messages[index];

            InfoUserMessage.Text =  $"from --> {message.From}\n" +
                                    $"time --> {message.Date}\n" +
                                    $"theme -->  {message.Subject}\n" +
                                    $"Message -->\n{message.TextBody}";

        }

        private async void Button_Download_gmail(object sender, RoutedEventArgs e)
        {
            try
            {
                ListMessage.Items.Clear();
                Messages.Clear();
                InfoUserMessage.Text = "";

                using (var client = new ImapClient())
                {
                    await client.ConnectAsync("imap.gmail.com", 993, MailKit.Security.SecureSocketOptions.SslOnConnect);

                    await client.AuthenticateAsync(Login, Password);

                    await client.Inbox.OpenAsync(FolderAccess.ReadOnly);

                    var inbox = await client.Inbox.SearchAsync(SearchQuery.All);

                    foreach (var item in inbox)
                    {
                        var res = await client.Inbox.GetMessageAsync(item);

                        Messages.Add(res);

                        ListMessage.Items.Add($"{res.Date} | {res.Subject}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            int index = ListMessage.SelectedIndex;

            if (index < 0 || index >= Messages.Count)
            {
                MessageBox.Show("Select Message");
                return;
            }

            var original = Messages[index];

            try
            {
                using (var client = new ImapClient())
                {
                    client.Connect("imap.gmail.com", 993, MailKit.Security.SecureSocketOptions.SslOnConnect);

                    client.Authenticate(Login, Password);

                    var folder = client.Inbox;
                    folder.Open(FolderAccess.ReadWrite);

                    var inbox = folder.Search(SearchQuery.All);
                    var id = inbox[index];

                    folder.MoveTo(id, client.GetFolder(MailKit.SpecialFolder.Trash));

                    client.Disconnect(true);
                }

                Messages.RemoveAt(index);
                ListMessage.Items.RemoveAt(index);
                InfoUserMessage.Text = "";

                MessageBox.Show("was Deleted");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }

        private void Button_Sort(object sender, RoutedEventArgs e)
        {
            Messages = Messages.OrderByDescending(x => x.Date).ToList();

            ListMessage.Items.Clear();

            foreach (var message in Messages)
            {
                ListMessage.Items.Add($"{message.Date} (--) {message.Subject}");
            }

            InfoUserMessage.Text = "";
        }

        private void Button_Filtr(object sender, RoutedEventArgs e)
        {
            try
            {
                ListMessage.Items.Clear();
                Messages.Clear();
                InfoUserMessage.Text = "";

                using (var client = new ImapClient())
                {
                    client.Connect("imap.gmail.com", 993, MailKit.Security.SecureSocketOptions.SslOnConnect);

                    client.Authenticate(Login, Password);

                    var folder = client.GetFolder(MailKit.SpecialFolder.Trash);

                    folder.Open(FolderAccess.ReadOnly);

                    var inbox = folder.Search(SearchQuery.All);

                    foreach (var item in inbox)
                    {
                        var res = folder.GetMessage(item);

                        Messages.Add(res);

                        ListMessage.Items.Add($"{res.Date} (--) {res.Subject}");
                    }

                    client.Disconnect(true);
                }

                MessageBox.Show("OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
    }
}