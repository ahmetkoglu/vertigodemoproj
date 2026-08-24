using WheelGame.Contracts.Services;

namespace WheelGame.Tests.EditMode.TestDoubles
{
    public class FakeProgressionUI : IProgressionUI
    {
        public bool InitProgressBarCalled { get; private set; }
        public bool UpdateProgressCalled { get; private set; }
        public bool UpdateUpcomingZonesCalled { get; private set; }
        public int LastProgressLevel { get; private set; }
        public int LastSafeZoneLevel { get; private set; }
        public int LastSuperZoneLevel { get; private set; }

        public void UpdateUpcomingZones(int nextSafeLevel, int nextSuperLevel)
        {
            UpdateUpcomingZonesCalled = true;
            LastSafeZoneLevel = nextSafeLevel;
            LastSuperZoneLevel = nextSuperLevel;
        }

        public void InitProgressBar(int currentLevel)
        {
            InitProgressBarCalled = true;
            LastProgressLevel = currentLevel;
        }

        public void UpdateLevelProgress(int currentLevel)
        {
            UpdateProgressCalled = true;
            LastProgressLevel = currentLevel;
        }
    }
}