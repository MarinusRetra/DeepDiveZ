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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }
        StartCoroutine(startMinigame());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            stopMinigame(Minigames.Grasmaaien);
        }
    }

    IEnumerator startMinigame()
    {
        yield return new WaitForSeconds(1);
        inMinigame = true;
        int randomTime = Random.Range(1, 10);
        int timesTakenSS = 0;
        while(inMinigame)
        {
            yield return new WaitForSeconds(1);
            currentSeconds++;
            if(currentSeconds == 10 && timesTakenSS < randomTime)
            {
                currentMinigameImage = ScreenshotManager.TakeScreenshot(mainCam);
                timesTakenSS++;
            }
        }
    }

    public void stopMinigame(Minigames miniGame)
    {
        inMinigame = false;
        MinigameProgress tracker = new MinigameProgress();
        if(currentMinigameImage == null)
        {
            currentMinigameImage = ScreenshotManager.TextureToSprite(templateImages[(int)miniGame]);
        }
        tracker.screenshot = currentMinigameImage;
        tracker.timeSpend = currentSeconds;
        tracker.minigame = miniGame;
        currentSeconds = 0;
        currentMinigameImage = null;
        SortTrackerInList(tracker);
        StartCoroutine(startMinigame());
    }

    public void SortTrackerInList(MinigameProgress progress)
    {
        if(progressList.Count == 0)
        {
            progressList.Add(progress);
            return;
        }
        for(int i = 0;i < progressList.Count;i++)
        {
            if (!(progressList[i].timeSpend > progress.timeSpend))
            {
                progressList.Insert(i, progress);
                return;
            }
        }
        progressList.Add(progress);
    }

}
public enum Minigames { Grasmaaien, Afwassen }
public struct MinigameProgress
{
    public Minigames minigame;
    public float timeSpend;
    public float progressOnTask;
    public Sprite screenshot;

}
