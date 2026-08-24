using NUnit.Framework;
using UnityEngine;
using WheelGame.Gameplay.Progression;

namespace WheelGame.Tests.EditMode.Gameplay
{
    public class ZoneManagerLogicTests
    {
        private GameObject _gameObject;
        private ZoneManager _zoneManager;

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject("ZoneManagerTests");
            _zoneManager = _gameObject.AddComponent<ZoneManager>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_gameObject);
        }

        [Test]
        public void CurrentLevel_ShouldStartAtOne()
        {
            Assert.AreEqual(1, _zoneManager.CurrentLevel);
        }

        [Test]
        public void IncreaseLevel_ShouldIncrementCurrentLevel()
        {
            _zoneManager.IncreaseLevel();

            Assert.AreEqual(2, _zoneManager.CurrentLevel);
        }

        [Test]
        public void ResetLevel_ShouldSetCurrentLevelToOne()
        {
            _zoneManager.IncreaseLevel();
            _zoneManager.IncreaseLevel();
            _zoneManager.ResetLevel();

            Assert.AreEqual(1, _zoneManager.CurrentLevel);
        }

        [Test]
        public void IsSafeZone_ShouldBeTrue_ForMultiplesOfFiveExceptSuperZones()
        {
            for (int i = 0; i < 4; i++)
            {
                _zoneManager.IncreaseLevel();
            }

            Assert.IsTrue(_zoneManager.IsSafeZone);
            Assert.IsFalse(_zoneManager.IsSuperZone);
        }

        [Test]
        public void IsSuperZone_ShouldBeTrue_ForMultiplesOfThirty()
        {
            for (int i = 0; i < 29; i++)
            {
                _zoneManager.IncreaseLevel();
            }

            Assert.IsTrue(_zoneManager.IsSuperZone);
            Assert.IsFalse(_zoneManager.IsSafeZone);
        }

        [Test]
        public void GetNextSafeZoneLevel_ShouldSkipSuperZoneLevels()
        {
            for (int i = 0; i < 24; i++)
            {
                _zoneManager.IncreaseLevel();
            }

            int nextSafeZone = _zoneManager.GetNextSafeZoneLevel();

            Assert.AreEqual(35, nextSafeZone);
        }

        [Test]
        public void GetNextSuperZoneLevel_ShouldReturnNextMultipleOfThirty()
        {
            for (int i = 0; i < 6; i++)
            {
                _zoneManager.IncreaseLevel();
            }

            int nextSuperZone = _zoneManager.GetNextSuperZoneLevel();

            Assert.AreEqual(30, nextSuperZone);
        }
    }
}