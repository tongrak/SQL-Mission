using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InsideChapterLayer.Model
{
    public class MissionPaperDetail
    {
        public string MissionName { get; set; }
        public IDictionary<string, bool> MissionDependencies { get; set; }
        public bool IsPass { get; set; }
    }
}
