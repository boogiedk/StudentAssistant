using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAssistant.Backend.Models.DownloadFileService
{
    public class DownloadFileParametersModel
    {
        public string FileNameRemote { get; set; }

        public string FileFormat { get; set; }

        public Uri RemoteUri { get; set; }

        public string FileNameLocal { get; set; }

        //public FileNameRemoteType FileNameRemoteType { get; set; }

        public string PathToFile { get; set; }
    }
}
