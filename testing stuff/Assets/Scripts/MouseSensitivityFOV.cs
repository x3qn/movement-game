using UnityEngine;

public class MouseSensitivityFOV : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The base FOV of the camera.")]
    public float baseFOV = 90f;  // Standard-FOV
    [Tooltip("The base mouse sensitivity.")]
    public float baseSensitivity = 1f;  // Standard-Maus-Sensitivität
    private float currentSensitivity;

    private Camera cam;

    void Start()
    {
        // Initialisiere die Kamera
        cam = Camera.main;

        if (cam == null)
        {
            Debug.LogError("Keine Kamera gefunden!");
            return;
        }
    }

    void Update()
    {
        // Dynamische Anpassung der Maus-Sensitivität basierend auf dem aktuellen FOV
        UpdateSensitivity();
        HandleMouseLook();
    }

    void UpdateSensitivity()
    {
        // Berechne den Skalierungsfaktor basierend auf dem aktuellen FOV
        float fovScale = cam.fieldOfView / baseFOV;
        currentSensitivity = baseSensitivity * fovScale;
    }

    void HandleMouseLook()
    {
        // Mausbewegung mit der dynamisch berechneten Sensitivität
        float mouseX = Input.GetAxis("Mouse X") * currentSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * currentSensitivity;

        // Drehung der Kamera um die Y-Achse (für horizontale Bewegung)
        transform.Rotate(Vector3.up * mouseX);

        // Drehung der Kamera um die X-Achse (für vertikale Bewegung)
        cam.transform.Rotate(Vector3.left * mouseY);
    }
}
