using UnityEngine;

// Attachez ce script aux ancres ou visualiseurs pour suivre leurs mouvements
public class TransformDebugger : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;

    private Transform initialParent;

    [SerializeField] private bool logChanges = true;
    [SerializeField] private string objectDescription;

    private void Awake()
    {
        objectDescription = gameObject.name;
        CaptureInitialState();
    }

    private void CaptureInitialState()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
        initialParent = transform.parent;

        Debug.Log($"[{objectDescription}] Initial state captured at Awake:");
        LogCurrentState();
    }

    private void Update()
    {
        if (!logChanges) return;

        bool positionChanged = Vector3.Distance(initialPosition, transform.position) > 0.01f;
        bool rotationChanged = Quaternion.Angle(initialRotation, transform.rotation) > 0.1f;
        bool localPositionChanged = Vector3.Distance(initialLocalPosition, transform.localPosition) > 0.01f;
        bool localRotationChanged = Quaternion.Angle(initialLocalRotation, transform.localRotation) > 0.1f;
        bool parentChanged = initialParent != transform.parent;

        if (positionChanged || rotationChanged || localPositionChanged || localRotationChanged || parentChanged)
        {
            Debug.Log($"[{objectDescription}] Transform changed in Update!");
            LogCurrentState();

            if (parentChanged)
            {
                Debug.Log($"[{objectDescription}] Parent changed from {initialParent?.name ?? "null"} to {transform.parent?.name ?? "null"}");
            }

            // Recapture l'état pour ne pas spammer les logs
            CaptureInitialState();
        }
    }

    private void LogCurrentState()
    {
        Debug.Log($"[{objectDescription}] Position: {transform.position}");
        Debug.Log($"[{objectDescription}] Rotation: {transform.rotation.eulerAngles}");
        Debug.Log($"[{objectDescription}] Local Position: {transform.localPosition}");
        Debug.Log($"[{objectDescription}] Local Rotation: {transform.localRotation.eulerAngles}");
    }

    // Appelé quand un script modifie la position dans l'éditeur
    private void OnValidate()
    {
        objectDescription = string.IsNullOrEmpty(objectDescription) ? gameObject.name : objectDescription;
    }
}