//using System.Collections.Generic;
//using UnityEngine;

//public class GrabManager : MonoBehaviour
//{
//    [Header("Raycast")]
//    public float rayDistance = 3f;
//    public LayerMask grabLayer;

//    [Header("Hold points")]
//    public Transform grabTransform;
//    public Rigidbody grabPointRb;

//    [Header("Keys")]
//    public KeyCode grabKey = KeyCode.E;
//    public KeyCode dropKey = KeyCode.Q;

//    private bool isGrabbed;
//    private GrabObject currentGrab;
//    private FixedJoint currentJoint;

//    private readonly Dictionary<Collider, bool> colliderStates = new Dictionary<Collider, bool>();

//    private void Awake()
//    {
//        if (grabTransform == null)
//            grabTransform = transform;

//        if (grabPointRb == null && grabTransform != null)
//            grabPointRb = grabTransform.GetComponent<Rigidbody>();
//    }

//    private void Update()
//    {
//        // STATIC: захват по одиночному нажатию E
//        if (Input.GetKeyDown(grabKey) && !isGrabbed)
//        {
//            if (TryGetTarget(out GrabObject target) && target.grabType == GrabObject.GrabType.Static)
//            {
//                GrabSpecificObject(target);
//                isGrabbed = true;
//            }
//        }

//        // DYNAMIC: захват тоже только по нажатию E (не при удержании)
//        if (Input.GetKeyDown(grabKey) && !isGrabbed)
//        {
//            if (currentGrab == null && TryGetTarget(out GrabObject target) && target.grabType == GrabObject.GrabType.Dynamic)
//            {
//                GrabSpecificObject(target);
//                isGrabbed = true;
//            }
//        }

//        // DYNAMIC: отпускание при отжатии E
//        if (Input.GetKeyUp(grabKey))
//        {
//            if (currentGrab != null && currentGrab.grabType == GrabObject.GrabType.Dynamic)
//            {
//                Drop();
//                isGrabbed = false;
//            }
//            // Для статики isGrabbed не сбрасываем, чтобы нельзя было взять второй предмет
//        }

//        // Сброс по отдельной клавише (Q), работает для обоих типов
//        if (Input.GetKeyDown(dropKey))
//        {
//            Drop();
//            isGrabbed = false;
//        }
//    }

//    private bool TryGetTarget(out GrabObject grabObject)
//    {
//        grabObject = null;

//        if (Camera.main == null)
//            return false;

//        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//        if (!Physics.Raycast(ray, out RaycastHit hit, rayDistance, grabLayer))
//            return false;

//        grabObject = hit.collider.GetComponent<GrabObject>();
//        if (grabObject == null)
//            grabObject = hit.collider.GetComponentInParent<GrabObject>();

//        return grabObject != null;
//    }

//    public void GrabSpecificObject(GrabObject grabObject)
//    {
//        if (grabObject == null || currentGrab != null)
//            return;

//        SetObjectTrigger(grabObject.transform, true);

//        switch (grabObject.grabType)
//        {
//            case GrabObject.GrabType.Static:
//                GrabStatic(grabObject);
//                break;

//            case GrabObject.GrabType.Dynamic:
//                GrabDynamic(grabObject);
//                break;
//        }
//    }

//    public void GrabSpecificObject(GameObject obj)
//    {
//        if (obj == null) return;

//        GrabObject grabObject = obj.GetComponent<GrabObject>();
//        if (grabObject == null)
//            grabObject = obj.GetComponentInParent<GrabObject>();

//        if (grabObject != null)
//            GrabSpecificObject(grabObject);
//    }

//    private void GrabStatic(GrabObject obj)
//    {
//        if (obj == null || obj.grabTransform == null)
//            return;

//        Rigidbody rb = obj.CachedRigidbody;
//        if (rb != null)
//        {
//            rb.velocity = Vector3.zero;
//            rb.angularVelocity = Vector3.zero;
//            rb.isKinematic = true;
//        }

//        foreach (Collider col in obj.GetComponentsInChildren<Collider>())
//            col.isTrigger = true;

//        Transform target = grabTransform;
//        Transform pivot = obj.grabTransform;

//        // Сначала выравниваем позицию и поворот относительно руки
//        obj.transform.position = target.position;
//        obj.transform.rotation = target.rotation;

//        // Компенсируем смещение точки захвата
//        Vector3 offset = obj.transform.position - pivot.position;
//        obj.transform.position += offset;

//        // Привязываем
//        obj.transform.SetParent(target);
//        currentGrab = obj;
//    }

//    private void GrabDynamic(GrabObject obj)
//    {
//        if (obj == null) return;

//        Rigidbody rb = obj.CachedRigidbody;
//        if (rb == null) return;

//        if (grabPointRb == null)
//        {
//            Debug.LogWarning("grabPointRb не назначен. Для Dynamic-захвата нужен Rigidbody у точки удержания.");
//            return;
//        }

//        rb.velocity = Vector3.zero;
//        rb.angularVelocity = Vector3.zero;

//        if (currentJoint != null)
//            Destroy(currentJoint);

//        currentJoint = rb.gameObject.AddComponent<FixedJoint>();
//        currentJoint.connectedBody = grabPointRb;
//        currentJoint.breakForce = Mathf.Infinity;
//        currentJoint.breakTorque = Mathf.Infinity;

//        currentGrab = obj;
//    }

//    private void Drop()
//    {
//        if (currentGrab == null) return;

//        switch (currentGrab.grabType)
//        {
//            case GrabObject.GrabType.Static:
//                Rigidbody rb = currentGrab.CachedRigidbody;
//                if (rb != null)
//                    rb.isKinematic = false;
//                currentGrab.transform.SetParent(null);
//                break;

//            case GrabObject.GrabType.Dynamic:
//                if (currentJoint != null)
//                {
//                    Destroy(currentJoint);
//                    currentJoint = null;
//                }
//                break;
//        }

//        SetObjectTrigger(currentGrab.transform, false);
//        currentGrab = null;
//    }

//    private void SetObjectTrigger(Transform root, bool value)
//    {
//        if (root == null) return;

//        Collider[] colliders = root.GetComponentsInChildren<Collider>(true);
//        foreach (Collider col in colliders)
//        {
//            if (col == null) continue;

//            if (value) // Захват: запоминаем и включаем триггер
//            {
//                if (!colliderStates.ContainsKey(col))
//                    colliderStates.Add(col, col.isTrigger);
//                col.isTrigger = true;
//            }
//            else // Отпускание: восстанавливаем исходное состояние
//            {
//                if (colliderStates.TryGetValue(col, out bool oldState))
//                    col.isTrigger = oldState;
//                else
//                    col.isTrigger = false;
//            }
//        }

//        if (!value)
//            colliderStates.Clear();
//    }

//    private void OnDrawGizmosSelected()
//    {
//        if (Camera.main == null) return;

//        Gizmos.color = Color.red;
//        Vector3 direction = Camera.main.transform.forward;
//        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + direction * rayDistance);

//        if (grabTransform != null)
//        {
//            Gizmos.color = Color.yellow;
//            Gizmos.DrawWireCube(grabTransform.position, Vector3.one * 0.25f);
//        }
//    }
//}

using System.Collections.Generic;
using UnityEngine;

public class GrabManager : MonoBehaviour
{
    [Header("Raycast")]
    public float rayDistance = 3f;
    public LayerMask grabLayer;

    [Header("Hold points")]
    public Transform grabTransform;
    public Rigidbody grabPointRb;

    [Header("Keys")]
    public KeyCode grabKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q;

    private bool isGrabbed;
    private GrabObject currentGrab;
    private FixedJoint currentJoint;

    private readonly Dictionary<Collider, bool> colliderStates = new Dictionary<Collider, bool>();

    private void Awake()
    {
        if (grabTransform == null)
            grabTransform = transform;

        if (grabPointRb == null && grabTransform != null)
            grabPointRb = grabTransform.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // АВТОМАТИЧЕСКИЙ СБРОС, ЕСЛИ ПРЕДМЕТ УНИЧТОЖЕН
        if (isGrabbed && currentGrab == null)
        {
            ForceRelease();
        }

        // STATIC: захват по одиночному нажатию E
        if (Input.GetKeyDown(grabKey) && !isGrabbed)
        {
            if (TryGetTarget(out GrabObject target) && target.grabType == GrabObject.GrabType.Static)
            {
                GrabSpecificObject(target);
                isGrabbed = true;
            }
        }

        // DYNAMIC: захват тоже только по нажатию E
        if (Input.GetKeyDown(grabKey) && !isGrabbed)
        {
            if (currentGrab == null && TryGetTarget(out GrabObject target) && target.grabType == GrabObject.GrabType.Dynamic)
            {
                GrabSpecificObject(target);
                isGrabbed = true;
            }
        }

        // DYNAMIC: отпускание при отжатии E
        if (Input.GetKeyUp(grabKey))
        {
            if (currentGrab != null && currentGrab.grabType == GrabObject.GrabType.Dynamic)
            {
                Drop();
                isGrabbed = false;
            }
        }

        // Сброс по отдельной клавише (Q), работает для обоих типов
        if (Input.GetKeyDown(dropKey))
        {
            Drop();
            isGrabbed = false;
        }
    }

    private bool TryGetTarget(out GrabObject grabObject)
    {
        grabObject = null;

        if (Camera.main == null)
            return false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, rayDistance, grabLayer))
            return false;

        grabObject = hit.collider.GetComponent<GrabObject>();
        if (grabObject == null)
            grabObject = hit.collider.GetComponentInParent<GrabObject>();

        return grabObject != null;
    }

    public void GrabSpecificObject(GrabObject grabObject)
    {
        if (grabObject == null || currentGrab != null)
            return;

        SetObjectTrigger(grabObject.transform, true);

        switch (grabObject.grabType)
        {
            case GrabObject.GrabType.Static:
                GrabStatic(grabObject);
                break;

            case GrabObject.GrabType.Dynamic:
                GrabDynamic(grabObject);
                break;
        }
    }

    public void GrabSpecificObject(GameObject obj)
    {
        if (obj == null) return;

        GrabObject grabObject = obj.GetComponent<GrabObject>();
        if (grabObject == null)
            grabObject = obj.GetComponentInParent<GrabObject>();

        if (grabObject != null)
            GrabSpecificObject(grabObject);
    }

    private void GrabStatic(GrabObject obj)
    {
        if (obj == null || obj.grabTransform == null)
            return;

        Rigidbody rb = obj.CachedRigidbody;
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        foreach (Collider col in obj.GetComponentsInChildren<Collider>())
            col.isTrigger = true;

        Transform target = grabTransform;
        Transform pivot = obj.grabTransform;

        obj.transform.position = target.position;
        obj.transform.rotation = target.rotation;

        Vector3 offset = obj.transform.position - pivot.position;
        obj.transform.position += offset;

        obj.transform.SetParent(target);
        currentGrab = obj;
    }

    private void GrabDynamic(GrabObject obj)
    {
        if (obj == null) return;

        Rigidbody rb = obj.CachedRigidbody;
        if (rb == null) return;

        if (grabPointRb == null)
        {
            Debug.LogWarning("grabPointRb не назначен. Для Dynamic-захвата нужен Rigidbody у точки удержания.");
            return;
        }

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (currentJoint != null)
            Destroy(currentJoint);

        currentJoint = rb.gameObject.AddComponent<FixedJoint>();
        currentJoint.connectedBody = grabPointRb;
        currentJoint.breakForce = Mathf.Infinity;
        currentJoint.breakTorque = Mathf.Infinity;

        currentGrab = obj;
    }

    private void Drop()
    {
        if (currentGrab == null) return;

        switch (currentGrab.grabType)
        {
            case GrabObject.GrabType.Static:
                Rigidbody rb = currentGrab.CachedRigidbody;
                if (rb != null)
                    rb.isKinematic = false;
                currentGrab.transform.SetParent(null);
                break;

            case GrabObject.GrabType.Dynamic:
                if (currentJoint != null)
                {
                    Destroy(currentJoint);
                    currentJoint = null;
                }
                break;
        }

        SetObjectTrigger(currentGrab.transform, false);
        currentGrab = null;
    }

    private void ForceRelease()
    {
        // Убираем джоинт, если остался
        if (currentJoint != null)
        {
            Destroy(currentJoint);
            currentJoint = null;
        }

        // Обнуляем ссылки и флаг, не трогая уничтоженный объект
        currentGrab = null;
        isGrabbed = false;
        colliderStates.Clear();
    }

    private void SetObjectTrigger(Transform root, bool value)
    {
        if (root == null) return;

        Collider[] colliders = root.GetComponentsInChildren<Collider>(true);
        foreach (Collider col in colliders)
        {
            if (col == null) continue;

            if (value)
            {
                if (!colliderStates.ContainsKey(col))
                    colliderStates.Add(col, col.isTrigger);
                col.isTrigger = true;
            }
            else
            {
                if (colliderStates.TryGetValue(col, out bool oldState))
                    col.isTrigger = oldState;
                else
                    col.isTrigger = false;
            }
        }

        if (!value)
            colliderStates.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        if (Camera.main == null) return;

        Gizmos.color = Color.red;
        Vector3 direction = Camera.main.transform.forward;
        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + direction * rayDistance);

        if (grabTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(grabTransform.position, Vector3.one * 0.25f);
        }
    }
}