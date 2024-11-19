using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput PlayerInput;
    [SerializeField] private LayerMask FloorLayer;

    public UnityEvent OnPlayerStandsStill;

    private NavMeshAgent agent;

    private bool isMoving;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        PlayerInput.actions["LMB"].performed += (InputAction.CallbackContext ctx) => MouseClicked();
    }

    private void MouseClicked()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Shoot ray from camera to where the player should move
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, FloorLayer))
        {
            isMoving = true;
            print("Ray hit somethin");
            agent.SetDestination(hit.point);
        }
    }

    private void Update()
    {
        //Call event when player stopped moving
        if (agent.hasPath == false && isMoving)
        {
            isMoving = false;
            print("Player stopped");
            OnPlayerStandsStill.Invoke();
        }
    }
}
