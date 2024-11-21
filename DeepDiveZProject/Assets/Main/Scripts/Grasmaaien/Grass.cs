using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Grass : MonoBehaviour
{
   [SerializeField] bool temp = false;

    [SerializeField] private List<GameObject> GrassObjectsStartCount;
    private List<GameObject> GrassObjectsCurrentCount;

    [SerializeField] private TextMeshProUGUI grassPercentageText;
    public void MinigameStart()
    {
        foreach (GameObject obj in GrassObjectsStartCount)
        { 
            obj.SetActive(true);
        }

        GrassObjectsCurrentCount = new(GrassObjectsStartCount);
        SetPercentage();
    }

    private void SetPercentage()
    {
        grassPercentageText.text = $"{ Mathf.Round(Mathf.Abs(((float)GrassObjectsCurrentCount.Count / (float)GrassObjectsStartCount.Count * 100) - 100))}";
    }

    private void Update()
    {
        if (temp)
        { 
            temp = false;
            MinigameStart();
        }
    }

    //Dit wordt aangeroepen als een gras stukje uitgezet wordt
    public void RemoveGrassElement(GameObject _grassObjectIn)
    {
        GrassObjectsCurrentCount.Remove(_grassObjectIn);
        SetPercentage();
        StopAllCoroutines();
    }

    //Op collision wordt deze als eerst aangeroepen
    public static IEnumerator WaitThenDestory(GameObject _gameObjectIn)
    {
        yield return new WaitForSeconds(0.2f);
        _gameObjectIn.SetActive(false);
    }
}