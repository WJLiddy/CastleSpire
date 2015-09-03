using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Xml;


//So, an anim sheet is simply a Texture2d that represents a rectangular object
public class AnimSheet
{
    public Texture2D sheet { get; private set; }
    public int frameWidth { get; private set; }
    public int frameHeight { get; private set; }
    public int frameCount { get; private set; }
    public int offsetX { get; private set; }
    public int offsetY { get; private set; }
    public int size { get; private set; }

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

        reader.ReadToFollowing("size");
        size = reader.ReadElementContentAsInt();

        reader.Close();

        sheet = Utils.TextureLoader(Path.ChangeExtension(pathToSheetXML, ".png"), GS.game.GraphicsDevice);

    }

    public void draw(SpriteBatch spriteBatch)
    {
        //destination, source
        spriteBatch.Draw(sheet, new Rectangle(0, 0, 800, 480), new Rectangle(0, 0, frameWidth, frameHeight), Color.White);
    }




}

