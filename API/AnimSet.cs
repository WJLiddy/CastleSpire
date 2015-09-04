using System;
using System.Collections;
using System.IO;
using System.Xml;

public class AnimSet
{

    Hashtable anims;
    AnimSheet currentAnim = null;
    int frame = 0;
    int dir = 0;
    int framesLeftToNext = 0;
    bool animate = false;

    public AnimSet(String pathToAnimUrl)
    {
        anims = new Hashtable();

        XmlReader reader = XmlReader.Create(pathToAnimUrl);

        reader.ReadToFollowing("AnimSheet");
        while (!reader.EOF)
        {
            String animSheetName = reader.ReadElementContentAsString();
            anims.Add(animSheetName, new AnimSheet(Path.GetDirectoryName(pathToAnimUrl) + @"\" + animSheetName + ".xml"));
            reader.ReadToFollowing("AnimSheet");
        }
   
        reader.Close();

        //by default, "idle" will always be loaded
        //TODO: Better implementation.
        hold("idle", 0, 0);

    }

    public void hold(string anim, int frame, int dir)
    {
        currentAnim = (AnimSheet)anims[anim];
        animate = false;
        this.frame = frame;
        this.dir = dir;
    }

    public void update(int frames)
    {
        //lol
    }

    //camera operation here.
    public void draw(int x, int y, int w, int h)
    {
        currentAnim.draw(frame,dir,x,y,w,h);
    }

    public void draw(int x, int y)
    {
        currentAnim.draw(frame, dir, x, y);
    }
}

