using System;
using System.Collections.Generic;
namespace Assets.Scripts.PuzzleComponent.BlankBlockComponent
{
    public class FixedTemplateService : IFixedTemplateService
    {
        public string[] OperatorsSymbol { get; } = { ">", "<", "=", ">=", "<=", "<>" };

        public string[] OperatorsWord { get; } = { "AND", "OR", "NOT", "BETWEEN", "LIKE" };

        public string[] Function { get; } = { "SUM", "AVG", "COUNT", "MIN", "MAX" };

        public string[] Command { get; } = { "SELECT", "FROM", "WHERE", "GROUP BY", "HAVING", "ORDER BY" };
    }
}
