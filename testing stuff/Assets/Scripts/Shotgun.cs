using UnityEngine;

public class ShotgunScript : MonoBehaviour
{
    public GameObject impactEffect;           // Effekt f�r den Treffer
    public int numberOfBullets = 8;           // Anzahl der Kugeln pro Schuss
    public float spreadAngle = 10f;           // Streuung der Sch�sse (Grad)
    public float range = 100f;                // Reichweite des Schusses
    public float damage = 10f;                // Schaden des Schusses
    public AudioClip shootSound;              // Schuss-Sound
    public Transform bulletSpawnPoint;        // Punkt, an dem der Schuss abgefeuert wird
    public float recoilForce = 5f;            // R�cksto�kraft des Schusses
    public Rigidbody rb;                      // Rigidbody des Spielers
    public bool canHitSelf = false;           // Ob der Spieler sich selbst treffen kann (im Inspector ausw�hlbar)
    public GameObject playerObject;           // Das Spieler-Objekt, das im Inspector zugewiesen wird
    public float fireRate = 1f;               // Verz�gerung zwischen den Sch�ssen (Sekunden)
    private float nextFireTime = 0f;          // Zeitpunkt, wann der n�chste Schuss erlaubt ist

    void Start()
    {
        // Sicherstellen, dass das Rigidbody zugewiesen ist
        if (rb == null && playerObject != null)
        {
            rb = playerObject.GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        // Wenn die Schusstaste gedr�ckt wird und die Verz�gerung eingehalten wurde
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate; // Verz�gerung setzen
        }
    }

    void Shoot()
    {
        // Schuss-Sound abspielen
        if (shootSound != null)
        {
            AudioSource.PlayClipAtPoint(shootSound, bulletSpawnPoint.position);
        }

        // Berechnungen f�r das Schie�en im Kreis-Muster
        for (int i = 0; i < numberOfBullets; i++)
        {
            float angleStep = 360f / numberOfBullets;
            float currentAngle = i * angleStep;
            Vector3 spread = Quaternion.Euler(0, currentAngle, 0) * bulletSpawnPoint.forward;

            RaycastHit hit;
            Ray ray = new Ray(bulletSpawnPoint.position, spread);

            if (Physics.Raycast(ray, out hit, range))
            {
                // �berpr�fung, ob der Spieler sich selbst treffen kann
                if (!canHitSelf && hit.transform.gameObject == playerObject)
                {
                    continue;
                }

                Debug.Log("Treffer! Ziel: " + hit.transform.name);

                if (impactEffect != null)
                {
                    Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }

                if (hit.transform.CompareTag("Enemy"))
                {
                    Enemy enemy = hit.transform.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damage);
                    }
                }
            }
        }

        // R�cksto� anwenden
        ApplyRecoil();
    }

    void ApplyRecoil()
    {
        if (rb != null)
        {
            Vector3 recoilDirection = -bulletSpawnPoint.forward * recoilForce;
            rb.AddForce(recoilDirection, ForceMode.Impulse);
        }
    }
}
