using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRange = 2;
    [SerializeField] private LayerMask interactionLayer;
    void Start()
    {
        InteractInCircle();
    }

    /// <summary>
    /// Checks around the player for any interactable objects, and interacts with them
    /// </summary>
    public void InteractInCircle()
    {
        //Get all colliders near
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange,interactionLayer);

        for(int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].gameObject.TryGetComponent<InteractableObject>(out InteractableObject currentObject))
            {
                currentObject.Interact();
            }
        }
    }
}
