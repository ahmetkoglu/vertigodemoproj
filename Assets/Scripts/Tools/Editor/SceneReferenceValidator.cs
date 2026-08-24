using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using WheelGame.Gameplay.Inventory;
using WheelGame.Gameplay.Management;
using WheelGame.Gameplay.Progression;
using WheelGame.Gameplay.Wheel;
using WheelGame.UI;

namespace WheelGame.Tools.Editor
{
    public static class SceneReferenceValidator
    {
        #region Menu Entry

        [MenuItem("Tools/WheelGame/Validate Active Scene References")]
        public static void ValidateActiveSceneReferences()
        {
            List<string> issues = new List<string>();

            ValidateManager(Object.FindObjectOfType<GameManager>(), new[]
            {
                "uiManager",
                "wheelManager",
                "zoneManager",
                "inventoryManager"
            }, issues);

            ValidateManager(Object.FindObjectOfType<UIManager>(), new[]
            {
                "mainBackgroundPanel",
                "revivePanel",
                "exitPopupCanvasGroup",
                "exitPopupBox",
                "mainCanvas",
                "uiEffectManager",
                "inventoryContainer",
                "inventoryItemPrefab",
                "levelContainer",
                "levelSlotPrefab",
                "btn_spin",
                "btn_walk_away",
                "btn_stay",
                "btn_leave",
                "btn_revive",
                "btn_give_up"
            }, issues);

            ValidateManager(Object.FindObjectOfType<WheelManager>(), new[]
            {
                "wheelBaseImage",
                "indicatorImage",
                "wheelGlowImage",
                "wheelContainer",
                "slicePrefab"
            }, issues);

            ValidateManager(Object.FindObjectOfType<ZoneManager>(), new[]
            {
                "normalZoneData",
                "safeZoneData",
                "superZoneData"
            }, issues);

            ZoneData[] zoneDataAssets = Resources.FindObjectsOfTypeAll<ZoneData>();
            foreach (ZoneData zoneData in zoneDataAssets)
            {
                if (zoneData == null)
                {
                    continue;
                }

                if (zoneData.availableRewards == null || zoneData.availableRewards.Count == 0)
                {
                    issues.Add($"[ZoneData] availableRewards is empty in asset '{zoneData.name}'.");
                }

                if (zoneData.hasBomb && zoneData.bombData == null)
                {
                    issues.Add($"[ZoneData] hasBomb is enabled but bombData is missing in asset '{zoneData.name}'.");
                }
            }

            if (issues.Count == 0)
            {
                Debug.Log("[SceneReferenceValidator] Active scene validation passed. No missing references were found.");
                EditorUtility.DisplayDialog("WheelGame Validation", "Active scene validation passed. No missing references were found.", "OK");
                return;
            }

            foreach (string issue in issues)
            {
                Debug.LogWarning(issue);
            }

            EditorUtility.DisplayDialog(
                "WheelGame Validation",
                $"Validation finished with {issues.Count} issue(s). Check the Console for details.",
                "OK");
        }

        #endregion

        #region Validation Helpers

        private static void ValidateManager(Object target, string[] fieldNames, List<string> issues)
        {
            if (target == null)
            {
                issues.Add("[SceneReferenceValidator] Required scene object is missing.");
                return;
            }

            foreach (string fieldName in fieldNames)
            {
                FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
                if (field == null)
                {
                    issues.Add($"[{target.GetType().Name}] Field '{fieldName}' was not found.");
                    continue;
                }

                object value = field.GetValue(target);
                if (value == null)
                {
                    issues.Add($"[{target.GetType().Name}] Field '{fieldName}' is not assigned on '{target.name}'.");
                }
            }
        }

        #endregion
    }
}