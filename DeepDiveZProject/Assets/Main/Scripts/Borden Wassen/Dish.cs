using UnityEngine;

public class Dish : MonoBehaviour
{
    [SerializeField] private ObjectGrabbing ObjectGrabbing;
    [SerializeField] private DishState State;
    [SerializeField] private Collider Collider;
    [SerializeField] private Rigidbody Rigidbody;

    private enum DishState
    {
        Dirty,
        BeingCleaned,
        Clean,
        Drying
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Colliding!");
        if (other.transform.TryGetComponent(out DishesPlaceSpot placeSpot))
        {
            print("Found placespot");
            ObjectGrabbing.StopGrabbing();
            transform.position = placeSpot.GetPlaceSpot.position;
            transform.rotation = placeSpot.GetPlaceSpot.rotation;
            Collider.enabled = false;
            Rigidbody.isKinematic = true;
            State = DishState.BeingCleaned;
        }
    }
}
