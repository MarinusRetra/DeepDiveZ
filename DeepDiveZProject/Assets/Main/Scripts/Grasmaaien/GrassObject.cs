using UnityEngine;

public class GrassObject : MonoBehaviour
{
    [SerializeField] Grass GrassParent;

    private void OnDisable()
    {
        GrassParent.RemoveGrassElement(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LawnMower")) 
        {
            Debug.Log("asdasdads");
            StartCoroutine(Grass.WaitThenDestory(gameObject));
        }
    }
}
