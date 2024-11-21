using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardReferences : MonoBehaviour
{
    public MinigameProgress stats;

    public TMP_Text timeText;
    public TMP_Text progressText;

    public Image screenshot;
    public Image thumb;
    public TMP_Text thumbText;

    public Sprite[] thumbSprites;

    public RawImage background;

    public TMP_Text titleText;

    public void UpdateCard()
    {
        int minutes = ((int)stats.timeSpend / 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, stats.timeSpend - minutes * 60);
        progressText.text = stats.progressOnTask.ToString() + "%";

        screenshot.sprite = stats.screenshot;
        Sprite sprite;
        switch (stats.rating)
        {
            case -1:
                thumb.sprite = thumbSprites[0];
                break;
            case 0:
                thumb.gameObject.SetActive(false);
                break;
            case 1:
                thumb.sprite = thumbSprites[1];
                break;
        }

        if(stats.minigame == Minigames.Grasmaaien)
        {
            background.color = Color.green;
        }else if(stats.minigame == Minigames.Afwassen)
        {
            background.color = Color.blue;
        }

        if(thumbText != null)
        {
            thumbText.text = stats.rating.ToString();
            thumb.sprite = thumbSprites[1];
        }

        if(titleText != null)
        {
            titleText.text = stats.leerLijn.ToString();
            print(stats.leerLijn.ToString());
        }
    }
}
