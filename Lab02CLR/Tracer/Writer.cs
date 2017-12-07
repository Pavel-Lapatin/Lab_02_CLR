using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;

namespace NetMastery.Lab02CLR.TracerLibrary
{
    public abstract class Writer
    {
        protected ITraceResultFormatter Formatter { get; }

        protected Writer(ITraceResultFormatter formatter)
        {
            Formatter = formatter;
        }

        public abstract void WriteResult(ITraceResult results);
    }
}
