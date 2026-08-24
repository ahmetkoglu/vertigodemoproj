using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using WheelGame.Gameplay.Wheel.Controllers;

namespace WheelGame.Tests.PlayMode.Wheel
{
    public class WheelSpinControllerPlayModeTests
    {
        private GameObject _root;
        private RectTransform _wheelContainer;

        [SetUp]
        public void SetUp()
        {
            _root = new GameObject("WheelSpinControllerPlayModeTests", typeof(RectTransform));
            _wheelContainer = _root.GetComponent<RectTransform>();
        }

        [TearDown]
        public void TearDown()
        {
            if (_root != null)
            {
                Object.DestroyImmediate(_root);
            }
        }

        [UnityTest]
        public IEnumerator SpinWheel_ShouldInvokeOnCompleteWithRequestedIndex()
        {
            WheelSpinController controller = new WheelSpinController(_wheelContainer, 1, 0.05f);
            bool callbackCalled = false;
            int completedIndex = -1;

            controller.SpinWheel(8, 3, index =>
            {
                callbackCalled = true;
                completedIndex = index;
            });

            yield return new WaitForSeconds(0.25f);

            Assert.IsTrue(callbackCalled);
            Assert.AreEqual(3, completedIndex);
        }
    }
}