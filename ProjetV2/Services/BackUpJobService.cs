using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetV2.Services
{
    internal class BackUpJobService
    {
        public void add_backup(string job_name, string source_direction, string target_direction, string backup_type,
            string FileSource, string FileDestination)
        {
            try
            {
                // Vérifier si le fichier ou dossier source existe
                if (File.Exists(FileSource) || Directory.Exists(FileSource))
                {
                    // Vérifier si le répertoire de destination existe, sinon le créer
                    if (!Directory.Exists(FileDestination))
                    {
                        Directory.CreateDirectory(FileDestination);
                    }

                    // Construire le chemin complet de la destination
                    string destinationPath = Path.Combine(FileDestination, Path.GetFileName(FileSource));

                    // Copier le fichier ou le dossier de source vers destination
                    if (File.Exists(FileSource))
                    {
                        File.Copy(FileSource, destinationPath, true);
                    }
                    else if (Directory.Exists(FileSource))
                    {
                        CopyDirectory(FileSource, destinationPath);
                    }

                    Console.WriteLine(System.Threading.Thread.CurrentThread.CurrentCulture.Name == "fr-FR" ? "La sauvegarde  '" + job_name + "' a été créée avec succès." : "The Backup  '" + job_name + "' Was added successfully.");

                }
                else
                {
                    Console.WriteLine("Le fichier ou le dossier source n'existe pas.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite : " + ex.Message);
            }
        }

        public void CopyDirectory(string sourceDir, string targetDir)
        {
            // Créer le répertoire de destination s'il n'existe pas
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            // Copier les fichiers
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string dest = Path.Combine(targetDir, Path.GetFileName(file));
                File.Copy(file, dest, true);
            }

            // Copier les sous-répertoires récursivement
            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string destSubDir = Path.Combine(targetDir, Path.GetFileName(subDir));
                CopyDirectory(subDir, destSubDir);
            }
        }


        public long GetSize(string fullPath)
        {
            long totalSize = 0;

            // Vérifie si le chemin spécifié est un dossier
            if (Directory.Exists(fullPath))
            {
                // Obtenir la taille des fichiers dans le dossier
                foreach (var file in Directory.GetFiles(fullPath))
                {
                    var fileInfo = new FileInfo(file);
                    totalSize += fileInfo.Length;
                }

                // Obtenir la taille des sous-dossiers en appelant récursivement la fonction GetSize
                foreach (var directory in Directory.GetDirectories(fullPath))
                {
                    totalSize += GetSize(directory); // Correction ici : utiliser le sous-dossier, pas le chemin d'origine
                }
            }
            else if (File.Exists(fullPath))
            {
                // Si le chemin spécifié est un fichier, obtenir simplement la taille du fichier
                var fileInfo = new FileInfo(fullPath);
                totalSize += fileInfo.Length;
            }
            else
            {
                // Le chemin n'existe pas
                Console.WriteLine("Le chemin spécifié n'existe pas.");
            }

            return totalSize;
        }
        public string CompareFolderSizes(string path1, string path2)
        {
            long size1 = GetSize(path1);
            long size2 = GetSize(path2);

            if (size1 == size2)
            {
                return "end";
            }
            else
            {
                return "active";
            }
        }
        public int CountFilesInFolder(string folderPath)
        {
            int fileCount = 0;

            try
            {
                // Récupérer tous les fichiers du dossier et de ses sous-dossiers
                string[] files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);

                // Compter le nombre de fichiers
                fileCount = files.Length;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors du comptage des fichiers : {ex.Message}");
            }

            return fileCount;
        }

    }
}

