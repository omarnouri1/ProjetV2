using ProjetV2.View;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ProjetV2
{
    public partial class LanguageSwitchWindow : Window
    {
        public LanguageSwitchWindow()
        {
            InitializeComponent();
        }

        private void ButtonLangFr_Click(object sender, RoutedEventArgs e)
        {
            // Changer le texte en français
            WelcomeText.Text = "Bienvenue dans notre sauvegarde";
            // Changer le texte des boutons en français
            ButtonSave.Content = "Sauvegarder";
            ButtonExit.Content = "Quitter";
        }

        private void ButtonLangEn_Click(object sender, RoutedEventArgs e)
        {
            // Changer le texte en anglais
            WelcomeText.Text = "Welcome to our backup";
            // Changer le texte des boutons en anglais
            ButtonSave.Content = "Save";
            ButtonExit.Content = "Quit";
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            // Fermer l'application
            Application.Current.Shutdown();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            // Rediriger vers la nouvelle page (remplacez "SavePage" par le nom de votre nouvelle page)
            Sauvegarde savePage = new Sauvegarde();
            this.Close();
            savePage.Show();
            // Fermer la fenêtre actuelle
        }
    }
}