using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private UnityEvent onInteract;

    /// <summary>
    /// Calls UnityEvent "onInteract" when interacting with this object.
    /// </summary>
    public void Interact()
    {
        onInteract.Invoke();
    }
}
