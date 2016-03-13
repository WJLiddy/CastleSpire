using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class Item : Entity
{
    private Texture2D Texture;
    private Texture2D Outline;
    private int OffsetX;
    private int OffsetY;
    private int HandX;
    private int HandY;

    public Item(String pathToXML,int x,int y)
    {
        ReadMapXML(pathToXML);
        Texture = Utils.TextureLoader(Path.ChangeExtension(pathToXML, ".png"));
        makeOutline();
        this.X = x;
        this.Y = y;
    }

    public void ReadMapXML(String url)
    {
        XmlReader reader = XmlReader.Create(url);

        reader.ReadToFollowing("x");
        string xOffParse = reader.ReadElementContentAsString();
        OffsetX = Int32.Parse(xOffParse);

        reader.ReadToFollowing("y");
        string yOffParse = reader.ReadElementContentAsString();
        OffsetY = Int32.Parse(yOffParse);

        reader.ReadToFollowing("size");
        string wParse = reader.ReadElementContentAsString();
        Size = Int32.Parse(wParse);

        reader.ReadToFollowing("handX");
        string handXParse = reader.ReadElementContentAsString();
        HandX = Int32.Parse(handXParse);

        reader.ReadToFollowing("handY");
        string handYParse = reader.ReadElementContentAsString();
        HandY = Int32.Parse(handYParse);

        reader.Close();
    }

    public void DrawOnFloor(AD2SpriteBatch sb, int camX, int camY)
    {
        sb.Draw(Texture, new Rectangle((-camX + X + -OffsetX), (-camY + Y + -OffsetY),Texture.Width/4,Texture.Height), new Rectangle(16, 0, Texture.Width/4, Texture.Height),Color.White);
    }

    public void DrawAlone(AD2SpriteBatch sb, int x, int y)
    {
        sb.Draw(Texture, new Rectangle(x, y, Texture.Width / 4, Texture.Height), new Rectangle(16, 0, Texture.Width / 4, Texture.Height), Color.White);
    }

    public static void DrawGlowingItems(AD2SpriteBatch sb, LinkedList<PC> activeCharacters, LinkedList<Item> items, int camX, int camY)
    {
        foreach (PC c in activeCharacters)
        {
            foreach (Item item in items)
            {
                if (item.collide(c))
                {
                    item.DrawOutline(sb, camX, camY);
                    break;
                }
            }

        }
    }

    public void DrawOutline(AD2SpriteBatch sb, int camX, int camY)
    {
        sb.Draw(Outline, new Rectangle((-camX + X + -OffsetX), (-camY + Y + 1 + -OffsetY), Texture.Width / 4, Texture.Height), new Rectangle(16, 0, Texture.Width / 4, Texture.Height), Color.White);
        sb.Draw(Outline, new Rectangle((-camX + X + 1 + -OffsetX), (-camY + Y + -OffsetY), Texture.Width / 4, Texture.Height), new Rectangle(16, 0, Texture.Width / 4, Texture.Height), Color.White);
        sb.Draw(Outline, new Rectangle((-camX + X + -OffsetX), (-camY + Y + -1 + -OffsetY), Texture.Width / 4, Texture.Height), new Rectangle(16, 0, Texture.Width / 4, Texture.Height), Color.White);
        sb.Draw(Outline, new Rectangle((-camX + X + -1 + -OffsetX), (-camY + Y + -OffsetY), Texture.Width / 4, Texture.Height), new Rectangle(16, 0, Texture.Width / 4, Texture.Height), Color.White);

    }

    public void makeOutline()
    {
        Outline = new Texture2D(Renderer.GraphicsDevice, Texture.Width, Texture.Height);
        Color[] itemTexture = new Color[Texture.Width * Texture.Height];
        Color[] outlineData = new Color[Texture.Width * Texture.Height];
        Texture.GetData<Color>(itemTexture);
        for(int i = 0; i != Texture.Width; i++)
        {
            for(int j = 0; j != Texture.Height; j++)
            {
                if (!itemTexture[i + Texture.Width * j].Equals(Color.Transparent))
                    outlineData[i + Texture.Width * j] = Color.Yellow;
            }
        }
        Outline.SetData<Color>(outlineData);
    }

}
