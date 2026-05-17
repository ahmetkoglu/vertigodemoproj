using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIEffectManager : MonoBehaviour
{
    public static UIEffectManager Instance { get; private set; }

    [Header("VFX Prefabs")]
    [SerializeField] private GameObject flashVfxPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Plays the flight animation of an icon from the wheel to the target inventory slot.
    /// Utilizes Object Pooling to avoid instantiation costs.
    /// </summary>
    public void PlayCollectAnimation(Canvas canvas, Sprite icon, Vector3 startWorldPos, RectTransform targetSlot, System.Action onComplete)
    {
        if (canvas == null)
        {
            onComplete?.Invoke();
            return;
        }

        // 1. Fetch from Pool
        GameObject flyer = IconPool.Instance.GetIcon(canvas.transform);
        RectTransform flyerRect = flyer.GetComponent<RectTransform>();
        
        // Reset Transforms
        flyerRect.position = startWorldPos;
        flyerRect.localScale = Vector3.one; 
        flyerRect.localRotation = Quaternion.identity; 
        
        Vector3 tempPos = flyerRect.localPosition;
        tempPos.z = 0f;
        flyerRect.localPosition = tempPos;

        flyer.GetComponent<Image>().sprite = icon;

        // 2. Define Target
        Vector3 targetPos = targetSlot != null ? targetSlot.position : Vector3.zero;

        // 3. Animation Sequence
        Sequence s = DOTween.Sequence();
        s.Append(flyerRect.DOScale(Vector3.one * 1.5f, 0.3f).SetEase(Ease.OutBack));
        s.Append(flyerRect.DOMove(targetPos, 0.6f).SetEase(Ease.InQuad));
        s.Join(flyerRect.DOScale(Vector3.one * 0.4f, 0.6f));
        s.Join(flyerRect.DORotate(new Vector3(0, 0, 360), 0.6f, RotateMode.FastBeyond360));

        // 4. On Complete Actions
        s.OnComplete(() =>
        {
            if (flyer != null) IconPool.Instance.ReturnIcon(flyer);
            PlayTargetFlashEffect(targetSlot);
            onComplete?.Invoke();
        });

        // Fail-safe in case DOTween gets stuck
        DOVirtual.DelayedCall(1.2f, () => {
            if (flyer != null && flyer.activeInHierarchy) 
            {
                IconPool.Instance.ReturnIcon(flyer);
                onComplete?.Invoke();
            }
        });
    }

    /// <summary>
    /// Instantiates and plays a brief flash effect on the target slot.
    /// </summary>
    private void PlayTargetFlashEffect(RectTransform targetSlot)
    {
        if (flashVfxPrefab != null && targetSlot != null)
        {
            GameObject flash = Instantiate(flashVfxPrefab, targetSlot);
            flash.transform.localPosition = Vector3.zero;
            flash.transform.localScale = Vector3.zero;    

            flash.transform.DOScale(Vector3.one * 1.5f, 0.3f).SetEase(Ease.OutQuint);
            flash.GetComponent<Image>().DOFade(0f, 0.3f).SetEase(Ease.InExpo).OnComplete(() =>
            {
                Destroy(flash); 
            });
        }
    }

    /// <summary>
    /// Plays the tension animation (Screen shake & Red flashing) when a bomb is hit.
    /// </summary>
    public void PlayBombTensionAnimation(Image backgroundPanel, System.Action onComplete)
    {
        Sequence s = DOTween.Sequence();
        
        if (Camera.main != null)
        {
            s.Join(Camera.main.transform.DOShakePosition(0.8f, 0.3f, 20, 90f, false, true));
        }
        // -----------------------------------

        if (backgroundPanel != null)
        {
            Color originalColor = backgroundPanel.color;
            s.Join(backgroundPanel.DOColor(Color.red, 0.15f).SetLoops(6, LoopType.Yoyo));
            
            s.OnComplete(() => {
                backgroundPanel.color = originalColor; 
                onComplete?.Invoke();
            });
        }
        else
        {
            s.OnComplete(() => onComplete?.Invoke());
        }
    }
}