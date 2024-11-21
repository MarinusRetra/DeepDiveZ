using UnityEngine;

public class Dish : MonoBehaviour
{
    public DishState State;
    [SerializeField] private Rigidbody Rigidbody;

    public bool MayPickup;

    private ObjectGrabbing objectGrabbing;
    private DishSpawner dishSpawner;

    public enum DishState
    {
        Dirty,
        BeingCleaned,
        Done,
        Drying
    }

    private void Awake()
    {
        objectGrabbing = FindAnyObjectByType<ObjectGrabbing>();
        dishSpawner = FindAnyObjectByType<DishSpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out DishesPlaceSpot placeSpot))
        {
            if (placeSpot.GetPlaceSpotType == DishesPlaceSpot.PlaceSpotType.Sink && State == DishState.Dirty)
            {
                print("Sink go brr");
                objectGrabbing.StopGrabbing();
                transform.position = placeSpot.GetPlaceSpot.position;
                transform.rotation = placeSpot.GetPlaceSpot.rotation;
                Rigidbody.isKinematic = true;
                State = DishState.BeingCleaned;
            }
            else if (placeSpot.GetPlaceSpotType == DishesPlaceSpot.PlaceSpotType.DryingRack && State == DishState.Done)
            {
                print("Drying rack");
                objectGrabbing.StopGrabbing();
                transform.position = placeSpot.GetPlaceSpot.position;
                transform.rotation = placeSpot.GetPlaceSpot.rotation;
                Rigidbody.isKinematic = true;
                State = DishState.BeingCleaned;
                MayPickup = false;
                dishSpawner.Remove(Rigidbody);
                dishSpawner.UnlockTop();
            }
        }
    }
}
