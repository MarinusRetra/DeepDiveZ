using UnityEngine;
using UnityEngine.InputSystem;

public class MouseClickManager : MonoBehaviour
{
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private LayerMask interactabeLayer;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerMovement playerMovement;

    private void OnEnable()
    {
        playerInput.actions["LMB"].performed += (InputAction.CallbackContext ctx) => MouseClicked();
    }

    private void MouseClicked()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Shoot ray from camera to check what is hit.
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactionLayer))
        {
            int objectLayerMask = (1 << hit.transform.gameObject.layer);

            //If the floor is hit, move the player to that location.
            if ((floorLayer.value & objectLayerMask) > 0)
            {
                playerMovement.Move(hit.point);
            }
            //If an interactable object is hit, interact with that object.
            else if ((interactabeLayer.value & objectLayerMask) > 0)
            {
                playerMovement.MoveToInteractable(hit.transform.gameObject);
            }
        }
    }
}
