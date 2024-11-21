using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressFeedback : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private List<MinigameProgress> progressList = new();
    private bool inMinigame = false;
    [SerializeField] private int currentSeconds = 0;
    private Sprite currentMinigameImage = null;
    [SerializeField]private Texture2D[] templateImages;

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject endUI;
    [SerializeField] private Transform scrollTransform;
    [SerializeField] private int cardOffset = 50;
    [SerializeField] private Minigames currentMinigame = Minigames.Grasmaaien;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }
        StartCoroutine(startMinigame(Minigames.Grasmaaien));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            stopMinigame();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(stopGame());
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartCoroutine(startMinigame(currentMinigame));
        }
    }

    IEnumerator startMinigame(Minigames currentMini)
    {
        currentMinigame = currentMini;
        yield return new WaitForSeconds(1);
        inMinigame = true;
        int randomTime = Random.Range(2, 10);
        int timesTakenSS = 1;
        while(inMinigame)
        {
            currentSeconds++;
            if(currentSeconds == 10 * timesTakenSS && timesTakenSS < randomTime)
            {
                currentMinigameImage = ScreenshotManager.TakeScreenshot(mainCam);
                timesTakenSS++;
            }
            yield return new WaitForSeconds(1);
        }
    }

    public void stopMinigame()
    {
        inMinigame = false;
        MinigameProgress tracker = new MinigameProgress();
        if(currentMinigameImage == null)
        {
            currentMinigameImage = ScreenshotManager.TextureToSprite(templateImages[(int)currentMinigame]);
        }
        tracker.screenshot = currentMinigameImage;
        tracker.timeSpend = currentSeconds;
        tracker.minigame = currentMinigame;
        currentSeconds = 0;
        currentMinigameImage = null;
        SortTrackerInList(tracker);
    }

    IEnumerator stopGame()
    {
        yield return new WaitForSeconds(3);
        stopMinigame();
        endUI.SetActive(true);

        for(int i = 0;i < progressList.Count; i++)
        {
            Vector3 pos = scrollTransform.position;
            GameObject card = Instantiate(cardPrefab, new Vector3(pos.x,pos.y - (cardOffset * (i)), pos.z), scrollTransform.rotation, scrollTransform.parent);
            CardReferences refr = card.GetComponent<CardReferences>();
            refr.stats = progressList[i];
            refr.UpdateCard();
            //yield return new WaitForSeconds(1);
        }
    }

    public void SortTrackerInList(MinigameProgress _progress)
    {
        if(progressList.Count == 0)
        {
            progressList.Add(_progress);
            return;
        }
        for(int i = 0;i < progressList.Count;i++)
        {
            if (!(progressList[i].timeSpend > _progress.timeSpend))
            {
                progressList.Insert(i, _progress);
                return;
            }
        }
        progressList.Add(_progress);
    }

}
public enum Minigames { Grasmaaien, Afwassen }
public struct MinigameProgress
{
    public Minigames minigame;
    public float timeSpend;
    public float progressOnTask;
    public Sprite screenshot;

    public int rating;

}
