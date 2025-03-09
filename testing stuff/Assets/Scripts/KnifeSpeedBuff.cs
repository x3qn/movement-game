using UnityEngine;

public class KnifeSpeedBuff : MonoBehaviour
{
    [Header("Knife Settings")]
    public GameObject knifeObject;         // Das Messer-Objekt, das per Inspector zugewiesen wird
    public float speedBuff = 2.0f;         // Der Buff, der angewendet wird, wenn das Messer aktiv ist
    public float defaultSpeed = 7.0f;      // Die normale Bewegungsgeschwindigkeit des Spielers

    private CPMPlayer playerMovement;      // Referenz zum Player-Movement-Skript

    void Start()
    {
        // Hole die Referenz auf das CPMPlayer-Skript
        playerMovement = GetComponent<CPMPlayer>();

        if (knifeObject == null)
        {
            Debug.LogWarning("KnifeObject ist nicht zugewiesen!");
        }
    }

    void Update()
    {
        // Überprüfe, ob das Messer aktiv ist und setze die Geschwindigkeit des Spielers entsprechend
        if (knifeObject != null && knifeObject.activeSelf)
        {
            ApplySpeedBuff();   // Speed-Buff anwenden, wenn das Messer aktiv ist
        }
        else
        {
            ResetSpeed();       // Geschwindigkeit zurücksetzen, wenn das Messer nicht aktiv ist
        }
    }

    // Methode zum Anwenden des Geschwindigkeitsschubs
    void ApplySpeedBuff()
    {
        if (playerMovement != null)
        {
            playerMovement.moveSpeed = defaultSpeed * speedBuff;  // Erhöhe die Geschwindigkeit des Spielers
        }
    }

    // Methode zum Zurücksetzen der Geschwindigkeit auf den Standardwert
    void ResetSpeed()
    {
        if (playerMovement != null)
        {
            playerMovement.moveSpeed = defaultSpeed;  // Setze die Geschwindigkeit auf den normalen Wert zurück
        }
    }
}
