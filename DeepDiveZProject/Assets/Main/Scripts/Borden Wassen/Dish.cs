using UnityEngine;

public class Dish : MonoBehaviour
{
    public DishState State;
    public bool MayPickup;

    [SerializeField] private Rigidbody rb;

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
        //Find scripts and assign. There can only ever be 1 of either of these scripts.
        objectGrabbing = FindAnyObjectByType<ObjectGrabbing>();
        dishSpawner = FindAnyObjectByType<DishSpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if whatever this collides with has a DishesPlaceSpot script.
        if (other.transform.TryGetComponent(out DishesPlaceSpot placeSpot))
        {
            //Check if the dish is dirty and it collides with the sink, then place it in the sink.
            if (placeSpot.GetPlaceSpotType == DishesPlaceSpot.PlaceSpotType.Sink && State == DishState.Dirty)
            {
                objectGrabbing.StopGrabbing();
                transform.SetPositionAndRotation(placeSpot.GetPlaceSpot.position, placeSpot.GetPlaceSpot.rotation);
                rb.isKinematic = true;
                State = DishState.BeingCleaned;
            }
            //Check if the dish is cleaned and it collides with the drying rack, then place it in the dryingrack.
            else if (placeSpot.GetPlaceSpotType == DishesPlaceSpot.PlaceSpotType.DryingRack && State == DishState.Done)
            {
                objectGrabbing.StopGrabbing();
                transform.SetPositionAndRotation(placeSpot.GetPlaceSpot.position, placeSpot.GetPlaceSpot.rotation);
                rb.isKinematic = true;
                State = DishState.Drying;
                dishSpawner.SetIsDone(gameObject, true);
                MayPickup = false;

                //Allow the top dish from the dishes pile to be grabbed.
                dishSpawner.UnlockTop();

                //Check if the minigame is done.
                dishSpawner.CheckDone();
            }
        }
    }
}
