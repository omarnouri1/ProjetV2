using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

namespace ProjetV2.Model
{
    public class BackUpTempsReel
    {
        public class LogEntry
        {
            public string JobName;
            public string SourceFilePath;
            public string TargetFilePath;
            public string State;
            public long TotalFilesToCopy;
            public long TotalFilesSize;
            public long NbFilesLeftToDo;
            public DateTime Time;
        }

        public string CheminFichier = "C:\\Users\\gu\\Desktop\\FichiersLogs\\Backuptempsreel.json";


    }
}



