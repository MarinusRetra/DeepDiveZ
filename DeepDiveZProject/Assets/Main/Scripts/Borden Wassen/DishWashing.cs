using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class DishWashing : MonoBehaviour
{
    [SerializeField] private DishSpawner dishSpawner;
    [SerializeField] private Camera taskCamera;
    [SerializeField] private LayerMask dishesLayer;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float minSpeed;
    [SerializeField] private float dirtDecreaseValue = 0.01f;

    private bool cursorHitDishes;
    private bool mouseClicking;

    private DecalProjector dirtDecalProjector;

    private void OnEnable()
    {
        //Set mouseClicking based on if the left mouse button is pressed or not.
        playerInput.actions["LMB"].performed += (InputAction.CallbackContext ctx) => mouseClicking = true;
        playerInput.actions["LMB"].canceled += (InputAction.CallbackContext ctx) => mouseClicking = false;
    }

    private void Update()
    {
        cursorHitDishes = false;

        //Shoot ray from the middle of the screen.
        Ray ray = taskCamera.ScreenPointToRay(Mouse.current.position.value);

        //If the ray hits a dish that is beingcleaned, allow it to be cleaned
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, dishesLayer))
        {
            if (hit.transform.TryGetComponent(out Dish dish))
            {
                if (dish.State == Dish.DishState.BeingCleaned)
                {
                    cursorHitDishes = true;
                    dirtDecalProjector = dish.GetComponentInChildren<DecalProjector>();
                }
            }
        }

        //When allowed and the mouse is moving fast enough, clean the detected dish.
        if (cursorHitDishes && mouseClicking && Mouse.current.delta.magnitude > minSpeed)
        {
            //Lower the opactity of the decal projector from the dish.
            dirtDecalProjector.fadeFactor -= dirtDecreaseValue;
            dishSpawner.SetAmountDone(dirtDecalProjector.transform.parent.gameObject, Mathf.Abs(dirtDecalProjector.fadeFactor - 1));
        }
    }
}
