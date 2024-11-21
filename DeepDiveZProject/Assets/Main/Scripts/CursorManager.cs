using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static void ChangeCursorLock(bool locked, bool visible)
    {
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if(visible)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
    }
}
