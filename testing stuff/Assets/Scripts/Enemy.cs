using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 50f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Enemy took " + amount + " damage! Health left: " + health);

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);  // Entfernt den Gegner aus der Szene
    }
}
