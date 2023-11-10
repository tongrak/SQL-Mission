using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.PuzzleComponent.SQLComponent
{
    public interface ISQLService
    {
        string[][] GetTableResult(string dbPath, string sql, PuzzleType puzzleType);
    }
}
