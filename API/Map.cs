using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class Map
{
    Texture2D baseMap;

    bool[,] collision;

    public class ParsedMapXML
    {
        public string baseName;
        public string collisionName;
        public string objectMask;
        public string objectLayer;
        public Color collisionKey;
        public Color wallKey;
        public Color baseKey;
        public Color yKey;

    }

    public class MapObject
    {
        public Texture2D t;
        public int x;
        public int base_y;
        public int h;
    }

    LinkedList<MapObject>[] allObjects;

    public Map(string pathToMap)
    {
        ParsedMapXML pmx  = readMapXML(pathToMap);

        baseMap = Utils.TextureLoader(Path.GetDirectoryName(pathToMap) + @"\" + pmx.baseName);

        Texture2D collisionMap = Utils.TextureLoader(Path.GetDirectoryName(pathToMap) + @"\" + pmx.collisionName);
        Color[] collisionColorsMap = new Color[collisionMap.Width * collisionMap.Height];
        collisionMap.GetData<Color>(collisionColorsMap);

        createCollisionMap(collisionMap, collisionColorsMap, pmx.collisionKey, pmx.wallKey);

        Texture2D objectMap = Utils.TextureLoader(Path.GetDirectoryName(pathToMap) + @"\" + pmx.objectLayer);

        Color[] objectMapColor = new Color[objectMap.Width * objectMap.Height];
        objectMap.GetData<Color>(objectMapColor);

        Texture2D objectMaskTexture = Utils.TextureLoader(Path.GetDirectoryName(pathToMap) + @"\" + pmx.objectMask);

        Color[] objectMaskColor = new Color[objectMap.Width * objectMap.Height];
        objectMaskTexture.GetData<Color>(objectMaskColor);

        createObjectMap(objectMapColor, objectMaskColor, pmx.baseKey, pmx.yKey);
    }

    public void drawBase(int x, int y, int w, int h)
    {
        Utils.sb.Draw(baseMap, new Rectangle(0, 0, w, h), new Rectangle(x, y, w, h), Color.White);
    }

    public void drawObjectLine(int x, int y, int w, int h, int step)
    {
        if (step + y >= 0 && step + y < (baseMap.Height))
        {
            foreach (MapObject o in allObjects[step + y])
            {
                Utils.sb.Draw(o.t, new Rectangle(-x + o.x, -y + o.base_y +- (o.h - 1), 1, o.h), new Rectangle(0, 0, 1, o.h), Color.White);
            }
        }
    }

    public bool collide(int x, int y)
    {
         return x < 0 || y < 0 || x >= baseMap.Width || y >= baseMap.Height || collision[x, y];
    }

    public void createCollisionMap(Texture2D collisionMap, Color[] colorsMap, Color collisionKey, Color wallKey)
    {
        collision = new bool[collisionMap.Width, collisionMap.Height];
    
        for (int x = 0; x != collisionMap.Width; x++)
        {
            for (int y = 0; y != collisionMap.Height; y++)
            {
                Color c = colorsMap[x + (y * collisionMap.Width)];
                collision[x, y] = c.Equals(wallKey) || c.Equals(collisionKey);
            }
        }
    }

    public void createObjectMap(Color[] objectLayer, Color[] objectMask, Color baseKey, Color yKey)
    { 
        bool[,] objectBaseMask = new bool[baseMap.Width, baseMap.Height];
        bool[,] objectHeightMask = new bool[baseMap.Width, baseMap.Height];

        populateBaseAndHeightMaps(objectMask, objectBaseMask, baseKey, objectHeightMask, yKey);

        allObjects = new LinkedList<MapObject>[baseMap.Height];
        //Now we have the base mask and height mask figured out. We need to find all of the bases, climb up the height, and map them to objects.
        for (int y = 0; y != baseMap.Height; y++)
        {
            allObjects[y] = new LinkedList<MapObject>();
            for (int x = 0; x != baseMap.Width ; x++)
            {

   
                if(objectBaseMask[x, y])
                {
                    int h = 1;
                    while(objectHeightMask[x,y-h])
                    {
                        h++;
                    }

                    Texture2D newObject = new Texture2D(Utils.gfx, 1, h);
                    Color[] colorData = new Color[h];

                    for (int dh = 0; dh != h; dh++)
                    {
                        //we know the object is 'h' pixels high from the base.
                        //we also know that the top is at y = 0.
                        colorData[dh] = objectLayer[x + ( (y +- ((h - 1) - dh)) * baseMap.Width)];  
                    }

                    newObject.SetData<Color>(colorData);

                    MapObject m = new MapObject();
                    m.x = x;
                    m.base_y = y;
                    m.t = newObject;
                    m.h = h;

                    allObjects[y].AddFirst(m);
                    

                }
                     

            }
        }
    }

    public ParsedMapXML readMapXML(String pathToMap)
    {
        ParsedMapXML pmx = new ParsedMapXML();
        XmlReader reader = XmlReader.Create(pathToMap);

        reader.ReadToFollowing("base");
        pmx.baseName = reader.ReadElementContentAsString();

        reader.ReadToFollowing("collision");
        pmx.collisionName = reader.ReadElementContentAsString();

        reader.ReadToFollowing("objectmask");
        pmx.objectMask = reader.ReadElementContentAsString();

        reader.ReadToFollowing("objectlayer");
        pmx.objectLayer = reader.ReadElementContentAsString();

        reader.ReadToFollowing("collisionKeyR");
        string collisionR = reader.ReadElementContentAsString();
        reader.ReadToFollowing("collisionKeyG");
        string collisionG = reader.ReadElementContentAsString();
        reader.ReadToFollowing("collisionKeyB");
        string collisionB = reader.ReadElementContentAsString();
        pmx.collisionKey = new Color(Int32.Parse(collisionR), Int32.Parse(collisionG), Int32.Parse(collisionB));

        reader.ReadToFollowing("wallKeyR");
        string wallR = reader.ReadElementContentAsString();
        reader.ReadToFollowing("wallKeyG");
        string wallG = reader.ReadElementContentAsString();
        reader.ReadToFollowing("wallKeyB");
        string wallB = reader.ReadElementContentAsString();
        pmx.wallKey = new Color(Int32.Parse(wallR), Int32.Parse(wallG), Int32.Parse(wallB));

        reader.ReadToFollowing("objectBaseKeyR");
        string objectBaseKeyR = reader.ReadElementContentAsString();
        reader.ReadToFollowing("objectBaseKeyG");
        string objectBaseKeyG = reader.ReadElementContentAsString();
        reader.ReadToFollowing("objectBaseKeyB");
        string objectBaseKeyB = reader.ReadElementContentAsString();
        pmx.baseKey = new Color(Int32.Parse(objectBaseKeyR), Int32.Parse(objectBaseKeyG), Int32.Parse(objectBaseKeyB));

        reader.ReadToFollowing("objectHeightKeyR");
        string objectHeightKeyR = reader.ReadElementContentAsString();
        reader.ReadToFollowing("objectHeightKeyG");
        string objectHeightKeyG = reader.ReadElementContentAsString();
        reader.ReadToFollowing("objectHeightKeyB");
        string objectHeightKeyB = reader.ReadElementContentAsString();
        pmx.yKey = new Color(Int32.Parse(objectHeightKeyR), Int32.Parse(objectHeightKeyG), Int32.Parse(objectHeightKeyB));

        reader.Close();
        return pmx;
    }

    private void populateBaseAndHeightMaps(Color[] objectMask, bool[,] objectBaseMask, Color baseKey, bool[,] objectHeightMask, Color yKey)
    {
        for (int x = 0; x != baseMap.Width; x++)
        {
            for (int y = 0; y != baseMap.Height; y++)
            {
                Color c = objectMask[x + (y * baseMap.Width)];
                objectBaseMask[x, y] = c.Equals(baseKey);
                objectHeightMask[x, y] = c.Equals(yKey);
            }
        }
    }
}

