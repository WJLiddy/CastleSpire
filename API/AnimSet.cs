using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    public void draw(SpriteBatch sb, int x, int y, int w, int h)
    {
        currentAnim.draw(sb,frame,dir,x,y,w,h);
    }

    public void draw(SpriteBatch sb, int x, int y)
    {
        currentAnim.draw(sb, frame, dir, x, y);
    }
}

