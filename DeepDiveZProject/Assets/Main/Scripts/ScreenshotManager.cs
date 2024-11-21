using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ScreenshotManager : MonoBehaviour
{
    public static Sprite TakeScreenshot(Camera screenshotCamera)
    {
        int screenshotWidth = 1000;  // Set desired width
        int screenshotHeight = 1000; // Set desired height
        RenderTexture tex = new RenderTexture(screenshotWidth, screenshotHeight, 24, RenderTextureFormat.ARGB32);
        screenshotCamera.targetTexture = tex;

        RenderTexture.active = tex;
        screenshotCamera.Render();

        Texture2D screenShot = new Texture2D(screenshotWidth, screenshotHeight, TextureFormat.RGBA32, false);
        screenShot.ReadPixels(new Rect(0, 0, screenshotWidth, screenshotHeight), 0, 0);
        screenShot.Apply();

        Sprite screenie = TextureToSprite(screenShot);

        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(tex);

        return screenie;
    }
    public static Sprite TextureToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        
    }
}
