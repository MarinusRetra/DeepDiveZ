using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float acceptDistance;

    public UnityEvent OnPlayerStandsStill;

    private NavMeshAgent agent;

    private bool isMoving;

    private InteractableObject interactable;

    private bool Active = true;

    //playerCollider is nodig om de radius te pakken voor de grass shader
    private CapsuleCollider playerCollider;

    private void Awake()
    {
        playerCollider = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void Move(Vector3 position)
    {
        interactable = null;
        isMoving = true;
        agent.SetDestination(position);
    }

    public void MoveToInteractable(GameObject _interactable)
    {
        //When no interactable script is found, return
        if (!_interactable.TryGetComponent(out InteractableObject targetInteractable))
        {
            Debug.Log("Interactable object not found on interactable", _interactable);
            return;
        }

        //When collider is found, move to the edge of the collider
        if (_interactable.TryGetComponent(out BoxCollider interactableCollider))
        {
            Move(interactableCollider.ClosestPoint(transform.position));
            Debug.DrawRay(interactableCollider.ClosestPoint(transform.position), Vector3.up, Color.green, 30);
        }
        //Otherwise just move to the center
        else
        {
            Move(_interactable.transform.position);
        }

        interactable = targetInteractable;
    }

    private void Update()
    {
        //Gebruik ik voor gras displacement bij de gras shader -Marinus
        if (Active)
        {
            Debug.Log(gameObject.name);
            Shader.SetGlobalVector("_Player", transform.position + Vector3.up * playerCollider.radius);
        }

        if (agent == null) return;

        //Call event when player stopped moving
        if (agent.hasPath == false && isMoving)
        {
            isMoving = false;
            OnPlayerStandsStill.Invoke();

            if (interactable) interactable.Interact();
        }

        if (agent.remainingDistance <= acceptDistance)
        {
            agent.SetDestination(transform.position);
        }
    }

    public void ToggleActive()
    { 
        Active = !Active;
    }
}
