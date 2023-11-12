using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.PuzzleComponent.SQLComponent
{
    public interface ISQLService
    {
        /// <summary>
        /// Get result from executing SQL.
        /// First column must be Images' name if puzzle type is Float image.
        /// </summary>
        /// <param name="dbConn">Must be full path for connecting to Database example "URI=file:folder/database.db"</param>
        /// <param name="sql">SQL command</param>
        /// <param name="puzzleType">Type of puzzle that want to execute.</param>
        /// <exception cref="SqliteException">If sql have banned word, it will throw exception</exception>
        /// <returns>Result after execute SQL and first row is attribute. If puzzle type is "A" then first column must be image column</returns>
        string[][] GetTableResult(string dbConn, string sql, PuzzleType puzzleType);

        /// <summary>
        /// Get group of schema from given tables.
        /// </summary>
        /// <param name="dbConn">Must be full path for connecting to Database example "URI=file:folder/database.db</param>
        /// <param name="tables">Group of table</param>
        /// <returns>Group of schema.</returns>
        Schema[] GetSchemas(string dbConn, string[] tables);
    }
}
