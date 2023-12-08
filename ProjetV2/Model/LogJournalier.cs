using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

namespace ProjetV2.Model
{
    public class LogJournalier
    {
        public class LogEntry
        {
            public string NomSauvegarde;

            public string Source;

            public string Destination;

            public long TailleFichier;

            public double TempsTransfert;

            public DateTime Time;
            public double TempsCryptage;
        }

        public string CheminFichier = "C:\\Users\\gu\\Desktop\\FichiersLogs\\LogJournalier.json";

       



    }
}

