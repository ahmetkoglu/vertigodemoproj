# PlayMode Test Notes

Bu klasördeki PlayMode testleri iki kategoriye ayrılıyor:

1. **Runtime interaction / lightweight integration tests**
   - `UIInputControllerPlayModeTests`
   - `UIToGameplayFlowPlayModeTests`
   - `WheelSpinControllerPlayModeTests`
   - `CollectibleRewardResolutionHandlerPlayModeTests`
   - `BombRewardResolutionHandlerPlayModeTests`

2. **Scene validation tests**
   - `SceneValidationPlayModeTests`

## Mevcut scene validation kapsamı

`SceneValidationPlayModeTests` şu kritik wiring noktalarını doğrular:

- `SampleScene` yüklenebiliyor mu
- `GameManager`, `UIManager`, `WheelManager`, `ZoneManager`, `InventoryManager` sahnede mevcut mu
- `GameManager` içindeki manager referansları dolu mu
- `UIManager` içindeki kritik button, panel, canvas ve prefab referansları dolu mu
- `WheelManager` içindeki image/container/prefab referansları dolu mu
- `ZoneManager` içindeki zone data referansları dolu mu

## Sonraki önerilen testler

- Full scene flow: `Idle -> Spinning -> Evaluation`
- Reward resolve sonrası gerçek scene üstünde inventory/progression doğrulaması
- UI popup görünürlük akışlarının gerçek scene ile doğrulanması