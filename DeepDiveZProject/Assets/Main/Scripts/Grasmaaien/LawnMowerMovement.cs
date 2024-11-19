using UnityEngine;
using UnityEngine.InputSystem;

public class LawnMowerMovement : MonoBehaviour
{
    [SerializeField] private float maxMovementSpeed;
    [SerializeField] private float turnSpeed;

    private Rigidbody rb;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = maxMovementSpeed;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * (((Mouse.current.delta.x.ReadValue()/100) * turnSpeed)));

    #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.C))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    #endif
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward, ForceMode.Impulse);
    }
}
