using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using WheelGame.Gameplay.Management;
using WheelGame.Gameplay.StateMachine.States;

namespace WheelGame.Tests.PlayMode.Scene
{
    public class FullSceneFlowPlayModeTests
    {
        #region Scene Setup

        [UnitySetUp]
        public IEnumerator LoadScene()
        {
            SceneManager.LoadScene("SampleScene");
            yield return null;
            yield return null;
        }

        #endregion

        #region Full Scene Flow Tests

        [UnityTest]
        public IEnumerator SceneStart_ShouldReachIdleState_AfterInitialBootSequence()
        {
            GameManager gameManager = Object.FindObjectOfType<GameManager>();
            Assert.IsNotNull(gameManager, "GameManager was not found in SampleScene.");

            GameStateMachine stateMachine = GetPrivateField<GameStateMachine>(gameManager, "_stateMachine");
            Assert.IsNotNull(stateMachine, "GameStateMachine was not initialized on GameManager.");

            yield return null;

            Assert.IsInstanceOf<IdleState>(stateMachine.CurrentState,
                "Expected the initial flow to complete as InitState -> IdleState.");
        }

        [UnityTest]
        public IEnumerator SpinButtonClick_ShouldTransitionFromIdleToSpinning_InRealScene()
        {
            GameManager gameManager = Object.FindObjectOfType<GameManager>();
            Assert.IsNotNull(gameManager, "GameManager was not found in SampleScene.");

            GameStateMachine stateMachine = GetPrivateField<GameStateMachine>(gameManager, "_stateMachine");
            Assert.IsNotNull(stateMachine, "GameStateMachine was not initialized on GameManager.");

            yield return null;

            Assert.IsInstanceOf<IdleState>(stateMachine.CurrentState,
                "Scene should be idle before simulating spin button click.");

            Button spinButton = FindButtonByName("ui_button_spin");
            Assert.IsNotNull(spinButton, "Spin button could not be found in SampleScene.");
            Assert.IsTrue(spinButton.interactable, "Spin button is expected to be interactable in IdleState.");

            spinButton.onClick.Invoke();
            yield return null;

            Assert.IsInstanceOf<SpinningState>(stateMachine.CurrentState,
                "Clicking the spin button in IdleState should transition to SpinningState.");
        }

        [UnityTest]
        public IEnumerator SpinFlow_ShouldEventuallyLeaveSpinningState_AfterWheelCompletes()
        {
            GameManager gameManager = Object.FindObjectOfType<GameManager>();
            Assert.IsNotNull(gameManager, "GameManager was not found in SampleScene.");

            GameStateMachine stateMachine = GetPrivateField<GameStateMachine>(gameManager, "_stateMachine");
            Assert.IsNotNull(stateMachine, "GameStateMachine was not initialized on GameManager.");

            yield return null;

            Button spinButton = FindButtonByName("ui_button_spin");
            Assert.IsNotNull(spinButton, "Spin button could not be found in SampleScene.");

            spinButton.onClick.Invoke();
            yield return null;

            Assert.IsInstanceOf<SpinningState>(stateMachine.CurrentState);

            yield return new WaitForSeconds(3.75f);
            yield return null;

            Assert.IsFalse(stateMachine.CurrentState is SpinningState,
                "After the wheel animation completes, the state machine should move beyond SpinningState.");
        }

        #endregion

        #region Test Helpers

        private static T GetPrivateField<T>(object target, string fieldName) where T : class
        {
            FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field, $"Field '{fieldName}' was not found on {target.GetType().Name}.");
            return field.GetValue(target) as T;
        }

        private static Button FindButtonByName(string buttonName)
        {
            Button[] buttons = Object.FindObjectsOfType<Button>(true);
            foreach (Button button in buttons)
            {
                if (button.gameObject.name == buttonName)
                {
                    return button;
                }
            }

            return null;
        }

        #endregion
    }
}