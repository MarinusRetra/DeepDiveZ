using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ExitButton : MonoBehaviour
{
    [SerializeField] private Camera TaskCamera;
    [SerializeField] private PlayerInput PlayerInput;
    [SerializeField] private ProgressFeedback progressFeedback;
    [SerializeField] private DishSpawner dishSpawner;

    public UnityEvent OnExit = new();

    private void OnEnable()
    {
        PlayerInput.actions["LMB"].started += (InputAction.CallbackContext ctx) => MouseClicked();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void MouseClicked()
    {
        Ray ray = TaskCamera.ScreenPointToRay(Mouse.current.position.value);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject == gameObject)
            {
                progressFeedback.StopMinigame(dishSpawner.GetPercentageDone());
                OnExit.Invoke();
            }
        }
    }
}
