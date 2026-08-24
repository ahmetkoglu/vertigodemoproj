using WheelGame.Contracts.Services;

namespace WheelGame.Tests.EditMode.TestDoubles
{
    public class FakeInputControlUI : IInputControlUI
    {
        public bool SpinButtonState { get; private set; }
        public bool WalkAwayButtonState { get; private set; }

        public void SetSpinButtonState(bool isActive)
        {
            SpinButtonState = isActive;
        }

        public void SetWalkAwayButtonState(bool isActive)
        {
            WalkAwayButtonState = isActive;
        }
    }
}