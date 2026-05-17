using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// Isolated feedback utility that hooks directly into Unity pointer pipelines to trigger spring-back tactile scaling motions on interaction.
/// </summary>
public class UIButtonJuice : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Target Configurations")]
    [Tooltip("The nested layout child transform asset slated for scaling actions.")]
    [SerializeField] private Transform visualTransform; 
    
    private Vector3 _originalScale;

    private void Start()
    {
        // Fail-safe fallback: Automatically crawl and default to the first immediate child index slot if none is supplied
        if (visualTransform == null && transform.childCount > 0) 
        {
            visualTransform = transform.GetChild(0);
        }
            
        _originalScale = visualTransform != null ? visualTransform.localScale : Vector3.one;
    }

    /// <summary>
    /// Triggered automatically on active screen pointer compression intervals. Snaps local scale structural limits down by 10%.
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (visualTransform == null) return;
        
        visualTransform.DOKill(); 
        visualTransform.DOScale(_originalScale * 0.9f, 0.1f).SetEase(Ease.OutQuad);
    }

    /// <summary>
    /// Triggered automatically when compression constraints release. Springs scale layouts comfortably back to their default limits.
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (visualTransform == null) return;
        
        visualTransform.DOKill();
        visualTransform.DOScale(_originalScale, 0.2f).SetEase(Ease.OutBack);
    }
}