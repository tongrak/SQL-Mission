using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ChapterLayer.Model
{
    [System.Serializable]
    public class ChapterConfig
    {
        public int ChpaterID;
        public string ChapterTitle;
        public string MissionConfigFolder;
        public string[] MissionFiles;
        public int[] ChapterDependencies;
    }
}
