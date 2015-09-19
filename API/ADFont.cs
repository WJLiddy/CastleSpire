using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class ADFont
{

    private Texture2D font;
    private int size;
    static private int[] space;

    public ADFont(string pathToFile)
    {
        font = Utils.TextureLoader(pathToFile);
        
        //vertical height
         size = 7;

        //every glyph has a different matrix as represented by this array.

         space = new int[128]{
            0,0,0,0,0,0,0,0,0,0, //0 - 9
            0,0,0,0,0,0,0,0,0,0, //10 - 9
            0,0,0,0,0,0,0,0,0,0, //20 - 9
            0,0,0,0,0,0,0,0,0,0, //30 - 9
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

    public void draw(string s, int x, int y, Color col, int scale = 1, bool outline = false){
        s = s.ToUpper();
        int xCursor = 0; 

        foreach(char c in s)
        { 
            int value = (int)c;
            if (value == (int)' ')
            {
                //space exception
                xCursor += 3*scale;
            }
            else
            {

                if (outline)
                {
                    Utils.sb.Draw(font, new Rectangle(x + scale + xCursor, y, scale * space[value], scale * size), new Rectangle(0, value * size, space[value], size), Color.Black);
                    Utils.sb.Draw(font, new Rectangle(x + -scale + xCursor, y, scale * space[value], scale * size), new Rectangle(0, value * size, space[value], size), Color.Black);
                    Utils.sb.Draw(font, new Rectangle(x + xCursor, y + scale, scale * space[value], scale * size), new Rectangle(0, value * size, space[value], size), Color.Black);
                    Utils.sb.Draw(font, new Rectangle(x + xCursor, y - scale , scale * space[value], scale * size), new Rectangle(0, value * size, space[value], size), Color.Black);
                }

                Utils.sb.Draw(font, new Rectangle(x+xCursor, y, scale * space[value], scale * size), new Rectangle(0, value * size, space[value], size), col);
 
                x += scale * (1 + space[value] + (outline? 2 : 0));
            }

        }
    }

    public static int scaleWidth(String s,bool outline)
    {
        s = s.ToUpper();
        int width = 0;

        foreach (char c in s)
        {
            int value = (int)c;
            if (value == (int)' ')
            {
                //space exception
                width += 3;
            }
            else
            {
                width += (1 + ADFont.space[value] + (outline ? 2 : 0));
            }

        }
        return width;
   }
}
    
