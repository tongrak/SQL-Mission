namespace Assets.Scripts.PuzzleComponent.BlankBlockComponent
{
    public interface IFixedTemplateService
    {
        string[] OperatorsSymbol { get; }
        string[] OperatorsWord { get; }
        string[] Function { get; }
        string[] Command { get; }
    }
}
