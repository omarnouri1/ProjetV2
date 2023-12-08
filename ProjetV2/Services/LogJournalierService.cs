using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjetV2.Model.LogJournalier;
using System.Collections.Generic;
using System.IO;
using ProjetV2.Model;


namespace ProjetV2.Services
{
    public class LogJournalierService
    {
        public LogJournalier logJournalier;
        
       
      
        public void AjouterLog(string nomSauvegarde, string source, string destination, long tailleFichier, double tempsTransfert, DateTime time, double tempsCryptage)
        {
            // Charger les logs existants depuis le fichier
            List<LogEntry> logs = ChargerLogs();

            // Ajouter une nouvelle entrée
            LogEntry nouvelleEntree = new LogEntry
            {
                NomSauvegarde = nomSauvegarde,
                Source = source,
                Destination = destination,
                TailleFichier = tailleFichier,
                TempsTransfert = tempsTransfert,
                Time = time ,
                TempsCryptage = tempsCryptage
            };

            logs.Add(nouvelleEntree);

            // Enregistrer la liste mise à jour dans le fichier JSON
            SauvegarderLogs(logs);
        }

    

        public List<LogEntry> ChargerLogs()
        {
            // Charger le contenu du fichier JSON dans une liste
            string contenuFichier = File.ReadAllText("C:\\Users\\gu\\Desktop\\FichiersLogs\\LogJournalier.json");
            List<LogEntry> logs = JsonConvert.DeserializeObject<List<LogEntry>>(contenuFichier);

            // Si la liste est nulle, initialiser avec une liste vide
            return logs ?? new List<LogEntry>();
        }

        public void SauvegarderLogs(List<LogEntry> logs)
        {
            // Convertir la liste en format JSON et enregistrer dans le fichier
            string contenuFichier = JsonConvert.SerializeObject(logs, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("C:\\Users\\gu\\Desktop\\FichiersLogs\\LogJournalier.json", contenuFichier);
        }
    }


}

