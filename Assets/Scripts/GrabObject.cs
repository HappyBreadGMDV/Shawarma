using UnityEngine;

[DisallowMultipleComponent]
public class GrabObject : MonoBehaviour
{
    [Header("Точка захвата на объекте")]
    public Transform grabTransform;

    [Header("Тип захвата")]
    public GrabType grabType = GrabType.Dynamic;

    public enum GrabType
    {
        Static,
        Dynamic
    }

    public Rigidbody CachedRigidbody { get; private set; }

    private void Awake()
    {
        CachedRigidbody = GetComponent<Rigidbody>();

        if (CachedRigidbody == null)
            CachedRigidbody = GetComponentInParent<Rigidbody>();

        if (grabTransform == null)
            grabTransform = transform;
    }
}