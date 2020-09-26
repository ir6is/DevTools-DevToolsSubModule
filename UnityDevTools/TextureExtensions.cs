using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class  TextureExtensions
{
    public static Texture2D DrawCircle(this Texture2D tex, Color color, int x, int y, int radius = 3,float xScale=1,float yScale=1)
    {
        float rSquared = radius * radius;

        //Mathf.RoundToInt(xScale* radius)
        //    Mathf.RoundToInt(yScale * radius)
        for (int u = x - Mathf.RoundToInt( radius/ xScale); u < x + Mathf.RoundToInt(radius / xScale) + 1; u++)
        {
            for (int v = y - Mathf.RoundToInt(radius / yScale); v < y + Mathf.RoundToInt(radius / yScale) + 1; v++)
            {
                var xSqr = (x - u) * (x - u);
                var ySqr = (y - v) * (y - v);

                xSqr *= Mathf.RoundToInt((xScale * xScale));
                ySqr *= Mathf.RoundToInt((yScale * yScale));
                if (xSqr + ySqr < rSquared)
                {
                    if (u >= 0 && u < tex.width)
                    {
                        if (v >= 0 && v < tex.height)
                        {
                            tex.SetPixel(u, v, color);
                        }
                    }
                }
            }

        }

        return tex;
    }

    public static Texture2D CreateCopy(this Texture2D texture)
    {
        //var newTexture = new Texture2D(texture.width, texture.height, texture.format, false);
        var newTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        var pixels= texture.GetPixels();
        newTexture.SetPixels(pixels);
        newTexture.Apply();
        return newTexture;
    }
}
