using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class ADFont
{

    private Texture2D font;
    private int size;
    private int[] alphaSpace;
    private int[] puncSpace;
    private int[] numSpace;

    public ADFont(string pathToFile)
    {
        font = Utils.TextureLoader(pathToFile);

        //hardcoded because I'm an adult and I do what I want
         size = 7;
                                   //a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z
            alphaSpace = new int[26]{3,3,3,3,2,2,3,3,3,4,3,3,5,3,3,3,4,3,3,3,3,3,5,3,3,3};
            puncSpace = new int[4]{2,2,2,3};
            numSpace = new int[10]{4,2,4,4,4,4,4,4,4,4};
    }

    public void draw(string s, int x, int y, Color col, int scale = 1, bool outline = false){
        s = s.ToUpper();
        int xCursor = 0; 

        foreach(char c in s)
        { 
            int value = (int)c;
            if (value == (int)' ')
            {
                xCursor += 3*scale;
            }
            else
            {
                int letterIndex = c - 65;

                if (outline)
                {
                    Utils.sb.Draw(font, new Rectangle(x + scale + xCursor, y, scale * alphaSpace[letterIndex], scale * size), new Rectangle(0, letterIndex * size, alphaSpace[letterIndex], size), Color.Black);
                    Utils.sb.Draw(font, new Rectangle(x + -scale + xCursor, y, scale * alphaSpace[letterIndex], scale * size), new Rectangle(0, letterIndex * size, alphaSpace[letterIndex], size), Color.Black);
                    Utils.sb.Draw(font, new Rectangle(x + xCursor, y + scale, scale * alphaSpace[letterIndex], scale * size), new Rectangle(0, letterIndex * size, alphaSpace[letterIndex], size), Color.Black);
                    Utils.sb.Draw(font, new Rectangle(x + xCursor, y - scale , scale * alphaSpace[letterIndex], scale * size), new Rectangle(0, letterIndex * size, alphaSpace[letterIndex], size), Color.Black);
                }

                Utils.sb.Draw(font, new Rectangle(x+xCursor, y, scale * alphaSpace[letterIndex], scale * size), new Rectangle(0, letterIndex * size, alphaSpace[letterIndex], size), col);
 
                x += scale * (1 + alphaSpace[letterIndex] + (outline? 2 : 0));
            }

        }
    }



}
    
