using OpenPop;
using OpenPop.Mime;
using OpenPop.Pop3;
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
using System.Data.SQLite;

namespace MailClient_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
            SQLiteConnection Con = new SQLiteConnection("Data Source=mailsDB.s3db;Version=3;");
            Con.Open();
            SQLiteCommand Cmd = new SQLiteCommand(Con);
            Cmd.CommandText = @"INSERT INTO EMails ";
            Con.Close();


        }
/*
        public DataTable GetDataTable(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                SQLiteConnection cnn = new SQLiteConnection(dbConnection);
                cnn.Open();
                SQLiteCommand mycommand = new SQLiteCommand(cnn);
                mycommand.CommandText = sql;
                SQLiteDataReader reader = mycommand.ExecuteReader();
                dt.Load(reader);
                reader.Close();
                cnn.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return dt;
        }
        */

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            if (Properties.Settings.Default.Username != "")
            {
                List<Message> MailList = FetchAllMessages(Properties.Settings.Default.Pop3Server, Properties.Settings.Default.Port, Properties.Settings.Default.SSL, Properties.Settings.Default.Username, Properties.Settings.Default.Password);

                foreach (Message MailItem in MailList)
                {
                    MailOverView.Items.Add(new Mail { Sender = MailItem.Headers.From.DisplayName, Subject = MailItem.Headers.Subject.ToString(), MessageID = MailItem.Headers.MessageId.ToString() });
                }
                MailOverView.SelectedIndex = 0;

            }
        }

        /// <summary>
        /// Example showing:
        ///  - how to find a html version in a Message
        ///  - how to save MessageParts to file
        /// </summary>
        /// <param name="message">The message to examine for html</param>
        public static void FindHtmlInMessage(Message message)
        {
            MessagePart html = message.FindFirstHtmlVersion();
            if (html != null)
            {
                // Save the plain text to a file, database or anything you like
                html.Save(new FileInfo("html.txt"));
            }
        }

        /// <summary>
        /// Example showing:
        ///  - how to fetch all messages from a POP3 server
        /// </summary>
        /// <param name="hostname">Hostname of the server. For example: pop3.live.com</param>
        /// <param name="port">Host port to connect to. Normally: 110 for plain POP3, 995 for SSL POP3</param>
        /// <param name="useSsl">Whether or not to use SSL to connect to server</param>
        /// <param name="username">Username of the user on the server</param>
        /// <param name="password">Password of the user on the server</param>
        /// <returns>All Messages on the POP3 server</returns>
        public List<Message> FetchAllMessages(string hostname, int port, bool useSsl, string username, string password)
        {
            // The client disconnects from the server when being disposed
            using (Pop3Client client = new Pop3Client())
            {
                // Connect to the server
                client.Connect(hostname, port, useSsl);

                // Authenticate ourselves towards the server
                client.Authenticate(username, password);

                // Get the number of messages in the inbox
                int messageCount = client.GetMessageCount();

                // We want to download all messages
                List<Message> allMessages = new List<Message>(messageCount);

                // Messages are numbered in the interval: [1, messageCount]
                // Ergo: message numbers are 1-based.
                // Most servers give the latest message the highest number
                for (int i = messageCount; i > 0; i--)
                {
                    //MailOverView.Items.Add(new Mail { Sender = a.Headers.From.DisplayName, Subject = foo.Headers.Subject.ToString(), MessageID = foo.Headers.MessageId.ToString() });
                    allMessages.Add(client.GetMessage(i));
                }

                // Now return the fetched messages
                return allMessages;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var childWindow = new UserSettings();
            childWindow.Show();
        }

        private void MailOverView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

    }
    public class Mail
    {
        public string Sender { get; set; }
        public string Subject { get; set; }
        public bool Selected { get; set; }
        public string MessageID { get; set; }
        
    }
}