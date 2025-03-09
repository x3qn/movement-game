using UnityEngine;

public class MouseSensitivityFOV : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The base FOV of the camera.")]
    public float baseFOV = 90f;  // Standard-FOV
    [Tooltip("The base mouse sensitivity.")]
    public float baseSensitivity = 1f;  // Standard-Maus-Sensitivit�t
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
        // Dynamische Anpassung der Maus-Sensitivit�t basierend auf dem aktuellen FOV
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
        // Mausbewegung mit der dynamisch berechneten Sensitivit�t
        float mouseX = Input.GetAxis("Mouse X") * currentSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * currentSensitivity;

        // Drehung der Kamera um die Y-Achse (f�r horizontale Bewegung)
        transform.Rotate(Vector3.up * mouseX);

        // Drehung der Kamera um die X-Achse (f�r vertikale Bewegung)
        cam.transform.Rotate(Vector3.left * mouseY);
    }
}
