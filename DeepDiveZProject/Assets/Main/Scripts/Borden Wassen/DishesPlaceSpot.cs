using UnityEngine;

public class DishesPlaceSpot : MonoBehaviour
{
    [SerializeField] private PlaceSpotType SpotType;
    [SerializeField] private Transform PlaceSpot;

    public PlaceSpotType GetPlaceSpotType => SpotType;

    public Transform GetPlaceSpot => PlaceSpot;

    public enum PlaceSpotType
    {
        Sink,
        DryingRack
    }
}
