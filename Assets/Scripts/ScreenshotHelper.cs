using UnityEngine;
using System.IO;

/// <summary>
/// Utility helper to capture pixel-perfect screenshots at the current Game View resolution.
/// Operates via Unity Editor Context Menus to avoid polluting runtime UI space.
/// </summary>
public class ScreenshotHelper : MonoBehaviour
{
    [Header("Save Settings")]
    [Tooltip("The folder name created in the project root directory (next to Assets folder).")]
    [SerializeField] private string folderName = "GameScreenshots";

    /// <summary>
    /// Captures a high-fidelity, un-aliased screenshot of the active Game View layout.
    /// Can be triggered directly from the Inspector inside the Unity Editor.
    /// </summary>
    [ContextMenu("📸 TAKE SCREENSHOT 📸")]
    public void TakeScreenshot()
    {
        // 1. Create a dedicated folder in the project root (next to Assets, Library) to keep things organized
        string directoryPath = Path.Combine(Application.dataPath, "..", folderName);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // 2. Generate a unique file name using active pixel resolution and exact timestamps
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = $"Capture_{Screen.width}x{Screen.height}_{timestamp}.png";
        string fullPath = Path.Combine(directoryPath, fileName);

        // 3. Execute Unity's high-fidelity capture pipeline
        ScreenCapture.CaptureScreenshot(fullPath);

        // 4. Log the output pathway cleanly to the developer console
        Debug.Log($"<color=cyan>[ScreenshotHelper]</color> Pixel-perfect capture saved successfully to: <color=yellow>{fullPath}</color>");
    }
}