using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class Item : Entity
{
    private Texture2D texture;
    private Texture2D outline;
    private int xOffset;
    private int yOffset;
    private int handX;
    private int handY;

    public Item(String pathToXML,int x,int y)
    {
        readMapXML(pathToXML);
        texture = Utils.TextureLoader(Path.ChangeExtension(pathToXML, ".png"));
        makeOutline();
        this.x = x;
        this.y = y;
    }

    public void readMapXML(String url)
    {
        XmlReader reader = XmlReader.Create(url);

        reader.ReadToFollowing("x");
        string xOffParse = reader.ReadElementContentAsString();
        xOffset = Int32.Parse(xOffParse);

        reader.ReadToFollowing("y");
        string yOffParse = reader.ReadElementContentAsString();
        yOffset = Int32.Parse(yOffParse);

        reader.ReadToFollowing("size");
        string wParse = reader.ReadElementContentAsString();
        size = Int32.Parse(wParse);

        reader.ReadToFollowing("handX");
        string handXParse = reader.ReadElementContentAsString();
        handX = Int32.Parse(handXParse);

        reader.ReadToFollowing("handY");
        string handYParse = reader.ReadElementContentAsString();
        handY = Int32.Parse(handYParse);

        reader.Close();
    }

    public void draw(int camX, int camY)
    {
        Utils.sb.Draw(texture, new Rectangle((-camX + x + -xOffset), (-camY + y + -yOffset),texture.Width/4,texture.Height), new Rectangle(16, 0, texture.Width/4, texture.Height),Color.White);
    }

    public static void drawGlowingItems(LinkedList<PC> activeCharacters, LinkedList<Item> items, int camX, int camY)
    {
        foreach (PC c in activeCharacters)
        {
            foreach (Item item in items)
            {
                if (item.collide(c))
                {
                    item.drawOutline(camX, camY);
                    break;
                }
            }

        }
    }

    public void drawOutline(int camX, int camY)
    {
        Utils.sb.Draw(outline, new Rectangle((-camX + x + -xOffset), (-camY + y + 1 + -yOffset), texture.Width / 4, texture.Height), new Rectangle(16, 0, texture.Width / 4, texture.Height), Color.White);
        Utils.sb.Draw(outline, new Rectangle((-camX + x + 1 + -xOffset), (-camY + y + -yOffset), texture.Width / 4, texture.Height), new Rectangle(16, 0, texture.Width / 4, texture.Height), Color.White);
        Utils.sb.Draw(outline, new Rectangle((-camX + x + -xOffset), (-camY + y + -1 + -yOffset), texture.Width / 4, texture.Height), new Rectangle(16, 0, texture.Width / 4, texture.Height), Color.White);
        Utils.sb.Draw(outline, new Rectangle((-camX + x + -1 + -xOffset), (-camY + y + -yOffset), texture.Width / 4, texture.Height), new Rectangle(16, 0, texture.Width / 4, texture.Height), Color.White);

    }

    public void makeOutline()
    {
        outline = new Texture2D(Utils.gfx, texture.Width, texture.Height);
        Color[] itemTexture = new Color[texture.Width * texture.Height];
        Color[] outlineData = new Color[texture.Width * texture.Height];
        texture.GetData<Color>(itemTexture);
        for(int i = 0; i != texture.Width; i++)
        {
            for(int j = 0; j != texture.Height; j++)
            {
                if (!itemTexture[i + texture.Width * j].Equals(Color.Transparent))
                    outlineData[i + texture.Width * j] = Color.Yellow;
            }
        }
        outline.SetData<Color>(outlineData);
    }

}
