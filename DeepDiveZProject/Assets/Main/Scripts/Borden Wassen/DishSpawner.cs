using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct DishData
{
    public float PercentDone;
    public Dish Dish;
    public Rigidbody Rb;
    public GameObject GameObject;
    public bool IsDone;
}

public class DishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject DishPrefab;
    [SerializeField] private int SpawnAmount = 10;
    [SerializeField] private ProgressFeedback progressFeedback;
    [SerializeField] private ExitButton exitButton;
    
    private List<DishData> dishes = new();

    private void OnEnable()
    {
        ResetMiniGame();
    }

    private void Setup()
    {
        for (int i = 0; i < SpawnAmount; i++)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                print(hit.transform.name);
                Vector3 spawnPos = hit.point;

                Dish dish = Instantiate(DishPrefab, spawnPos, Quaternion.identity).GetComponent<Dish>();
                dish.name = "Plate: " + i;
                DishData dishData = new()
                {
                    Dish = dish,
                    Rb = dish.GetComponent<Rigidbody>(),
                    GameObject = dish.gameObject,
                };

                dishData.Rb.isKinematic = true;
                dishData.Dish.MayPickup = false;

                dishes.Add(dishData);
            }
        }

        UnlockTop();
    }

    public void UnlockTop()
    {
        DishData validDishData = new();

        for (int i = 0; i < dishes.Count; i++)
        {
            if (!dishes[i].IsDone) validDishData = dishes[i];
        }

        if (validDishData.GameObject != null)
        {
            validDishData.Rb.isKinematic = false;
            validDishData.Dish.MayPickup = true;
        }
        else
        {
            print("Unlock to didn't work, minigame done?");
        }
    }

    public DishData GetDishData(GameObject dish)
    {
        for (int i = 0; i < dishes.Count; i++)
        {
            if (dishes[i].GameObject == dish)
            {
                return dishes[i];
            }
        }

        print("NOT WORKING11");

        return new DishData();
    }

    public void SetIsDone(GameObject dish, bool value)
    {
        for (int i = 0; i < dishes.Count; i++)
        {
            if (dishes[i].GameObject == dish)
            {
                DishData dishData = dishes[i];
                dishData.IsDone = value;
                dishes[i] = dishData;
            }
        }
    }

    public void CheckDone()
    {
        bool done = false;

        for (int i = 0; i < dishes.Count; i++)
        {
            if (dishes[i].IsDone) return;
        }

        progressFeedback.StopMinigame(100);
        exitButton.OnExit.Invoke();
    }

    public float GetPercentageDone()
    {
        float percentage = 0;

        float amountDone = 0;

        for (int i = 0; i < dishes.Count; i++)
        {
            if (dishes[i].IsDone) amountDone++;
        }

        percentage = (amountDone / (float)dishes.Count) * 100;

        return percentage;
    }

    public void ResetMiniGame()
    {
        for (int i = 0; i < dishes.Count; i++)
        {
            DestroyImmediate(dishes[i].GameObject);
        }

        dishes.Clear();
        dishes = new();

        Setup();
    }
}
