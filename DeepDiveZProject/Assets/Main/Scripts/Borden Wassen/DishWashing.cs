using UnityEngine;
using UnityEngine.InputSystem;

public class DishWashing : MonoBehaviour
{
    [SerializeField] private Camera taskCamera;
    [SerializeField] private LayerMask DishesLayer;
    [SerializeField] private PlayerInput PlayerInput;

    private void OnEnable()
    {
        PlayerInput.actions["LMB"].performed += (InputAction.CallbackContext ctx) => MouseClicked();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void MouseClicked()
    {
        Ray ray = taskCamera.ScreenPointToRay(Mouse.current.position.value);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, DishesLayer))
        {
            if (hitInfo.transform.TryGetComponent(out Dish dish))
            {
                print("Found plate");
            }
        }
    }
}
