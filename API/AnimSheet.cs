using Microsoft.Xna.Framework.Graphics;
using System;


//So, an anim sheet is simply a Texture2d with some matrix properties. Pretty Simple.
class AnimSheet
{
    Texture2D sheet;
    uint frameWidth;
    uint frameHight;
    uint xFrameCount;
    uint yFrameCount;

    //now, all these animsheets represent an object, probably. 
    //To find where the top left corner of that object exists on the map, use offsetX, offsetY.
    uint offsetX;
    uint offsetY;

    public AnimSheet(String pathToSheetXML)
    {

        


    }



}

