using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class IconPool : MonoBehaviour
{
    public static IconPool Instance { get; private set; }

    [Header("Pool Settings")]
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private int initialPoolSize = 10;

    // The queue holding the inactive objects ready to be reused.
    private Queue<GameObject> _pool = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Pre-warm the pool to avoid instantiation spikes during gameplay
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewIcon();
        }
    }

    /// <summary>
    /// Instantiates a new icon, disables it, and adds it to the pool.
    /// </summary>
    /// <returns>The newly created icon GameObject.</returns>
    private GameObject CreateNewIcon()
    {
        GameObject obj = Instantiate(iconPrefab, transform);
        obj.SetActive(false);
        _pool.Enqueue(obj);
        return obj;
    }

    /// <summary>
    /// Retrieves an icon from the pool. If the pool is empty, it creates a new one.
    /// Replaces the costly Instantiate() method.
    /// </summary>
    /// <param name="targetCanvas">The canvas where the icon will be parented.</param>
    /// <returns>An active icon GameObject.</returns>
    public GameObject GetIcon(Transform targetCanvas)
    {
        GameObject obj = _pool.Count > 0 ? _pool.Dequeue() : CreateNewIcon();

        obj.transform.SetParent(targetCanvas);
        obj.SetActive(true);
        return obj;
    }

    /// <summary>
    /// Returns an icon to the pool, cleaning up its state and active animations.
    /// Replaces the costly Destroy() method.
    /// </summary>
    /// <param name="obj">The icon GameObject to return.</param>
    public void ReturnIcon(GameObject obj)
    {
        // CRITICAL: Kill any lingering DOTween animations on the object before pooling
        obj.transform.DOKill(); 
        
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        _pool.Enqueue(obj);
    }
}