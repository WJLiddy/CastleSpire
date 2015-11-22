using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class Light
{
    public Texture2D Texture { get; private set; }
    public int CenterX;
    public int CenterY;
    private float R;
    private float G;
    private float B;
    private float Strength;
    private int FallOff100;

    public Light(float r, float g, float b, int x, int y,int fallOff100)
    {
        Normalize(r, g, b);
        CenterX = x;
        CenterY = y;
        FallOff100 = fallOff100;
        Texture = GenerateLightTexture();
    }

    //generates linear fall-off lightsource.
    private Texture2D GenerateLightTexture()
    {
        Color[] lightMap = new Color[FallOff100 * FallOff100 * 4];

        Fill(lightMap, FallOff100*2, FallOff100*2, new Color(0,0,0,0f));

        for (int lit_x = 0; lit_x != (FallOff100*2); lit_x++)
        {
            for (int lit_y = 0; lit_y != (FallOff100*2); lit_y++)
            {
                if ((Utils.Dist(lit_x, FallOff100, lit_y, FallOff100) < FallOff100))
                {
                    float delta = (float)(FallOff100 - Utils.Dist(lit_x, FallOff100, lit_y, FallOff100)) / FallOff100;
                    lightMap[lit_x + ((FallOff100 * 2) * (lit_y))] = new Color(delta*R, delta*G, delta*B, Strength*delta);
                }
            }
        }

        Texture2D map = new Texture2D(Renderer.GraphicsDevice, FallOff100 * 2, FallOff100 * 2);
        map.SetData<Color>(lightMap);
        return map;
    }
    
    public static void Fill(Color[] lightMap, int w, int h, Color f)
    {
        for (int dx = 0; dx != w; dx++)
        {
            for (int dy = 0; dy != h; dy++)
            {
                lightMap[dx + (w * dy)] = f;
            }
        }
    }

    public int GetX()
    {
        return CenterX - FallOff100;
    }

    public int GetY()
    {
        return CenterY - FallOff100;
    }

    private void Normalize(float r, float g, float b)
    {
        Strength = Math.Max(Math.Max(r, g), b);
        float min = Math.Min(Math.Min(r, g), b);
        R = r - min;
        G = g - min;
        B = b - min;
    }
}
