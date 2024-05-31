using System.Windows;
using System.Windows.Controls;

namespace UserInterface
{
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBox(string message)
        {
            InitializeComponent();
            TextBlock? messageText = FindName("MessageTextBlock") as TextBlock;
            messageText.Text = message ?? null;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public static bool Show(string message)
        {
            CustomMessageBox msgBox = new CustomMessageBox(message);

            return msgBox.ShowDialog() ?? false;
        }
    }
}