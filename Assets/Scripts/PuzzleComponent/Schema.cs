namespace Assets.Scripts.PuzzleComponent
{
    public struct Schema
    {
        public readonly string TableName;
        public readonly string[] Attributes;

        public Schema(string tableName, string[] attributes)
        {
            TableName = tableName;
            Attributes = attributes;
        }
    }
}
