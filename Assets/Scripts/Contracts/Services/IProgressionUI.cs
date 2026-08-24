namespace WheelGame.Contracts.Services
{
    public interface IProgressionUI
    {
        void UpdateUpcomingZones(int nextSafeLevel, int nextSuperLevel);
        void InitProgressBar(int currentLevel);
        void UpdateLevelProgress(int currentLevel);
    }
}