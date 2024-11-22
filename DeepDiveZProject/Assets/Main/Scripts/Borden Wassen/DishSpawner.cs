using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct DishData
{
    public float PercentDone;
    public Dish Dish;
    public Rigidbody Rb;
    public GameObject GameObject;
    public bool IsDone;
    public float AmountDone;
}

public class DishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject DishPrefab;
    [SerializeField] private int SpawnAmount = 10;
    [SerializeField] private ProgressFeedback progressFeedback;
    [SerializeField] private ExitButton exitButton;
    [SerializeField] private TMP_Text ProgressText;
    
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

    public void SetAmountDone(GameObject dish, float value)
    {
        for (int i = 0; i < dishes.Count; i++)
        {
            if (dishes[i].GameObject == dish)
            {
                DishData dishData = dishes[i];
                dishData.AmountDone = value;
                dishes[i] = dishData;
            }
        }
    }

    public void CheckDone()
    {
        bool done = false;

        GetPercentageDone();

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

        float percentageDone = 0;

        for (int i = 0; i < dishes.Count; i++)
        {
            if (dishes[i].IsDone)
            {
                amountDone++;
                percentageDone += dishes[i].AmountDone;
            }
        }

        print(percentageDone);

        //Times 2 because the decal projector can only become 1 at max
        percentage = ((percentageDone) / ((float)dishes.Count)) * 100;

        ProgressText.SetText(Mathf.Round(percentage * 10) / 10 + "%");

        return Mathf.Round(percentage * 10) / 10;
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
