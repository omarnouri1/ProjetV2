using static ProjetV2.Model.BackUpTempsReel;
using Newtonsoft.Json;
using System.IO;
using ProjetV2.Model;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Diagnostics;

namespace ProjetV2.Services
{
    public class BackUpTempsReelService
    {
        public BackUpTempsReel backupTempsreel;


        public void AjouterLogTempsreel(string JobName, string SourceFilePath, string TargetFilePath, string State, long TotalFilesToCopy, long TotalFileSize, long NbFilesLeftToDo, DateTime Time)
        {
            // Charger les logs existants depuis le fichier
            List<LogEntry> logs = ChargerLogs();

            // Ajouter une nouvelle entrée
            LogEntry nouvelleEntree = new LogEntry
            {
                JobName = JobName,
                SourceFilePath = SourceFilePath,
                TargetFilePath = TargetFilePath,
                State = State,
                TotalFilesToCopy = TotalFilesToCopy,
                TotalFilesSize = TotalFileSize,
                NbFilesLeftToDo = NbFilesLeftToDo,
                Time = Time
            };

            logs.Add(nouvelleEntree);

            // Enregistrer la liste mise à jour dans le fichier JSON
            SauvegarderLogs(logs);
        }

        public List<LogEntry> ChargerLogs()
        {
            // Charger le contenu du fichier JSON dans une liste
            string contenuFichier = File.ReadAllText("C:\\Users\\gu\\Desktop\\FichiersLogs\\Backuptempsreel.json");
            List<LogEntry> logs = JsonConvert.DeserializeObject<List<LogEntry>>(contenuFichier);

            // Si la liste est nulle, initialiser avec une liste vide
            return logs ?? new List<LogEntry>();
        }

        public void SauvegarderLogs(List<LogEntry> logs)
        {
            // Convertir la liste en format JSON et enregistrer dans le fichier
            string contenuFichier = JsonConvert.SerializeObject(logs, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("C:\\Users\\gu\\Desktop\\FichiersLogs\\Backuptempsreel.json", contenuFichier);
        }
        public T OuvrirJson<T>(string pathdujson)
        {
            try
            {
                if (File.Exists(pathdujson))
                {
                    string contenuJson = File.ReadAllText(pathdujson);
                    T objetDeserialise = JsonConvert.DeserializeObject<T>(contenuJson);
                    return objetDeserialise;
                }
                else
                {
                    Console.WriteLine("Le fichier JSON spécifié n'existe pas.");
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la lecture du fichier JSON : {ex.Message}");
                return default(T);
            }
        }

        public void ConvertirJsonEnXml(string cheminJson, string cheminXml)
        {
            // Lecture du fichier JSON avec encodage UTF-8
            string contenuJson = File.ReadAllText(cheminJson, Encoding.UTF8);

            // Convertir le JSON en tableau d'objets JToken
            JToken tableauJson = JToken.Parse(contenuJson);

            // Créer le document XML
            XDocument documentXml = new XDocument(new XElement("Root"));

            // Ajouter les éléments XML à partir des objets JSON dans le tableau
            AjouterElementsXml(documentXml.Root, tableauJson);

            // Sauvegarder le document XML dans un fichier avec encodage UTF-8
            using (var stream = new StreamWriter(cheminXml, false, Encoding.UTF8))
            {
                documentXml.Save(stream);
            }
        }

        public void AjouterElementsXml(XElement parent, JToken token)
        {
            if (token.Type == JTokenType.Array)
            {
                // Si le token est un tableau, traiter chaque élément du tableau
                foreach (var element in token)
                {
                    AjouterElementsXml(parent, element);
                }
            }
            else if (token.Type == JTokenType.Object)
            {
                // Si le token est un objet, ajouter les éléments XML
                foreach (var propriete in ((JObject)token).Properties())
                {
                    XElement nouvelElement = new XElement(propriete.Name);

                    AjouterElementsXml(nouvelElement, propriete.Value);

                    parent.Add(nouvelElement);
                }
            }
            else
            {
                // Si le token est une valeur, ajouter un élément avec la valeur
                parent.Add(new XElement("Item", token.ToString()));
            }
        }
        public void OuvrirFichier(string chemin)
        {
            try
            {
                // Vérifier si le fichier existe avant de l'ouvrir
                if (System.IO.File.Exists(chemin))
                {
                    // Utiliser Process.Start pour ouvrir le fichier avec l'application par défaut
                    Process.Start(chemin);
                }
                else
                {
                    Console.WriteLine("Le fichier n'existe pas : " + chemin);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite : " + ex.Message);
            }
        }

    }

}

