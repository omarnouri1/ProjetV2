using Newtonsoft.Json;
using ProjetV2.Model;
using ProjetV2.Services;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace ProjetV2.View
{
    public partial class Sauvegarde : Window
    {
        private BackUpJobService backupJobService;
        private LogJournalierService logJournalierService;
        private BackUpTempsReelService backUpTempsReelService;
       

        public Sauvegarde()
        {
            InitializeComponent();
            backupJobService = new BackUpJobService();
            logJournalierService = new LogJournalierService();
            backUpTempsReelService = new BackUpTempsReelService();
           
        }
        Stopwatch stopwatch = new Stopwatch();
        // Exemple d'utilisation de la classe LogJournalier
        


        private void ButtonLangFr_Click(object sender, RoutedEventArgs e)
        {
            NomSauvegardeLabel.Text = "Nom Du Backup";
            SourceLabel.Text = "Repertoire Source";
            DestinationLabel.Text = "Repertoire Destination";
            TypeBackupLabel.Text = "Type de Backup";
            PathSourceDirectoryLabel.Text = "Adresse source";
            PathDestinationDirectoryLabel.Text = "Adresse Destination";
            Ajouter.Content = "Ajouter";
            TextBoxLabel.Text = "Si oui , donner le clé de cryptage (8 bits)";
            EnableTextBoxRadioButton.Content = "Crypter ce backjob ?";
            AjouterAvecCryptage.Content = "Ajouter avec cryptage";
            Pause.Content = "Pause";
            Arreter.Content = "Arrêter";
            Xml.Content = "Exporter en XML";
            Json.Content = "Exporter en Json";
        }

        private void ButtonLangEn_Click(object sender, RoutedEventArgs e)
        {
            NomSauvegardeLabel.Text = "BackUp Name";
            SourceLabel.Text = "Direction source";
            DestinationLabel.Text = "Direction Target";
            TypeBackupLabel.Text = "Backup Type";
            PathSourceDirectoryLabel.Text = "Path Source";
            PathDestinationDirectoryLabel.Text = "Path Destination";
            TextBoxLabel.Text = "If yes , please write your crypt key (8 bits) ";
            EnableTextBoxRadioButton.Content = "Encrypt this backjob ?";
            AjouterAvecCryptage.Content = "Add with encryption";
            Ajouter.Content = "Add";
            Pause.Content = "Pause";
            Arreter.Content = "Stop";
            Xml.Content = "Export in XML";
            Json.Content = "Export in Json";
        }


        public void ButtonAjouter_Click(object sender, RoutedEventArgs e)
        {
            string typeBackup = ((ComboBoxItem)TypeBackupComboBox.SelectedItem)?.Content.ToString();
            string Source = ((ComboBoxItem)SourceTextBox.SelectedItem)?.Content.ToString();
            string Destination = ((ComboBoxItem)DestinationTextBox.SelectedItem)?.Content.ToString();
            string nomSauvegarde = NomSauvegardeTextBox.Text;
            string pathSourceDirectory = PathSourceDirectoryTextBox.Text;
            string pathDestinationDirectory = PathDestinationDirectoryTextBox.Text;
            // mettre StopWatch en start  
            stopwatch.Start();
            // Appeler la méthode add_backup avec les paramètres initialisés
            backupJobService.add_backup(nomSauvegarde, Source, Destination, typeBackup, pathSourceDirectory, pathDestinationDirectory);
            // mettre StopWatch en stop  
            stopwatch.Stop();
            MessageBox.Show($"Sauvegarde effectuée avec succès!\nBackup done successfully! \nNom de Sauvegarde : {nomSauvegarde}");
            // mettre la duree dans la variable tempstransfert
            TimeSpan tempsTransfert = stopwatch.Elapsed;
            long filesize = backupJobService.GetSize(pathSourceDirectory);
            // Exemple d'ajout d'une entrée au journal
            logJournalierService.AjouterLog(nomSauvegarde, Source, Destination, filesize, tempsTransfert.TotalMilliseconds, DateTime.Now,0);
           
            string etat = backupJobService.CompareFolderSizes(pathSourceDirectory, pathDestinationDirectory);
            int nombrefichiersource = backupJobService.CountFilesInFolder(pathSourceDirectory);
            int nombrefichierdest = backupJobService.CountFilesInFolder(pathDestinationDirectory);
            int nbfichierrest = nombrefichiersource - nombrefichierdest;         
            //Ajout d'un log temps reel et log journalier
            backUpTempsReelService.AjouterLogTempsreel(nomSauvegarde, pathSourceDirectory, pathDestinationDirectory,etat, nombrefichiersource, filesize, nbfichierrest, DateTime.Now);
            logJournalierService.AjouterLog(nomSauvegarde, Source, Destination, filesize, tempsTransfert.TotalMilliseconds, DateTime.Now,0);
        }

        private void ButtonPause_Click(object sender, RoutedEventArgs e)
        {
            // Logique de pause
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            // Logique d'arrêt
        }

        private void ButtonExportXML_Click(object sender, RoutedEventArgs e)
        {
            string pathToJson = "C:\\Users\\gu\\Desktop\\FichiersLogs\\LogJournalier.json";
            string pathToXml = "C:\\Users\\gu\\Desktop\\FichiersLogs\\data.xml";


            backUpTempsReelService.ConvertirJsonEnXml(pathToJson, pathToXml);
            backUpTempsReelService.OuvrirFichier(pathToXml);

            MessageBox.Show($"Sauvegarde exporté en XML\n Backjob exported to XML ");

        }

        private void ButtonExportJSON_Click(object sender, RoutedEventArgs e)
        {
            string cheminDuFichierJson = "C:\\Users\\gu\\Desktop\\FichiersLogs\\LogJournalier.json";
            var listeDesSauvegardes = backUpTempsReelService.OuvrirJson<List<LogJournalier.LogEntry>>(cheminDuFichierJson);

            if (listeDesSauvegardes != null && listeDesSauvegardes.Count > 0)
            {
                // Utilisez une StringBuilder pour construire le message
                StringBuilder messageBuilder = new StringBuilder();

                foreach (var sauvegarde in listeDesSauvegardes)
                {
                    messageBuilder.AppendLine($"Nom de la sauvegarde : {sauvegarde.NomSauvegarde}\nSource : {sauvegarde.Source}\n");
                }

                // Affichez le message dans la MessageBox
                MessageBox.Show(messageBuilder.ToString());
            }
            else
            {
                MessageBox.Show("La liste des sauvegardes est vide ou le fichier JSON est vide.");
            }
            backUpTempsReelService.OuvrirFichier(cheminDuFichierJson);
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            // Fermer la fenêtre actuelle
            this.Close();
        }
        private void EnableTextBox_Checked(object sender, RoutedEventArgs e)
        {
            TextBoxControl.IsEnabled = true;
        }

        private void EnableTextBox_Unchecked(object sender, RoutedEventArgs e)
        {
            TextBoxControl.IsEnabled = false;
            TextBoxControl.Text = string.Empty; // Effacez le texte lorsque le champ est désactivé si nécessaire
        }
        private void ButtonAjouterAvecCryptage_Click(object sender, RoutedEventArgs e)
        {
            string typeBackup = ((ComboBoxItem)TypeBackupComboBox.SelectedItem)?.Content.ToString();
            string Source = ((ComboBoxItem)SourceTextBox.SelectedItem)?.Content.ToString();
            string Destination = ((ComboBoxItem)DestinationTextBox.SelectedItem)?.Content.ToString();
            string nomSauvegarde = NomSauvegardeTextBox.Text;
            string pathSourceDirectory = PathSourceDirectoryTextBox.Text;
            string pathDestinationDirectory = PathDestinationDirectoryTextBox.Text;
            string CleCryptage = TextBoxControl.Text;
            string etat = backupJobService.CompareFolderSizes(pathSourceDirectory, pathDestinationDirectory);
            int nombrefichiersource = backupJobService.CountFilesInFolder(pathSourceDirectory);
            int nombrefichierdest = backupJobService.CountFilesInFolder(pathDestinationDirectory);
            int nbfichierrest = nombrefichiersource - nombrefichierdest;
            long filesize = backupJobService.GetSize(pathSourceDirectory);
            stopwatch.Start();
            // Appeler la méthode add_backup avec les paramètres initialisés
            backupJobService.add_backup(nomSauvegarde, Source, Destination, typeBackup, pathSourceDirectory, pathDestinationDirectory);
            // mettre StopWatch en stop  
            stopwatch.Stop();
            // mettre la duree dans la variable tempstransfert
            TimeSpan tempsTransfert = stopwatch.Elapsed;
            // Chemin vers votre programme console .exe
            string cheminExe = @"C:\Users\gu\source\repos\CryptoSoft\CryptoSoft\bin\Debug\net8.0\CryptoSoft.exe";
            
            // Créez un processus pour lancer votre programme avec les paramètres
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = cheminExe,
                Arguments = $"{pathDestinationDirectory} {pathDestinationDirectory} {CleCryptage}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            stopwatch.Start();
            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                Console.WriteLine(output);  
                process.WaitForExit(); 
            }
            stopwatch.Stop();
            TimeSpan tempsTransfertcryptage = stopwatch.Elapsed;
            logJournalierService.AjouterLog(nomSauvegarde, Source, Destination, filesize, tempsTransfert.TotalMilliseconds,
                DateTime.Now, tempsTransfertcryptage.TotalMilliseconds);
           
                backUpTempsReelService.AjouterLogTempsreel(nomSauvegarde, pathSourceDirectory, pathDestinationDirectory
                    , etat, nombrefichiersource, filesize, nbfichierrest, DateTime.Now);
            
          
          
        }

    }

    }

