namespace WTF.Common.DebugSystem
{
    public interface IDebugSystem
    {
        public void Log(string message);
        public void LogWarning(string message);
        public void LogError(string message);
    }
}
