using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    public Camera playerCamera;
    public LineRenderer lineRenderer;
    public LayerMask grappleLayer;
    public Transform gunTip;
    public float maxGrappleDistance = 20f;
    public float springForce = 5f;
    public float damper = 5f;
    public float massScale = 4.5f;

    private SpringJoint joint;
    private Vector3 grapplePoint;
    private Rigidbody rb;
    private bool isGrappling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Linke Maustaste zum Greifen
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }

        if (isGrappling)
        {
            lineRenderer.SetPosition(0, gunTip.position);
        }
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, maxGrappleDistance, grappleLayer))
        {
            grapplePoint = hit.point;
            isGrappling = true;

            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distance = Vector3.Distance(transform.position, grapplePoint);
            joint.maxDistance = distance * 0.8f;
            joint.minDistance = distance * 0.25f;

            joint.spring = springForce;
            joint.damper = damper;
            joint.massScale = massScale;

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, gunTip.position);
            lineRenderer.SetPosition(1, grapplePoint);
        }
    }

    void StopGrapple()
    {
        isGrappling = false;
        lineRenderer.enabled = false;
        Destroy(joint);
    }
}
