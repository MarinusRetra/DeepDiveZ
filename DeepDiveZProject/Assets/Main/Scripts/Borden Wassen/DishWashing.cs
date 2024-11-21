using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class DishWashing : MonoBehaviour
{
    [SerializeField] private DishSpawner DishSpawner;
    [SerializeField] private Camera taskCamera;
    [SerializeField] private LayerMask DishesLayer;
    [SerializeField] private PlayerInput PlayerInput;
    [SerializeField] private float minSpeed;
    [SerializeField] private float dirtDecreaseValue = 0.01f;

    private bool isClicking;
    private bool mouseClicking;

    private Vector2 lastMousePos;

    private DecalProjector dirtDecalProjector;

    private void OnEnable()
    {
        PlayerInput.actions["LMB"].performed += (InputAction.CallbackContext ctx) => mouseClicking = true;
        PlayerInput.actions["LMB"].canceled += (InputAction.CallbackContext ctx) => mouseClicking = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        isClicking = false;

        Ray ray = taskCamera.ScreenPointToRay(Mouse.current.position.value);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, DishesLayer))
        {
            //print("Hit ray");
            if (hitInfo.transform.TryGetComponent(out Dish dish))
            {
                //print("Found dish");
                if (dish.State == Dish.DishState.BeingCleaned)
                {
                    //print("Dish is being cleaned");
                    isClicking = true;
                    dirtDecalProjector = dish.GetComponentInChildren<DecalProjector>();
                }
            }
        }

        //print("Mouse speed: " + Vector2.Distance(Mouse.current.position.value, lastMousePos) / Time.deltaTime);
        if (isClicking && mouseClicking && Vector2.Distance(Mouse.current.position.value, lastMousePos) / Time.deltaTime > minSpeed)
        {
            //print("Washing");
            dirtDecalProjector.fadeFactor -= dirtDecreaseValue;
            DishSpawner.SetAmountDone(dirtDecalProjector.transform.parent.gameObject, Mathf.Abs(dirtDecalProjector.fadeFactor - 1));
        }

        lastMousePos = Mouse.current.position.value;
    }
}
