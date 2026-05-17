# 🎡 Wheel of Fortune - Mobile Loot System

A production-ready, highly optimized mobile Lucky Wheel system built with **Unity 2021 LTS**. This project was architected using strict software engineering principles to ensure decoupled UI management, high performance on mobile devices, and seamless scaling across multiple screen aspect ratios.

---

## 🚀 Technical Specifications & Features

* **Engine Version:** Unity 2021.3 LTS (Ensures long-term stability and platform compatibility)
* **Scripting Backend:** IL2CPP (Compiled to native C++ code for high performance)
* **Target Architectures:** ARMv7 and ARM64 (Full compatibility with modern 64-bit Android devices)
* **Performance Optimizations:** * Locked at **60 FPS** (`Application.targetFrameRate`) to bypass Unity's default mobile 30 FPS throttle.
  * Integrated **Sprite Atlas** to minimize draw calls and optimize UI batching.
  * Decoupled **Static and Dynamic Canvases** to dramatically reduce Canvas Re-batching overhead during spin animations.
  * Raycast Target optimization on static UI elements to reduce GPU overhead.

---

## 🏗️ Architecture & Design Patterns

The codebase is engineered with a focus on maintainability, scalability, and testability, completely avoiding the common "Fat Managers" anti-pattern.

### 1. State Pattern (Finite State Machine)
The core game loop is controlled via an interface-based **State Machine**. Transitions between the main game phases are strictly decoupled, ensuring each state has its own isolated logic:
* `InitState`: Prepares wheel data and initializes the layout.
* `WheelRotationState`: Handles deterministic physics-based/tweened rotation.
* `EvaluationState`: Computes rewards and triggers UI rewards animations.
* `GameOverState`: Manages risk/reward pop-ups (Revive/Give Up loops).

### 2. SOLID Principles & Decoupled UI
* **Single Responsibility (SRP):** Game data (ScriptableObjects) and UI presentation are strictly separated. The wheel does not know *what* it is spinning; it simply serves as a visual bridge for the underlying data layer.
* **Open/Closed Principle (OCP):** New rewards or game behaviors can be added without modifying the core spinning or transition logic.

### 3. Slot-Based Hierarchy (Modular Design)
Instead of hardcoding mathematical vector placements (`sin/cos` matrices) inside the code, the wheel uses a **Slot-Based Hierarchy**. Transparent anchors are placed manually inside the editor. The setup manager dynamically instantiates and centers reward prefabs into these anchors (`Vector3.zero`), allowing game designers to swap visuals or add special effects seamlessly without a single line of code change.

---

## 📱 Dynamic UI Scaling (Aspect Ratios)

The UI dynamically resizes across a wide variety of mobile displays using a **Canvas Scaler (
