using UnityEngine;

public class SwordDash : MonoBehaviour
{
    public float minDashForce = 5f;
    public float maxDashForce = 20f;
    public float chargeTime = 2f; // Maximale Ladezeit in Sekunden
    public Rigidbody rb;
    public KeyCode chargeKey = KeyCode.Mouse1;
    public Transform playerCamera; // Kamera-Referenz für Blickrichtung

    private bool isCharging = false;
    private float chargeStartTime;

    void Update()
    {
        if (Input.GetKeyDown(chargeKey))
        {
            StartCharging();
        }
        if (Input.GetKeyUp(chargeKey))
        {
            PerformDash();
        }
    }

    void StartCharging()
    {
        isCharging = true;
        chargeStartTime = Time.time;
    }

    void PerformDash()
    {
        if (!isCharging) return;

        isCharging = false;
        float chargeDuration = Mathf.Clamp(Time.time - chargeStartTime, 0, chargeTime);
        float dashForce = Mathf.Lerp(minDashForce, maxDashForce, chargeDuration / chargeTime);

        Vector3 dashDirection = playerCamera.transform.forward; // Blickrichtung des Spielers inkl. vertikale Richtung
        dashDirection.Normalize();

        rb.velocity = Vector3.zero; // Zurücksetzen der Geschwindigkeit
        rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);
    }
}