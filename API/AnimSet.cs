using System;
using System.Collections;
using System.IO;
using System.Xml;

public class AnimSet
{

    //All of the animations in this set.
    private Hashtable anims;
    //The current animation we're on.
    public AnimSheet currentAnim { get; private set; } = null;

    //Current frame we are on.
    private int frame = 0;
    //Current direction we are on.
    private int dir = 0;
    //Ticks until next frame.
    private int ticksLeftToNext = 0;
    //If true, automatically animate on tick.
    private bool animate = false;

    public AnimSet(String pathToAnimUrl)
    {
        anims = new Hashtable();
        //Loads in all the animsheets, picking the last one to be the default.
        loadAnimSets(pathToAnimUrl);

        //Overrides default if "idle" is in the set.
        if (anims["idle"] != null)
            hold("idle", 0, 0);
    }

    //freezes to this animation at this frame and direction.
    public void hold(string anim, int frame, int dir)
    {
        currentAnim = (AnimSheet)anims[anim];
        animate = false;
        this.frame = frame;
        this.dir = dir;
    }

    //starts animation. If animation is already in progress, does nothing
    public void autoAnimate(string anim, int dir)
    {
        currentAnim = (AnimSheet)anims[anim];
        this.dir = dir;

        if (!animate)
        {
            ticksLeftToNext = currentAnim.speed;
            animate = true;
            frame = 0;
        }
    }

    public void update()
    {
        if (animate)
        {
            ticksLeftToNext--;

            if (ticksLeftToNext == 0)
            {
                frame = (frame + 1) % currentAnim.frameCount;
                ticksLeftToNext = currentAnim.speed;
            }
        }
    }

    public void draw(int x, int y, int w, int h)
    {
        currentAnim.draw(frame,dir,x,y,w,h);
    }

    public void draw(int x, int y)
    {
        currentAnim.draw(frame, dir, x, y);
    }

    public void loadAnimSets(String pathToAnimUrl)
    {
        XmlReader reader = XmlReader.Create(pathToAnimUrl);
        reader.ReadToFollowing("AnimSheet");
        while (!reader.EOF)
        {
            String animSheetName = reader.ReadElementContentAsString();
            anims.Add(animSheetName, new AnimSheet(Path.GetDirectoryName(pathToAnimUrl) + @"\" + animSheetName + ".xml"));
            hold(animSheetName, 0, 0);
            reader.ReadToFollowing("AnimSheet");
        }
        reader.Close();
    }
}

