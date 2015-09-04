using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Xml;

//An anim sheet is basically a Texture2d with frames. 
public class AnimSheet
{
    public Texture2D sheet { get; private set; }
    public int frameWidth { get; private set; }
    public int frameHeight { get; private set; }
    public int frameCount { get; private set; }
    public int offsetX { get; private set; }
    public int offsetY { get; private set; }
    public int speed { get; private set; }
    
    public AnimSheet(String pathToSheetXML)
    {
        XmlReader reader = XmlReader.Create(pathToSheetXML);

        reader.ReadToFollowing("frameWidth");
        frameWidth = reader.ReadElementContentAsInt();

        reader.ReadToFollowing("frameHeight");
        frameHeight = reader.ReadElementContentAsInt();

        reader.ReadToFollowing("frameCount");
        frameCount = reader.ReadElementContentAsInt();

        reader.ReadToFollowing("offsetX");
        offsetX = reader.ReadElementContentAsInt();

        reader.ReadToFollowing("offsetY");
        offsetY = reader.ReadElementContentAsInt();

        reader.ReadToFollowing("defaultSpeed");

        if (!reader.EOF)
            speed = reader.ReadElementContentAsInt();

        reader.Close();

        sheet = Utils.TextureLoader(Path.ChangeExtension(pathToSheetXML, ".png"));

    }
    
    //draw a given frame at a given place at a given size
    public void draw(int frame, int dir, int x, int y, int w, int h)
    {
        //destination, source
        Utils.sb.Draw(sheet, new Rectangle(x, y, w, h), new Rectangle(frame*frameWidth, dir*frameHeight, frameWidth, frameHeight), Color.White);
    }

    //draw a given frame at a given place at a given size
    public void draw(int frame, int dir, int x, int y)
    {
        //destination, source
        Utils.sb.Draw(sheet, new Rectangle(x, y, frameWidth, frameHeight), new Rectangle(frame * frameWidth, dir * frameHeight, frameWidth, frameHeight), Color.White);
    }




}

