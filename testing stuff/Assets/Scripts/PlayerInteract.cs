using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactDistance = 3f;
    public LayerMask interactLayer;
    public TextMeshProUGUI interactText;

    private void Update()
    {
        CheckForInteractable();
    }
    void CheckForInteractable()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactText.text = "Press [F] to interact";
                interactable.enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    interactable.Interact();
                }
            }
        }
        else
        {
            interactText.enabled = false;
        }
    }
}
