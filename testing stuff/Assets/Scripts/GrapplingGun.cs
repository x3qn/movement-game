using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    public Camera playerCamera;                  // Kamera des Spielers
    public LineRenderer lineRenderer;            // Linie für die Grapple-Darstellung
    public LayerMask grappleLayer;               // Layer, auf denen der Grapple stattfinden kann
    public Transform gunTip;                     // Die Spitze der Grappling Gun
    public float maxGrappleDistance = 20f;       // Maximale Distanz für das Grapple
    public float springForce = 5f;               // Federkraft für das SpringJoint
    public float damper = 5f;                    // Dämpfung des SpringJoints
    public float massScale = 4.5f;               // Massenskalierung für das SpringJoint
    public Rigidbody playerRb;                   // Rigidbody des Spielers

    private SpringJoint joint;                   // SpringJoint, das den Grapple-Effekt steuert
    private Vector3 grapplePoint;                // Der Punkt, an dem das Grapple ansetzt
    private bool isGrappling = false;            // Gibt an, ob gerade gegrabbelt wird

    void Start()
    {
        if (playerRb == null)
        {
            playerRb = GetComponentInParent<Rigidbody>();  // Zuweisung des Rigidbodies des Spielers
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Linke Maustaste zum Greifen
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0)) // Maustaste loslassen zum Stoppen
        {
            StopGrapple();
        }

        if (isGrappling)
        {
            lineRenderer.SetPosition(0, gunTip.position); // Setzt die Position des LineRenderers
        }
    }

    void StartGrapple()
    {
        RaycastHit hit;
        // Überprüft, ob der Grapple-Punkt in Reichweite ist
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, maxGrappleDistance, grappleLayer))
        {
            grapplePoint = hit.point;
            isGrappling = true;

            // Füge das SpringJoint hinzu, um den Grapple zu ermöglichen
            joint = playerRb.gameObject.AddComponent<SpringJoint>(); // Hier verwenden wir das Rigidbody des Spielers
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            // Berechne die Entfernung zwischen der Waffe und dem Grapple-Punkt
            float distance = Vector3.Distance(transform.position, grapplePoint);
            joint.maxDistance = distance * 0.8f;
            joint.minDistance = distance * 0.25f;

            // Setze die physikalischen Eigenschaften des SpringJoints
            joint.spring = springForce;
            joint.damper = damper;
            joint.massScale = massScale;

            // Aktiviere den LineRenderer, um die Grapple-Linie darzustellen
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, gunTip.position);
            lineRenderer.SetPosition(1, grapplePoint);
        }
    }

    void StopGrapple()
    {
        isGrappling = false;

        // Deaktiviert den LineRenderer und entfernt den SpringJoint
        lineRenderer.enabled = false;
        Destroy(joint);
    }
}
