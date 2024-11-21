using UnityEngine;
using UnityEngine.InputSystem;

public class LawnMowerMovement : MonoBehaviour
{
    [SerializeField] private float maxMovementSpeed;
    [SerializeField] private float turnSpeed;

    private CapsuleCollider playerCollider;

    private Rigidbody rb;

    bool Active = false;

    private void Start()
    {
        playerCollider = GetComponent<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = maxMovementSpeed;
    }

    private void Update()
    {
        if (Active)
        {
            Debug.Log(gameObject.name);
            Shader.SetGlobalVector("_Player", transform.position + Vector3.up * playerCollider.radius);
        }

        transform.Rotate(Vector3.up * (((Mouse.current.delta.x.ReadValue() / 100) * turnSpeed)));

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.C))
        {
            Cursor.lockState = CursorLockMode.None;
        }
#endif
    }

    public void ToggleActive()
    { 
        Active = !Active;
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward, ForceMode.Impulse);
    }
}
