using System;

namespace WheelGame.Contracts.Services
{
    public interface IOverlayUI
    {
        void ShowRevivePanel(bool isActive);
        void PlayBombTensionAnimation(Action onComplete);
    }
}