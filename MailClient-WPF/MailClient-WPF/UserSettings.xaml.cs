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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MailClient_WPF
{
    /// <summary>
    /// Interaction logic for UserSettings.xaml
    /// </summary>
    public partial class UserSettings : Window
    {
        public UserSettings()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Password = txtPassword.Password;
            Properties.Settings.Default.Pop3Server = txtPop.Text;
            Properties.Settings.Default.Username = txtUsername.Text;
            //Properties.Settings.Default.Port = intPort.Value;
            Properties.Settings.Default.SSL = chkSSL.IsChecked.Value;
            Properties.Settings.Default.Save();

            MessageBox.Show("User settings have been saved.", "Saved.");
        }
    }
}
