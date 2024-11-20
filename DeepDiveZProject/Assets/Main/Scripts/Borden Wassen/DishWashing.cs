using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class DishWashing : MonoBehaviour
{
    [SerializeField] private Camera taskCamera;
    [SerializeField] private LayerMask DishesLayer;
    [SerializeField] private PlayerInput PlayerInput;
    [SerializeField] private DecalProjector dirtDecalProjector;
    [SerializeField] private float minSpeed;
    [SerializeField] private float dirtDecreaseValue = 0.01f;

    private bool isClicking;

    private Vector2 lastMousePos;

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
                isClicking = true;
            }
        }
    }

    private void Update()
    {
        //print("Mouse speed: " + Vector2.Distance(Mouse.current.position.value, lastMousePos) / Time.deltaTime);
        if (Mouse.current.leftButton.IsPressed() && Vector2.Distance(Mouse.current.position.value, lastMousePos) / Time.deltaTime > minSpeed)
        {
            print("Washing");
            dirtDecalProjector.fadeFactor -= dirtDecreaseValue;
        }

        lastMousePos = Mouse.current.position.value;
    }
}
