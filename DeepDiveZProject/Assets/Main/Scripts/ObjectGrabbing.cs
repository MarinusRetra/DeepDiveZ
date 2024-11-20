using NUnit.Framework.Constraints;
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

    private void OnEnable()
    {
        PlayerInput.actions["LMB"].performed += (InputAction.CallbackContext ctx) => MouseClicked();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void MouseClicked()
    {
        Ray ray = TaskCamera.ScreenPointToRay(Mouse.current.position.value);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, GrabbableLayer))
        {
            currentGrabbable = hitInfo.transform.gameObject;
        }
    }

    private void Update()
    {
        if (currentGrabbable)
        {
            Vector3 mousePos = new Vector3(Mouse.current.position.value.x, Mouse.current.position.value.y, DistanceToPlayer);
            Vector3 worldMousePos = TaskCamera.ScreenToWorldPoint(mousePos);

            currentGrabbable.GetComponent<Rigidbody>().linearVelocity = (worldMousePos - currentGrabbable.transform.position) * MovementSpeed;
        }
    }

    public void StopGrabbing()
    {
        currentGrabbable = null;
    }
}
