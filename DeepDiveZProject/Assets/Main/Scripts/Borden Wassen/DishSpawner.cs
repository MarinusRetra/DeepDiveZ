using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject DishPrefab;
    [SerializeField] private int SpawnAmount = 10;

    private List<Rigidbody> dishes = new();

    private void OnEnable()
    {
        for (int i = 0; i < SpawnAmount; i++)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                Vector3 spawnPos = hit.point;

                Rigidbody dish = Instantiate(DishPrefab, spawnPos, Quaternion.identity).GetComponent<Rigidbody>();
                dishes.Add(dish);
                dish.isKinematic = true;
                dish.GetComponent<Dish>().MayPickup = false;
            }
        }

        UnlockTop();
    }

    public void UnlockTop()
    {
        dishes[dishes.Count - 1].isKinematic = false;
        dishes[dishes.Count - 1].GetComponent<Dish>().MayPickup = true;
    }

    public void Remove(Rigidbody dish)
    {
        dishes.Remove(dish);
    }
}
