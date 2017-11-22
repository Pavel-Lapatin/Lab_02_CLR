
namespace NetMastery.Lab02CLR.Formatters.FormatterPluginContract
{
    public interface ITraceResultFormatter
    {
        string FlagValue { get;}
        string GetFormat();
        void Format(ITraceResult traceResult);
    }
}
