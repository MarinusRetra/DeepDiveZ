using UnityEngine;

public class DishesPlaceSpot : MonoBehaviour
{
    [SerializeField] private PlaceSpotType spotType;
    [SerializeField] private Transform placeSpot;

    public PlaceSpotType GetPlaceSpotType => spotType;

    public Transform GetPlaceSpot => placeSpot;

    public enum PlaceSpotType
    {
        Sink,
        DryingRack
    }
}
