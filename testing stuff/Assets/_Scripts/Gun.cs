using UnityEngine;

public class GunScript : MonoBehaviour
{
    public GameObject impactEffect;          // Optional: Ein Partikel oder Effekt für den Treffer
    public float bulletSpeed = 100f;         // Geschwindigkeit des Raycasts
    public float range = 100f;              // Reichweite des Raycasts
    public float damage = 10f;              // Schaden des Schusses
    public AudioClip shootSound;            // Optional: Schuss-Sound
    public Transform bulletSpawnPoint;      // Der Punkt, an dem der Schuss abgefeuert wird
    public GameObject trailPrefab;          // Trail Prefab für den Schuss

    private GameObject currentTrail;        // Referenz zum aktuellen Trail

    void Update()
    {
        // Wenn die Schusstaste gedrückt wird
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
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
                Destroy(currentTrail, 0.1f);  // Zerstöre den Trail nach kurzer Zeit
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
