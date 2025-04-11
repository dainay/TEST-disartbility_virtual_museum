using UnityEngine;
using UnityEngine.InputSystem; // Pour gérer le Player Input

public class PickupYellow : MonoBehaviour
{
    public float raycastRange = 10f; // Distance du Raycast
    public Transform cameraTransform; // Référence de la caméra
    public float holdDistance = 10f; // Distance à laquelle l'objet est tenu

    private GameObject heldObject = null; // Objet actuellement tenu
    private Rigidbody heldObjectRb = null; // Rigidbody de l'objet tenu
    private bool isHoldingObject = false; // Si le joueur tient un objet

    void Update()
    {
        // Si le joueur clique avec la souris pour ramasser ou lâcher un objet
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
                TryPickupObject();
            else
                DropObject();
        }

        // Si un objet est tenu, on le maintient centré et tourné vers la caméra
        if (heldObject != null)
            KeepObjectCentered();
    }

    void TryPickupObject()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, raycastRange))
        {
            if (hit.collider.CompareTag("PickupYellow"))
            {
                heldObject = hit.collider.gameObject;
                heldObjectRb = heldObject.GetComponent<Rigidbody>();

                if (heldObjectRb != null)
                {
                    heldObjectRb.useGravity = false;
                    heldObjectRb.isKinematic = true;
                }

                isHoldingObject = true;
            }
        }
    }

    void KeepObjectCentered()
    {
        // Position : centré et plus haut
        Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * holdDistance + cameraTransform.up * 3f;
        heldObject.transform.position = targetPosition;

        // Rotation : bien orienter le côté face de la pancarte vers la caméra
        Quaternion lookRotation = Quaternion.LookRotation(cameraTransform.forward);
        heldObject.transform.rotation = lookRotation * Quaternion.Euler(0, 180, 0); // Ajustement de 180° en Y
    }

    void DropObject()
    {
        if (heldObjectRb != null)
        {
            heldObjectRb.useGravity = true;
            heldObjectRb.isKinematic = false;
        }

        heldObject = null;
        heldObjectRb = null;
        isHoldingObject = false;
    }
}
