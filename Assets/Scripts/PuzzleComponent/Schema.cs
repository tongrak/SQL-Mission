namespace Assets.Scripts.PuzzleComponent
{
    public struct Schema
    {
        public string TableName { get; private set; }
        public string[] Attributes { get; private set; }

        public Schema(string tableName, string[] attributes)
        {
            TableName = tableName;
            Attributes = attributes;
        }
    }
}
