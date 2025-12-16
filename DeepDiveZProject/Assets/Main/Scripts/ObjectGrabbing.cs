using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectGrabbing : MonoBehaviour
{
    [SerializeField] private Camera taskCamera;
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private LayerMask grabbableLayer;
    [SerializeField] private float movementSpeed;
    [SerializeField] private PlayerInput playerInput;

    private GameObject currentGrabbable;

    private float pickUpCooldown = 0.5f;
    private float pickUpTimer;
    private bool mouseClicked;

    private void OnEnable()
    {
        //Call MouseClicked and MouseUp based on if the left mouse button is pressed or not.
        playerInput.actions["LMB"].started += (InputAction.CallbackContext ctx) => MouseClicked();
        playerInput.actions["LMB"].canceled += (InputAction.CallbackContext ctx) => MouseUp();
    }

    private void MouseUp()
    {
        mouseClicked = false;

        //Only pickup the object when the player holds the left mouse button for a short time, otherwise allow for manipulating the object, like washing the dishes.
        if (pickUpTimer <= pickUpCooldown)
        {
            print("went go");
            Ray ray = taskCamera.ScreenPointToRay(Mouse.current.position.value);

            //Shoot ray from the middle of the camera to check if the ray hits a dish.
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, grabbableLayer))
            {
                print(hit.transform.gameObject.name);
                if (hit.transform.TryGetComponent(out Dish dish) && !dish.MayPickup) return;
                else if (dish.State == Dish.DishState.BeingCleaned) dish.State = Dish.DishState.Done;

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
        //Keep track of how long the mouse is pressed.
        if (mouseClicked)
        {
            pickUpTimer += Time.deltaTime;
        }

        //Move the grabbale towards the mouse position with a preset z distance.
        if (currentGrabbable)
        {
            Vector3 mousePos = new Vector3(Mouse.current.position.value.x, Mouse.current.position.value.y, distanceToPlayer);
            Vector3 worldMousePos = taskCamera.ScreenToWorldPoint(mousePos);

            currentGrabbable.GetComponent<Rigidbody>().linearVelocity = (worldMousePos - currentGrabbable.transform.position) * movementSpeed;
            currentGrabbable.transform.rotation = Quaternion.identity;
        }
    }

    /// <summary>
    /// Stop grabbing the currentGrabbable.
    /// </summary>
    public void StopGrabbing()
    {
        currentGrabbable = null;
    }
}
