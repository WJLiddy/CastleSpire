using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class ADFont
{
    private Texture2D font;
    private int height;
    static private int[] spaceBetween;

    public ADFont(string pathToFile)
    {
        font = Utils.TextureLoader(pathToFile);
        height = 7;
        spaceBetween = defaultSpacing();      
    }

    public void draw(string s, int x, int y, Color col, int scale = 1, bool outline = false){
        s = s.ToUpper();

        //where we are putting the next letter
        int xCursor = 0; 

        foreach(char c in s)
        {
            int value = (int)c;
            //Draw a black underline of the letter.
            if (outline)
            {
                Utils.sb.Draw(font, new Rectangle(x + scale + xCursor, y, scale * spaceBetween[value], scale * height), new Rectangle(0, value * height, spaceBetween[value], height), Color.Black);
                Utils.sb.Draw(font, new Rectangle(x + -scale + xCursor, y, scale * spaceBetween[value], scale * height), new Rectangle(0, value * height, spaceBetween[value], height), Color.Black);
                Utils.sb.Draw(font, new Rectangle(x + xCursor, y + scale, scale * spaceBetween[value], scale * height), new Rectangle(0, value * height, spaceBetween[value], height), Color.Black);
                Utils.sb.Draw(font, new Rectangle(x + xCursor, y - scale , scale * spaceBetween[value], scale * height), new Rectangle(0, value * height, spaceBetween[value], height), Color.Black);
            }

            Utils.sb.Draw(font, new Rectangle(x+xCursor, y, scale * spaceBetween[value], scale * height), new Rectangle(0, value * height, spaceBetween[value], height), col);
 
            xCursor += scale * (1 + spaceBetween[value] + (outline? 2 : 0));
        }
    }

    public static int getScaleWidth(String s,bool outline)
    {
        s = s.ToUpper();
        int width = 0;

        foreach (char c in s)
        {
            width += (1 + ADFont.spaceBetween[(int)c] + (outline ? 2 : 0));
        }
        return width;
   }

    public static int[] defaultSpacing()
    {
        return new int[128]{
            0,0,0,0,0,0,0,0,0,0, //0 - 9
            0,0,0,0,0,0,0,0,0,0, //10 - 9
            0,0,0,0,0,0,0,0,0,0, //20 - 9
            0,0,2,0,0,0,0,0,0,0, //30 - 9
            0,0,0,0,0,0,0,0,3,2, //40 - 9
            3,3,3,3,3,3,3,3,1,0, //50 - 9
            0,0,0,0,0,3,3,3,3,2, //60 - 9
            2,3,3,3,4,3,3,5,3,3, //70 - 9
            3,4,3,3,3,3,3,5,3,3, //80 - 9
            3,0,0,0,0,0,0,0,0,0, //90 - 9
            0,0,0,0,0,0,0,0,0,0, //100 - 9
            0,0,0,0,0,0,0,0,0,0, //110 - 9
            0,0,0,0,0,0,0,0};
    }
}
    
