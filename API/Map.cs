using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Xml;

public class Map
{
    Texture2D baseMap;
    bool[,] collision;

    public Map(string pathToMap)
    {

        XmlReader reader = XmlReader.Create(pathToMap);

        reader.ReadToFollowing("base");
        string baseName = reader.ReadElementContentAsString();
        reader.ReadToFollowing("collision");
        string collisionName = reader.ReadElementContentAsString();

        reader.ReadToFollowing("collisionKeyR");
        string collisionR = reader.ReadElementContentAsString();
        reader.ReadToFollowing("collisionKeyG");
        string collisionG= reader.ReadElementContentAsString();
        reader.ReadToFollowing("collisionKeyB");
        string collisionB = reader.ReadElementContentAsString();

        reader.ReadToFollowing("wallKeyR");
        string wallR = reader.ReadElementContentAsString();
        reader.ReadToFollowing("wallKeyG");
        string wallG = reader.ReadElementContentAsString();
        reader.ReadToFollowing("wallKeyB");
        string wallB = reader.ReadElementContentAsString();

        reader.Close();

        baseMap = Utils.TextureLoader(Path.GetDirectoryName(pathToMap) + @"\" + baseName);
 
        //parse collisionMap
        Texture2D collisionMap = Utils.TextureLoader(Path.GetDirectoryName(pathToMap) + @"\" + collisionName);

        Color[] colorsMap = new Color[collisionMap.Width * collisionMap.Height];
        collisionMap.GetData<Color>(colorsMap);

        Color wallKey = new Color(Int32.Parse(wallR), Int32.Parse(wallG), Int32.Parse(wallB));
        Color collisionKey = new Color(Int32.Parse(collisionR), Int32.Parse(collisionG), Int32.Parse(collisionB));

        collision = new bool[collisionMap.Width,collisionMap.Height];

        for (int x = 0; x != collisionMap.Width; x++)
        {
            for (int y = 0; y != collisionMap.Height; y++)
            {
                Color c = colorsMap[x + (y * collisionMap.Width)];

                if (c.Equals(wallKey) || c.Equals(collisionKey))
                {
                    collision[x, y] = true;
                }
            }
        }

        

    }

    public void draw(int x, int y, int w, int h)
    {
        Utils.sb.Draw(baseMap, new Rectangle(0, 0, w, h), new Rectangle(x, y, w, h), Color.White);
    }

    public bool collide(int x, int y)
    {
        //check for big numbers to
         return x < 0 || y < 0 || collision[x, y];
    }


}

