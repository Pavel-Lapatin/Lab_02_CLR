using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;

namespace NetMastery.Lab02CLR.TracerLibrary
{
    public interface ITracer
    {
        void StartTrace();
        void StopTrace();
        ITraceResult GetTraceResult();
    }
}
