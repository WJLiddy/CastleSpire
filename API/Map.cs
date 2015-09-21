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

        XmlReader reader = XmlReader.Create(pathToMap);

        reader.ReadToFollowing("base");
        string baseName = reader.ReadElementContentAsString();
        reader.ReadToFollowing("collision");
        string collisionName = reader.ReadElementContentAsString();

        reader.ReadToFollowing("objectmask");
        string objectMask = reader.ReadElementContentAsString();
        reader.ReadToFollowing("objectlayer");
        string objectLayer = reader.ReadElementContentAsString();

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

        reader.ReadToFollowing("objectBaseKeyR");
        string objectBaseKeyR = reader.ReadElementContentAsString();
        reader.ReadToFollowing("objectBaseKeyG");
        string objectBaseKeyG = reader.ReadElementContentAsString();
        reader.ReadToFollowing("objectBaseKeyB");
        string objectBaseKeyB = reader.ReadElementContentAsString();

        reader.ReadToFollowing("objectHeightKeyR");
        string objectHeightKeyR = reader.ReadElementContentAsString();
        reader.ReadToFollowing("objectHeightKeyG");
        string objectHeightKeyG = reader.ReadElementContentAsString();
        reader.ReadToFollowing("objectHeightKeyB");
        string objectHeightKeyB = reader.ReadElementContentAsString();

        reader.Close();

        baseMap = Utils.TextureLoader(Path.GetDirectoryName(pathToMap) + @"\" + baseName);

        Texture2D collisionMap = Utils.TextureLoader(Path.GetDirectoryName(pathToMap) + @"\" + collisionName);
        Color[] collisionColorsMap = new Color[collisionMap.Width * collisionMap.Height];
        collisionMap.GetData<Color>(collisionColorsMap);


        //fetch wall keys
        Color wallKey = new Color(Int32.Parse(wallR), Int32.Parse(wallG), Int32.Parse(wallB));
        Color collisionKey = new Color(Int32.Parse(collisionR), Int32.Parse(collisionG), Int32.Parse(collisionB));

        createCollisionMap(collisionMap, collisionColorsMap, collisionKey, wallKey);



        Color baseKey = new Color(Int32.Parse(objectBaseKeyR), Int32.Parse(objectBaseKeyG), Int32.Parse(objectBaseKeyB));
        Color yKey = new Color(Int32.Parse(objectHeightKeyR), Int32.Parse(objectHeightKeyG), Int32.Parse(objectHeightKeyB));


        Texture2D objectMap = Utils.TextureLoader(Path.GetDirectoryName(pathToMap) + @"\" + objectLayer);

        Color[] objectMapColor = new Color[objectMap.Width * objectMap.Height];
        objectMap.GetData<Color>(objectMapColor);


        Texture2D objectMaskTexture = Utils.TextureLoader(Path.GetDirectoryName(pathToMap) + @"\" + objectMask);

        Color[] objectMaskColor = new Color[objectMap.Width * objectMap.Height];
        objectMaskTexture.GetData<Color>(objectMaskColor);

        createObjectMap(objectMap.Width, objectMap.Height, objectMapColor, objectMaskColor, baseKey, yKey);

    }

    public void drawBase(int x, int y, int w, int h)
    {
        Utils.sb.Draw(baseMap, new Rectangle(0, 0, w, h), new Rectangle(x, y, w, h), Color.White);
    }

    public void drawObjectLine(int x, int y, int w, int h, int step)
    {
        if (step + y >= 0 && step + y < baseMap.Height)
        {
            foreach (MapObject o in allObjects[step + y])
            {
                //      Utils.sb.Draw(o.t, new Rectangle(0, 0, 1, o.h), new Rectangle(x + o.x, y + -o.h + o.base_y, 1, o.h) , Color.White);
                Utils.sb.Draw(o.t, new Rectangle(-x + o.x, -y + o.base_y - o.h, 1, o.h), new Rectangle(0, 0, 1, o.h), Color.White);
            }
        }
      //  { }
       
            //desination, source
  //          if (objectBaseMask[dx,step])
    //        {
              //  Utils.sb.Draw(objectMap, new Rectangle(dx, step, 1, 1), new Rectangle(x + dx, y + step, 1, 1), Color.White);
      //      }
        //}
    }


    public bool collide(int x, int y)
    {
        //check for big numbers to
         return x < 0 || y < 0 || collision[x, y];
    }

    public void createCollisionMap(Texture2D collisionMap, Color[] colorsMap, Color collisionKey, Color wallKey)
    {

        collision = new bool[collisionMap.Width, collisionMap.Height];

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

    public void createObjectMap(int mapW, int mapH, Color[] objectLayer, Color[] objectMask, Color baseKey, Color yKey)
    {
        

        bool[,] objectBaseMask = new bool[mapW, mapH];
        bool[,] objectHeightMask = new bool[mapW, mapH];

        for (int x = 0; x != mapW; x++)
        {
            for (int y = 0; y != mapH; y++)
            {
                Color c = objectMask[x + (y * mapW)];
                
                if (c.Equals(baseKey))
                {
                    objectBaseMask[x, y] = true;
                }

                if (c.Equals(yKey))
                {
                    objectHeightMask[x, y] = true;
                }

            }
        }


        allObjects = new LinkedList<MapObject>[mapH];
        //Now we have the base mask and height mask figured out. We need to find all of the bases, climb up the height, and map them to objects.
        for (int y = 0; y != mapH; y++)
        {
            allObjects[y] = new LinkedList<MapObject>();
            for (int x = 0; x != mapW; x++)
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
                        colorData[dh] = objectLayer[x + ( (y +- ((h - 1) - dh)) * mapW)];  
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

}

