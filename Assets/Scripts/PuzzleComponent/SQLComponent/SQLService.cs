using System;
using System.Data;
using System.Linq;
using Mono.Data.Sqlite;

namespace Assets.Scripts.PuzzleComponent.SQLComponent
{
    public class SQLService: ISQLService
    {
        private string[] _bannedWords = { "create", "update", "delete", "insert", "drop", "alter", "truncate", "grant", "revoke", "commit", "rollback", "savepoint" };
        /// <summary>
        /// Get result from executing SQL.
        /// First column must be Images' name if puzzle type is Float image.
        /// </summary>
        /// <param name="dbConn">Must be full path for connecting to Database example "URI=file:folder/database.db"</param>
        /// <param name="sql">SQL command</param>
        /// <param name="puzzleType">Type of puzzle that want to execute.</param>
        /// <exception cref="SqliteException">If sql have banned word, it will throw exception</exception>
        /// <returns>Result after execute SQL and first row is attribute. If puzzle type is "A" then first column must be image column</returns>
        public string[][] GetTableResult(string dbConn, string sql, ImgType puzzleType)
        {
            // 1) Check banned word & validate sql
            _ValidateSQL(dbConn, sql);

            // 2) If puzzle type is float image, insert img column to sql command.
            if (puzzleType == ImgType.A)
            {
                sql = _InsertImgColumn(sql);
            }

            // 3) Execute & return result
            return _GetQueryResult(dbConn, sql);
        }

        #region For validate method
        /// <summary>
        /// Validate sql & search check if have banned word for preventing from sql injection.
        /// </summary>
        /// <param name="dbConn">Full path for connecting to sqlite database.</param>
        /// <param name="sql">SQL command.</param>
        /// <exception cref="SqliteException">If sql have banned word, it will throw exception</exception>
        private void _ValidateSQL(string dbConn, string sql)
        {
            if (_HaveBannedWord(sql))
            {
                throw new SqliteException(_GetWarningWord_BannedWord());
            }
            else
            {
                // Connect to database
                using (SqliteConnection connection = new SqliteConnection(dbConn))
                {
                    connection.Open();
                    // Query to database
                    using (SqliteCommand command = new SqliteCommand(sql, connection))
                    {
                        // Read data from query
                        using (IDataReader reader = command.ExecuteReader())
                        {

                        }
                    }
                    connection.Close();
                }
            }
        }

        private bool _HaveBannedWord(string sql)
        {
            string[] sqlWords = sql.ToLower().Split(' ', ';');

            foreach (string word in sqlWords)
            {
                if (_bannedWords.Contains(word))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Warning word for query that use banned word.
        /// </summary>
        /// <returns>Warning message for query that use banned word.</returns>
        private string _GetWarningWord_BannedWord()
        {
            string warningWord = "You don't have permission to use this command:";

            for (int i = 0; i < _bannedWords.Length; i++)
            {
                warningWord += " \"" + _bannedWords[i] + "\"";
                if (i < _bannedWords.Length - 1)
                {
                    warningWord += ",";
                }
            }

            return warningWord;
        }
        #endregion

        /// <summary>
        /// Get result from executing SQL.
        /// </summary>
        /// <param name="dbConn">Must be full path for connecting to Database example "URI=file:folder/database.db"</param>
        /// <param name="sql">SQL command</param>
        /// <returns>Result from execute sql command and first row is attributes.</returns>
        private string[][] _GetQueryResult(string dbConn, string sql)
        {
            string[][] queryResult;
            int numOfRecord = 0;

            // Connect to database
            using (SqliteConnection connection = new SqliteConnection(dbConn))
            {
                connection.Open();
                // Query to database
                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    // Count number of record
                    using (IDataReader forCountReader = command.ExecuteReader())
                    {
                        while (forCountReader.Read())
                        {
                            numOfRecord += 1;
                        }
                    }
                    // Read data from query
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        queryResult = new string[reader.FieldCount][];

                        // set attribute in result
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            queryResult[i] = new string[numOfRecord + 1];
                            queryResult[i][0] = reader.GetName(i);
                        }
                        // fill value for each header from each row in table
                        int record_index = 1;
                        while (reader.Read())
                        {
                            for (int j = 0; j < reader.FieldCount; j++)
                            {
                                queryResult[j][record_index] = reader.GetValue(j).ToString();
                            }
                            record_index++;
                        }
                    }
                }
                connection.Close();
            }
            return queryResult;
        }

        /// <summary>
        /// Insert image column to query command.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>Query command that have image column.</returns>
        private string _InsertImgColumn(string sql)
        {
            const string imgColumn = "MockImgColumn";
            const string selectWord = "select ";

            int selectIndex = sql.ToLower().IndexOf(selectWord);

            if (selectIndex >= 0 && sql.ElementAt(selectIndex + selectWord.Length) != '*')
            {
                int imgIndex = selectIndex + selectWord.Length;

                return sql.Insert(imgIndex, $"{imgColumn},");
            }
            else
            {
                return sql;
            }
        }

        public Schema[] GetSchemas(string dbConn, string[] tables)
        {
            Schema[] schemas = new Schema[tables.Length];

            // Connect to database
            using (SqliteConnection connection = new SqliteConnection(dbConn))
            {
                connection.Open();

                foreach(string table in tables)
                {
                    string sql = $"SELECT * FROM {table} LIMIT 1";
                    // Query to database
                    using (SqliteCommand command = new SqliteCommand(sql, connection))
                    {
                        // Read data from query
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            string[] attributes = new string[reader.FieldCount];

                            // get all attribute
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                attributes[i] = reader.GetName(i);
                            }

                            schemas.Append(new Schema(table, attributes));
                        }
                    }
                }
                connection.Close();
            }
            return schemas;
        }
    }
}
