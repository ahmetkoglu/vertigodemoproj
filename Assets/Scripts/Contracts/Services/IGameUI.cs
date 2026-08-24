using System;

namespace WheelGame.Contracts.Services
{
    public interface IGameUI :
        IInputControlUI,
        IInventoryUI,
        IOverlayUI,
        IProgressionUI
    {
        event Action SpinRequested;
        event Action WalkAwayRequested;
        event Action ReviveRequested;
        event Action GiveUpRequested;
    }
}