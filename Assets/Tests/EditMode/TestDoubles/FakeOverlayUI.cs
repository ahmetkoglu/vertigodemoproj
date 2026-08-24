using System;
using WheelGame.Contracts.Services;

namespace WheelGame.Tests.EditMode.TestDoubles
{
    public class FakeOverlayUI : IOverlayUI
    {
        public bool? RevivePanelState { get; private set; }
        public bool BombTensionCalled { get; private set; }

        public void ShowRevivePanel(bool isActive)
        {
            RevivePanelState = isActive;
        }

        public void PlayBombTensionAnimation(Action onComplete)
        {
            BombTensionCalled = true;
            onComplete?.Invoke();
        }
    }
}