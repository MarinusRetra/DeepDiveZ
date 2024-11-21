using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.L))
        {
            StopMinigame(0);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(StopGame());
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartCoroutine(StartMinigame(Minigames.Grasmaaien));
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(StartMinigame(Minigames.Afwassen));
        }
#endif
    }

    public void StartMinigameFunction(string game)
    {
        Minigames miniGame;

        switch (game)
        {
            case "AfWas":
                miniGame = Minigames.Afwassen;
                break;

            case "GrasMaaien":
                miniGame = Minigames.Grasmaaien;
                break;

            default:
                print("WARNING: Not correct string name, defaulting to grasmaaien");
                miniGame = Minigames.Grasmaaien;
                break;
        }

        StartCoroutine(StartMinigame(miniGame));
    }

    public IEnumerator StartMinigame(Minigames currentMini)
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

    public void StopMinigame(float _progress)
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
        tracker.progressOnTask = (int)_progress;
        currentSeconds = 0;
        currentMinigameImage = null;
        SortTrackerInList(tracker);
    }

    public void StopGameFunction()
    {
        StartCoroutine(StopGame());
    }

    public IEnumerator StopGame()
    {
        //StopMinigame(0);
        endUI.SetActive(true);
        FillInUI();
        for (int i = 0;i < progressList.Count; i++)
        {
            GameObject card = Instantiate(cardPrefab, scrollTransform.parent);
            CardReferences refr = card.GetComponent<CardReferences>();
            refr.stats = progressList[i];
            refr.UpdateCard();
            yield return new WaitForSeconds(.3f);
        }
        
    }

    private void FillInUI()
    {
        LeerlijnStats best = GetBestMinigame();
        MinigameProgress summaryCard = new();
        summaryCard.timeSpend = best.totalTimeSpent;
        summaryCard.rating = best.totalThumbs;
        summaryCard.progressOnTask = best.totalPercentage / best.amountOfGames;
        summaryCard.leerLijn = best.leerLijn;
        summaryCard.rating = 0;
        summaryCard.screenshot = ScreenshotManager.TextureToSprite(best.image);
        CardReferences endRef = endUI.GetComponent<CardReferences>();
        endRef.stats = summaryCard;
        endRef.UpdateCard();
    }

    private LeerlijnStats GetBestMinigame()
    {
        LeerlijnStats[] lijnen = new LeerlijnStats[2];
        lijnen[0].leerLijn = Leerlijnen.Groen;
        lijnen[1].leerLijn = Leerlijnen.Detailhandel;
        for(int i = 0;i < lijnen.Length; i++)
        {
            lijnen[i].totalTimeSpent = 0;
            lijnen[i].totalPercentage = 0;
            lijnen[i].totalThumbs = 0;
            lijnen[i].amountOfGames = 0;
        }
        for (int i = 0; i < progressList.Count; i++)
        {
            int currentLijn = 0;
            if (progressList[i].minigame == Minigames.Grasmaaien)
            {
                currentLijn = 0;
            }else if (progressList[i].minigame == Minigames.Afwassen)
            {
                currentLijn = 1;
            }

            lijnen[currentLijn].totalTimeSpent += progressList[i].timeSpend;
            lijnen[currentLijn].totalPercentage += progressList[i].progressOnTask;
            lijnen[currentLijn].amountOfGames++;
            if (progressList[i].rating == 1) { lijnen[currentLijn].totalThumbs++; }
        }

        LeerlijnStats bestLijn = lijnen[0];
        bestLijn.image = templateImages[0];
        for (int i = 1; i < lijnen.Length; i++)
        {
            if (lijnen[i].totalTimeSpent > lijnen[i - 1].totalTimeSpent)
            {
                bestLijn = lijnen[i];
                bestLijn.image = templateImages[i];
            }
        }

        return bestLijn;
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
    public Leerlijnen leerLijn;
    public int timeSpend;
    public int progressOnTask;
    public Sprite screenshot;

    public int rating;

}
public enum Leerlijnen { Groen,Detailhandel,Product,Kunst,Dienstverlening }
public struct LeerlijnStats
{
    public Leerlijnen leerLijn;
    public int amountOfGames;
    public int totalTimeSpent;
    public int totalPercentage;
    public int totalThumbs;
    public Texture2D image;
}
