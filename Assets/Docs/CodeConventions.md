# WheelGame Code Conventions

Bu doküman, proje içinde uygulanan klasörleme, namespace, asmdef, region ve validation/test yaklaşımını özetler.

## 1. Folder / Domain Organization

Kod tabanı aşağıdaki ana alanlara ayrılır:

```text
Assets/Scripts/Contracts
Assets/Scripts/Gameplay
Assets/Scripts/UI
Assets/Scripts/Tools
Assets/Tests
```

### Amaç
- `Contracts`: ortak interface’ler, enum’lar ve servis sözleşmeleri
- `Gameplay`: oyun akışı, state machine, progression, rewards, wheel, inventory
- `UI`: facade, controller, component, effect ve pooling katmanı
- `Tools`: yardımcı editor/runtime araçları
- `Tests`: EditMode ve PlayMode testleri

## 2. Namespace Convention

Namespace’ler klasör yapısını yansıtmalıdır.

### Örnekler

```csharp
namespace WheelGame.Contracts.StateMachine
namespace WheelGame.Gameplay.Management
namespace WheelGame.Gameplay.Progression
namespace WheelGame.UI.Controllers
namespace WheelGame.UI.Pooling
namespace WheelGame.Tools.Editor
```

### Kural
- Dosya yolu ile namespace mümkün olduğunca paralel olmalı
- Global namespace kullanılmamalı
- Eski/arsiv scriptler aktif kod alanından ayrılmalı

## 3. Assembly Definition (asmdef) Convention

Asmdef yapısı modülerlik, compile süreleri ve dependency direction için kullanılır.

### Temel prensip

```text
Contracts -> bağımsız taban katman
Gameplay -> Contracts'a bağımlı
UI -> Contracts'a ve ilgili UI assembly’lerine bağımlı
Tools.Editor -> sadece Editor platformunda çalışır
Tests -> ilgili runtime assembly’lerine bağımlı
```

### Hedef dependency yönü

```text
Contracts
  ↓
Gameplay / UI
  ↓
Tests
```

### Kural
- Bir assembly başka assembly’deki bir tipe doğrudan dokunuyorsa, referansı doğrudan asmdef içine eklenmeli
- Transitive dependency varsayımı yapılmamalı

## 4. Region Usage Convention

`#region` sadece büyük veya birden fazla mantıksal bölümü olan sınıflarda kullanılmalıdır.

### Uygun adaylar
- Facade sınıflar
- Orchestration / manager sınıfları
- Editor tool’lar
- Büyük test dosyaları

### Uygun olmayan adaylar
- Küçük, tek sorumluluklu sınıflar
- Basit handler’lar
- Kısa utility sınıfları

### Önerilen region başlıkları

```text
Serialized References
Events
Runtime Services
Runtime Controllers
Unity Lifecycle
I<GameInterface>
Editor Validation
Test Helpers
Scene Setup
Validation Helpers
```

## 5. Scene / Inspector Validation Approach

Eksik referansların runtime’a kalmaması için üç seviyeli yaklaşım kullanılır:

### 1. OnValidate
- Kritik MonoBehaviour ve ScriptableObject sınıflarında eksik referans uyarıları

### 2. Editor Tool
- `Tools/WheelGame/Validate Active Scene References`
- aktif scene içindeki kritik wiring’i tarar

### 3. PlayMode Validation Tests
- `SceneValidationPlayModeTests`
- sahne yükleme, manager varlığı ve serialized field wiring doğrulaması

## 6. Test Structure

### EditMode
Pure gameplay logic ve interface tabanlı akış testleri için kullanılır.

Örnekler:
- state machine
- command coordinator
- reward resolver
- progression logic
- inventory logic

### PlayMode
Runtime interaction, scene wiring ve integration akışları için kullanılır.

Örnekler:
- button click -> state transition
- wheel spin -> completion flow
- reward resolve -> inventory/progression/UI flow
- scene validation
- full scene boot flow

## 7. General Coding Principles

- State’ler concrete manager’lara değil `IGameContext` üzerinden servis interface’lerine erişmelidir
- UI tarafında facade (`UIManager`) ile alt controller sorumlulukları ayrılmalıdır
- Wheel tarafında facade (`WheelManager`) ile alt controller sorumlulukları ayrılmalıdır
- Reward çözümleme akışı `RewardResolver` + handler yapısıyla genişletilebilir tutulmalıdır
- Singleton bağımlılıklarından kaçınılmalıdır

## 8. Project Hygiene

- Arşiv/legacy scriptler aktif production kodundan ayrılmalı
- Klasör / namespace uyumsuzlukları düzeltilmeli
- Boş veya duplicate script dosyaları temizlenmeli
- Yeni testler ve editor araçları ilgili asmdef sınırları içinde tutulmalı
