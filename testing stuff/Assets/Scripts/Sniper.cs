using UnityEngine;

public class SniperScript : MonoBehaviour
{
    public GameObject scopeOverlay;       // Das Objekt, das beim Zielen aktiviert wird
    public Camera mainCamera;             // Die Hauptkamera des Spielers
    public float scopedFOV = 15f;         // Sichtfeld beim Zielen
    private float normalFOV;              // Urspr�ngliches Sichtfeld
    private bool isScoped = false;        // Status, ob man gerade scoped
    public float scopeSensitivity = 0.5f; // Empfindlichkeit der Maus im Scope-Modus

    private float normalSensitivity;      // Normale Maus-Sensitivit�t
    private Transform weaponTransform;    // Transform der Waffe, um die Drehung zu steuern

    // GunScript spezifische Variablen
    public GameObject impactEffect;          // Optional: Ein Partikel oder Effekt f�r den Treffer
    public float bulletSpeed = 100f;         // Geschwindigkeit des Raycasts
    public float range = 100f;              // Reichweite des Raycasts
    public float damage = 10f;              // Schaden des Schusses
    public AudioClip shootSound;            // Optional: Schuss-Sound
    public Transform bulletSpawnPoint;      // Der Punkt, an dem der Schuss abgefeuert wird
    public GameObject trailPrefab;          // Trail Prefab f�r den Schuss

    private GameObject currentTrail;        // Referenz zum aktuellen Trail

    void Start()
    {
        if (mainCamera != null)
        {
            normalFOV = mainCamera.fieldOfView;
        }
        if (scopeOverlay != null)
        {
            scopeOverlay.SetActive(false); // Sicherstellen, dass das Overlay zu Beginn deaktiviert ist
        }

        // Speichert die normale Maus-Sensitivit�t, um sie sp�ter wiederherzustellen
        normalSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1f); // Standardwert ist 1, wenn nicht gesetzt

        // Waffe Transform finden
        weaponTransform = transform.Find("Weapon"); // Beispiel: Wenn die Waffe ein Kind des Spielers ist
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Rechtsklick gedr�ckt
        {
            ToggleScope(true);
        }
        else if (Input.GetMouseButtonUp(1)) // Rechtsklick losgelassen
        {
            ToggleScope(false);
        }

        // Wenn man scoped ist, dann die Maus-Sensitivit�t anpassen
        if (isScoped)
        {
            AdjustMouseSensitivity(scopeSensitivity);
        }
        else
        {
            AdjustMouseSensitivity(normalSensitivity);
        }

        // Schie�en, wenn der linke Mausbutton gedr�ckt wird
        if (Input.GetMouseButtonDown(0)) // Linke Maustaste (Schie�en)
        {
            Shoot();
        }
    }

    void ToggleScope(bool state)
    {
        isScoped = state;

        if (scopeOverlay != null)
        {
            scopeOverlay.SetActive(state);
        }

        if (mainCamera != null)
        {
            mainCamera.fieldOfView = state ? scopedFOV : normalFOV;

            // Near Clipping Plane anpassen
            mainCamera.nearClipPlane = state ? 0.01f : 0.3f;
        }

        // Maus Cursor Sperren/Entsperren
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !state; // Cursor unsichtbar beim Zielen
    }

    void AdjustMouseSensitivity(float sensitivity)
    {
        // Mausbewegungen holen und mit der Sensitivit�t multiplizieren
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Horizontale Drehung der Kamera und des K�rpers (Waffe folgt)
        transform.Rotate(0, mouseX, 0); // Drehung des K�rpers (Horizontal)
        if (weaponTransform != null)
        {
            weaponTransform.Rotate(0, mouseX, 0); // Drehung der Waffe (Horizontal)
        }

        // Vertikale Drehung der Kamera (Neigung)
        mainCamera.transform.Rotate(-mouseY, 0, 0); // Drehung der Kamera (Vertikal)
    }

    void Shoot()
    {
        // Schuss-Sound abspielen
        if (shootSound != null)
        {
            AudioSource.PlayClipAtPoint(shootSound, bulletSpawnPoint.position);
        }

        // Trail Renderer erstellen
        if (trailPrefab != null)
        {
            currentTrail = Instantiate(trailPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            // Setze den Trail zu Beginn des Schusses
            currentTrail.transform.position = bulletSpawnPoint.position;
        }

        // Raycast starten
        RaycastHit hit;
        Ray ray = new Ray(bulletSpawnPoint.position, bulletSpawnPoint.forward);

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log("Treffer! Ziel: " + hit.transform.name); // Debug-Ausgabe

            // Optional: Treffer-Effekt erzeugen
            if (impactEffect != null)
            {
                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }

            // Wenn ein Gegner getroffen wird, Schaden an ihm anwenden
            if (hit.transform.CompareTag("Enemy"))
            {
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }

            // Beende den Trail an der Trefferposition
            if (currentTrail != null)
            {
                Destroy(currentTrail, 0.1f);  // Zerst�re den Trail nach kurzer Zeit
            }
        }
        else
        {
            // Wenn kein Treffer erfolgt, beende den Trail
            if (currentTrail != null)
            {
                Destroy(currentTrail, 0.1f);
            }
        }
    }
}
