namespace Assets.Scripts.DataPersistence.BlankBlockComponent
{
    public interface IFixedTemplateService
    {
        string[] OperatorsSymbol { get; }
        string[] OperatorsWord { get; }
        string[] Function { get; }
        string[] Command { get; }
    }
}
