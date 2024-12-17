using System.Collections.Generic;
using TMPro;
using UnityEngine;

//This struct contains data for a dish. This setup for all dishes.
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
    [SerializeField] private GameObject dishPrefab;
    [SerializeField] private int spawnAmount = 10;
    [SerializeField] private ProgressFeedback progressFeedback;
    [SerializeField] private ExitButton exitButton;
    [SerializeField] private TMP_Text progressText;
    
    private List<DishData> dishes = new();

    private void OnEnable()
    {
        ResetMiniGame();
    }

    private void Setup()
    {
        //Spawn the plates
        for (int i = 0; i < spawnAmount; i++)
        {
            //Shoots a ray downwards until it hits something, either a plate or the counter top.
            //At that position it spawns the plate to make sure they are all stacked on top of each other.
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                Vector3 spawnPos = hit.point;

                Dish dish = Instantiate(dishPrefab, spawnPos, Quaternion.identity).GetComponent<Dish>();

                //Name the plate for debugging
                dish.name = "Plate: " + i;
                DishData dishData = new()
                {
                    Dish = dish,
                    Rb = dish.GetComponent<Rigidbody>(),
                    GameObject = dish.gameObject,
                };

                //Make it kinematic so that the pile doesn't fall over.
                dishData.Rb.isKinematic = true;
                dishData.Dish.MayPickup = false;

                dishes.Add(dishData);
            }
        }

        UnlockTop();
    }

    /// <summary>
    /// Allow the top dish to be pickup.
    /// </summary>
    public void UnlockTop()
    {
        DishData validDishData = new();

        //Set validDishDate to the data from the first dish that is not done, so the top dish.
        for (int i = 0; i < dishes.Count; i++)
        {
            if (!dishes[i].IsDone) validDishData = dishes[i];
        }

        //Check if the dish actually exists, if it doens't there probably isn't another dish, so the minigame is probably done.
        if (validDishData.GameObject != null)
        {
            validDishData.Rb.isKinematic = false;
            validDishData.Dish.MayPickup = true;
        }
        else
        {
            print("UnlockTop didn't work, minigame done?");
        }
    }

    /// <summary>
    /// Returns the DishData based on a GameObject.
    /// </summary>
    /// <param name="dish">The GameObject the search is based on.</param>
    /// <returns>If found the correct DishData, otherwise an empty DishData object.</returns>
    public DishData GetDishData(GameObject dish)
    {
        for (int i = 0; i < dishes.Count; i++)
        {
            if (dishes[i].GameObject == dish)
            {
                return dishes[i];
            }
        }

        return new DishData();
    }

    /// <summary>
    /// Search for the correct dish using a GameObject and then set IsDone of the found DishData to the "value".
    /// </summary>
    /// <param name="dish">The GameObject used in the search.</param>
    /// <param name="value">The value the found DishDate's IsDone is set to.</param>
    public void SetIsDone(GameObject dish, bool value)
    {
        DishData dishData = GetDishData(dish);

        dishData.IsDone = value;
    }

    /// <summary>
    /// Search for the correct dish using a GameObject and then set AmountDone of the found DishData to the "value".
    /// </summary>
    /// <param name="dish">The GameObject used in the search.</param>
    /// <param name="value">The value the found DishDate's AmountDone is set to.</param>
    public void SetAmountDone(GameObject dish, float value)
    {
        DishData dishData = GetDishData(dish);

        dishData.AmountDone = value;
    }

    /// <summary>
    /// Check if the minigame is done.
    /// </summary>
    public void CheckDone()
    {
        float percentageDone = GetPercentageDone();

        //Check if the minigame is done by checking if all the dishes are done.
        for (int i = 0; i < dishes.Count; i++)
        {
            if (dishes[i].IsDone) return;
        }

        progressFeedback.StopMinigame(percentageDone);
        exitButton.OnExit.Invoke();
    }

    /// <summary>
    /// Get the total percentage done of all dishes
    /// </summary>
    /// <returns>A number that is the total percentage of all dishes AmountDone from 0 to 100.</returns>
    public float GetPercentageDone()
    {
        float amountDone = 0;

        float percentageDone = 0;

        //Check for all dishes if their done and keep count with amountDone.
        //Also calculate total percentageDone.
        for (int i = 0; i < dishes.Count; i++)
        {
            if (dishes[i].IsDone)
            {
                amountDone++;
                percentageDone += dishes[i].AmountDone;
            }
        }

        //Calculate percentage.
        float percentage = (percentageDone) / ((float)dishes.Count) * 100;

        progressText.SetText(Mathf.Round(percentage * 10) / 10 + "%");

        return Mathf.Round(percentage * 10) / 10;
    }

    /// <summary>
    /// Reset the minigame to make sure it's replayable.
    /// </summary>
    public void ResetMiniGame()
    {
        //Destroy all the plates.
        for (int i = 0; i < dishes.Count; i++)
        {
            Destroy(dishes[i].GameObject);
        }

        //Clear the dishes list.
        dishes.Clear();
        dishes = new();

        //Sets up the next dish minigame, like spawning dishes.
        Setup();
    }
}
