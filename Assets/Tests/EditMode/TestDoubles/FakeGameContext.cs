using WheelGame.Contracts.Rewards;
using WheelGame.Contracts.Services;
using WheelGame.Contracts.StateMachine;

namespace WheelGame.Tests.EditMode.TestDoubles
{
    public class FakeGameContext : IGameContext
    {
        public IInputControlUI InputUI { get; set; }
        public IInventoryUI InventoryUI { get; set; }
        public IOverlayUI OverlayUI { get; set; }
        public IProgressionUI ProgressionUI { get; set; }
        public IWheelService Wheel { get; set; }
        public IZoneService Zone { get; set; }
        public IInventoryService Inventory { get; set; }
        public IRewardFlowService RewardFlow { get; set; }
        public IRewardResolver RewardResolver { get; set; }

        public IGameState LastChangedState { get; private set; }

        public void ChangeState(IGameState newState)
        {
            LastChangedState = newState;
        }
    }
}