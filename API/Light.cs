using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class Light
{
    Color c;
    double str;
    int x;
    int y;
    int fallOff50;
    int fallOff100;

    public Light(Color c, int x, int y, int fallOff50, int fallOff100)
    {
        this.c = c;
        this.x = x;
        this.y = y;
        this.fallOff50 = fallOff50;
        this.fallOff100 = fallOff100;
    }

    //TODO: Add "strength" to light parameter? So some lights are brighter than others.

    //A lightmap is a texure that is drawn on top of everything to simulate lighting effects.
    public static Texture2D createLightMap(Color ambient, LinkedList<Light> Lights, int camX, int camY, int w, int h)
    {
        //first make a light map that is the size of the whole screen.
        Color[] lightMap = new Color[w * h];

        //Make it all ambient.
        for(int x = 0; x != w; x++)
        {
            for(int y = 0; y != h; y++)
            {
                lightMap[x + (w * y)] = ambient;
            }

        }

        //Now, what light do is blend with ambient based on strength.
        //crappy O(n^3) algo but we're just testing.

        //Make it all ambient.
        for (int x = 0; x != w; x++)
        {
            for (int y = 0; y != h; y++)
            {
                foreach(Light l in Lights)
                {
                    if (Utils.intdist(x, l.x, y, l.y) < l.fallOff100)
                    {
                        //draw with respect to camera.
                        //Cool Glitch Utils.mix(Utils.intdist(x, l.x, y, l.y),l.fallOff100,l.c,ambient);

                         lightMap[(x - camX) + (w * (y - camY))] = Utils.mix(l.fallOff100,Utils.intdist(x, l.x, y, l.y),l.c,ambient);
                    }

                }
            }
        }


        Texture2D lightMapTexture = new Texture2D(Utils.gfx,w,h);
        lightMapTexture.SetData<Color>(lightMap);
        return lightMapTexture;

    }

}
