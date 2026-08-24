# WheelGame Case Study

A Unity-based wheel reward game prototype refactored with a stronger architecture, modular assembly setup, validation tooling, and automated tests.

## Unity Version
- `2021.3.45f2`

## Project Overview
This project is a wheel-based reward game prototype where the player spins a reward wheel, progresses through zones, collects temporary rewards, and faces bomb/fail states with revive and give up flows.

The project was revised after architectural feedback and improved in the following areas:
- namespace hierarchy
- asmdef modularization
- state machine separation
- interface-based service contracts
- singleton cleanup in active gameplay flow
- EditMode and PlayMode tests
- scene validation tests and editor validation tooling
- folder / namespace / project hygiene cleanup

## Main Architectural Improvements
### Contracts-first structure
The project now separates core contracts into dedicated interfaces under `Contracts/`:
- state machine contracts
- reward contracts
- service contracts

### State-driven gameplay flow
Gameplay flow is now organized with:
- `GameStateMachine`
- `GameCommandCoordinator`
- `GameRewardFlowController`
- dedicated state classes (`InitState`, `IdleState`, `SpinningState`, `EvaluationState`, `ClaimState`, `GameOverState`)

### UI and Wheel refactor
Large manager classes were decomposed into smaller controller/facade structures:
- `UIManager` + UI controllers
- `WheelManager` + wheel controllers

### Reward resolution pipeline
Reward handling was separated into:
- `RewardResolver`
- `BombRewardResolutionHandler`
- `CollectibleRewardResolutionHandler`

## Project Structure
```text
Assets/
  Scripts/
    Contracts/
    Gameplay/
    UI/
    Tools/
  Tests/
    EditMode/
    PlayMode/
  Docs/

