using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectGrabbing : MonoBehaviour
{
    [SerializeField] private Camera TaskCamera;
    [SerializeField] private float DistanceToPlayer;
    [SerializeField] private LayerMask GrabbableLayer;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private PlayerInput PlayerInput;

    private GameObject currentGrabbable;
    private Vector2 mousePos;

    private float pickUpCooldown = 0.5f;
    private float pickUpTimer;
    private bool mouseClicked;

    private void OnEnable()
    {
        PlayerInput.actions["LMB"].started += (InputAction.CallbackContext ctx) => MouseClicked();
        PlayerInput.actions["LMB"].canceled += (InputAction.CallbackContext ctx) => MouseUp();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void MouseUp()
    {
        mouseClicked = false;

        if (pickUpTimer <= pickUpCooldown)
        {
            Ray ray = TaskCamera.ScreenPointToRay(Mouse.current.position.value);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, GrabbableLayer))
            {
                if (hit.transform.TryGetComponent(out Dish dish) && !dish.MayPickup) return;
                else if (dish.State == Dish.DishState.BeingCleaned) dish.State = Dish.DishState.Done;

                print("May pickup: " + dish.MayPickup);
                print("Grab");
                currentGrabbable = hit.transform.gameObject;
                currentGrabbable.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    private void MouseClicked()
    {
        pickUpTimer = 0;
        mouseClicked = true;
    }

    private void Update()
    {
        if (mouseClicked)
        {
            pickUpTimer += Time.deltaTime;
        }

        if (currentGrabbable)
        {
            Vector3 mousePos = new Vector3(Mouse.current.position.value.x, Mouse.current.position.value.y, DistanceToPlayer);
            Vector3 worldMousePos = TaskCamera.ScreenToWorldPoint(mousePos);

            currentGrabbable.GetComponent<Rigidbody>().linearVelocity = (worldMousePos - currentGrabbable.transform.position) * MovementSpeed;
            currentGrabbable.transform.rotation = Quaternion.identity;
        }
    }

    public void StopGrabbing()
    {
        currentGrabbable = null;
    }
}
