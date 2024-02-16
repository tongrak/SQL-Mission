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
}