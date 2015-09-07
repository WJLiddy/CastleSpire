using System;
using System.Collections;
using System.IO;
using System.Xml;

public class AnimSet
{

    private Hashtable anims;
    public AnimSheet currentAnim { get; private set; } = null;
    private int frame = 0;
    private int dir = 0;
    private int framesLeftToNext = 0;
    private bool animate = false;

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

    public void startAnim(string anim, int dir)
    {
        currentAnim = (AnimSheet)anims[anim];
        this.dir = dir;

        if (!animate)
        {
            framesLeftToNext = currentAnim.speed;
            animate = true;
            frame = 0;
        }
    }

    public void update()
    {
        if (animate)
        {
            framesLeftToNext--;

            if (framesLeftToNext == 0)
            {
                frame = (frame + 1) % currentAnim.frameCount;
                framesLeftToNext = currentAnim.speed;
            }
        }

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

