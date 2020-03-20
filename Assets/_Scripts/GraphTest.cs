using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphTest : MonoBehaviour
{
    [Header("GraphCongif")]
    Vector2 xLimits;
    Vector2 yLimits;

    [Header("SpriteConfig")]
    public Vector2 spriteSize;
    public Image image;

    Texture2D texture;
    Rect rect;

    private void Awake()
    {
    }

    private void Start()
    {
        rect = new Rect(0, 0, spriteSize.x, spriteSize.y);
    }

    void GenerateGraph(float[] dataY)
    {
        Color[] colors = new Color[(int)(spriteSize.x * spriteSize.y)];
        for (int i = 0; i < colors.Length; i++)
        {
            int coordX = i - (int)(i/spriteSize.x)*(int)spriteSize.x;
            int coordY = (int)(i / spriteSize.x);

            float x = (xLimits.y - xLimits.x) * ((float)coordX / (float)spriteSize.x) + xLimits.x;
            float y = (yLimits.y - yLimits.x) * ((float)coordY / (float)spriteSize.y) + yLimits.x;

            if (y < dataY[(int)x])
            {
                colors[i] = Color.red;
            }
            else
            {
                colors[i] = Color.white;
            }
               
        }
        texture = new Texture2D((int)spriteSize.x, (int)spriteSize.y);
        texture.SetPixels(colors);

        texture.Apply();

        Sprite sprite = Sprite.Create(texture, rect, new Vector2(0, 0));
        image.sprite = sprite;
    }

    public void RefreshData(float[] dataY,Vector2 yLimits)
    {
        this.yLimits = yLimits;

        xLimits.x = 0;
        xLimits.y = dataY.Length-1;

        GenerateGraph(dataY);
    }

    float TestFunction(float x)
    {
        return Mathf.Sin(x)/x;
    }
}
