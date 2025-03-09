using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float bulletLifetime = 5f;    // Wie lange die Kugel existiert, bevor sie automatisch zerst�rt wird
    public float damage = 10f;           // Schaden, den die Kugel verursacht
    public GameObject impactEffect;      // Optional: Ein Partikel oder Effekt, der beim Aufprall gezeigt wird

    private void Start()
    {
        // Zerst�re die Kugel nach einer bestimmten Zeit, falls sie nichts trifft
        Destroy(gameObject, bulletLifetime);
    }

    void OnCollisionEnter(Collision collision)  // 3D Kollision
    {
        Debug.Log("Bullet hit: " + collision.gameObject.name); // Debug-Log zum Testen

        // Optional: Zeige einen Impact-Effekt
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        // Schaden verursachen, wenn ein Gegner getroffen wird
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        // Zerst�re die Kugel nach dem Treffer
        Destroy(gameObject);
    }
}
