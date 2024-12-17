using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ExitButton : MonoBehaviour
{
    public UnityEvent OnExit = new();

    [SerializeField] private Camera taskCamera;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private ProgressFeedback progressFeedback;
    [SerializeField] private DishSpawner dishSpawner;

    private void OnEnable()
    {
        //Call MouseClicked when the mouse is clicked.
        playerInput.actions["LMB"].started += (InputAction.CallbackContext ctx) => MouseClicked();
    }

    private void MouseClicked()
    {
        Ray ray = taskCamera.ScreenPointToRay(Mouse.current.position.value);

        //Shoot a ray from the middle of the camera to check if this object, the exit button, is hit.
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject == gameObject)
            {
                //If it hits the exit button, the OnExit event is invoked and the dish washing minigame is stopped.
                progressFeedback.StopMinigame(dishSpawner.GetPercentageDone());
                OnExit.Invoke();
            }
        }
    }
}
