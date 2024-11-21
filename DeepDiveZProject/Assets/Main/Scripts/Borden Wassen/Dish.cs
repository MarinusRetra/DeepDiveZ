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
                transform.SetPositionAndRotation(placeSpot.GetPlaceSpot.position, placeSpot.GetPlaceSpot.rotation);
                Rigidbody.isKinematic = true;
                State = DishState.BeingCleaned;
            }
            else if (placeSpot.GetPlaceSpotType == DishesPlaceSpot.PlaceSpotType.DryingRack && State == DishState.Done)
            {
                print("Drying rack");
                objectGrabbing.StopGrabbing();
                transform.SetPositionAndRotation(placeSpot.GetPlaceSpot.position, placeSpot.GetPlaceSpot.rotation);
                Rigidbody.isKinematic = true;
                State = DishState.Drying;
                //DishData dishData = dishSpawner.GetDishData(gameObject);
                //dishData.IsDone = true;
                dishSpawner.SetIsDone(gameObject, true);
                //DishData test = dishSpawner.GetDishData(gameObject);
                //print("IsDone : " + test.IsDone);
                MayPickup = false;
                dishSpawner.UnlockTop();
                dishSpawner.CheckDone();
            }
        }
    }
}
