using UnityEngine;
using UnityEngine.InputSystem;

public class MouseClickManager : MonoBehaviour
{
    [SerializeField] private LayerMask InteractionLayer;
    [SerializeField] private LayerMask InteractabeLayer;
    [SerializeField] private LayerMask FloorLayer;
    [SerializeField] private PlayerInput PlayerInput;
    [SerializeField] private PlayerMovement PlayerMovement;

    private void OnEnable()
    {
        PlayerInput.actions["LMB"].performed += (InputAction.CallbackContext ctx) => MouseClicked();
    }

    private void MouseClicked()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Shoot ray from camera to where the player should move
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, InteractionLayer))
        {
            int objectLayerMask = (1 << hit.transform.gameObject.layer);

            if ((FloorLayer.value & objectLayerMask) > 0)
            {
                PlayerMovement.Move(hit.point);
            }
            else if ((InteractabeLayer.value & objectLayerMask) > 0)
            {
                PlayerMovement.MoveToInteractable(hit.transform.gameObject);
            }
        }
    }
}
