using System;
using System.Collections;
using UnityEngine;

namespace Gameplay.UI
{
    public readonly struct SchemaDTO
    {
        public readonly string tableName;
        public readonly string[] attribuites;
        public SchemaDTO(string tableName, string[] attribuites)
        {
            this.tableName = tableName;
            this.attribuites = attribuites;
        }
    }
    public readonly struct ExecuteResultDTO
    {
        public readonly (bool isError, string errorMessage) errorModel;
        public readonly string[][] tableResult;
        public ExecuteResultDTO((bool isError, string errorMessage) errorModel, string[][] tableResult)
        {
            this.errorModel = errorModel;
            this.tableResult = tableResult;
        }
    }

}