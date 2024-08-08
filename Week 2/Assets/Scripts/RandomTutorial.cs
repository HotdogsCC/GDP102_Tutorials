using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomTutorial : MonoBehaviour
{
    bool finished = false;
    public Texture2D targetTexture;
    public RawImage rawImage;
    public int width = 200;
    public int height = 200;

    // Start is called before the first frame update
    void Start()
    {
        targetTexture = new Texture2D(width, height);
        rawImage.texture = targetTexture;
        clearTexture(Color.black);
        targetTexture.Apply();
    }

    void clearTexture(Color color)
    {
        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                targetTexture.SetPixel(x, y, color);
            }
        }
    }

    void demonstrateRandom()
    {
        float stepSize = 0.01f;
        for(int i = 0; i < 100; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            Color pixel = targetTexture.GetPixel(x, y);
            if(pixel.r < 1)
            {
                pixel.r += stepSize;
            }
            else
            {
                finished = true;
            }
            targetTexture.SetPixel(x, y, pixel);
        }
        targetTexture.Apply();
    }
    long totalPoints = 0;
    long pointsInsideCircle = 0;
    void findPisUsingRandomNumbers()
    {
        float stepSize = 0.005f;
        for (int i = 0; i < 200; i++)
        {
            totalPoints++;
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            float fx = x - (width / 2.0f);
            float fy = y - (height / 2.0f);
            float distanceToCentre = Mathf.Pow(Mathf.Pow(fx, 2)+ Mathf.Pow(fy, 2), 0.5f);
            Color pixel = targetTexture.GetPixel(x, y);
            if(pixel.r >= 1 || pixel.b >= 1)
            {
                finished = true;
            }
            else if (distanceToCentre < (width / 2))
            {
                pointsInsideCircle++;
                pixel.b += stepSize;
            }
            else
            {
                pixel.r += stepSize;
            }
            targetTexture.SetPixel(x, y, pixel);
        }
        targetTexture.Apply();
    }
    double count = 0;
    // Update is called once per frame
    void Update()
    {
        if (!finished)
        {
            findPisUsingRandomNumbers();
            count++;
            if (count > 500)
            {
                count = 0;
                float PI = 4 * pointsInsideCircle / (float)totalPoints;
                Debug.Log(PI.ToString());
            }
        }
    }
}
