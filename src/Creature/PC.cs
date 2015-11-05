﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class PC : Creature
{
    public RaceUtils.Race race { get; private set; }
    AnimationSet anim;
    enum Dir { UP,RIGHT,DOWN,LEFT};
    Dir dir = Dir.DOWN;
    public string name { get; private set; }

    public PC(int racei)
    {
        //TODO Clean
        x = 225;
        y = 500;
        race = (RaceUtils.Race)racei;
        switch (race)
        {
            case RaceUtils.Race.Pirate:
                anim = new AnimationSet(@"creatures\pc\pirate\anim.xml");
                stats = new StatSet(@"creatures\pc\pirate\stat.xml");
                size = 12;
                name = "NANCY";
                break;
            case RaceUtils.Race.Dragon:
                anim = new AnimationSet(@"creatures\pc\dragon\anim.xml");
                stats = new StatSet(@"creatures\pc\dragon\stat.xml");
                size = 16;
                name = "ALESSIA";
                break;
            case RaceUtils.Race.Meximage:
                anim = new AnimationSet(@"creatures\pc\meximage\anim.xml");
                stats = new StatSet(@"creatures\pc\meximage\stat.xml");
                size = 14;
                name = "JESUS";
                break;
            case RaceUtils.Race.Ninja:
                anim = new AnimationSet(@"creatures\pc\ninja\anim.xml");
                stats = new StatSet(@"creatures\pc\ninja\stat.xml");
                size = 12;
                name = "BIP";
                break;


        }

        //fix!
        anim.speed = 5;
        HP = stats.vit();
        MP = stats.aff();
        FA = MAX_FATIGUE;
    }

    public void update(Input i, GameTime delta)
    {
        //physically move character.
        move(i,delta);
        animateMove(i);

        if (i.pressedUSE)
            use();
    
        anim.update();
    }

    //Consider a camera
    public void draw(int cameraX, int cameraY )
    {
        anim.draw(x + - cameraX, y + - cameraY);
    }

    private void prioritize(int dir)
    {
        int oldDirIndex = 0;

        for(int i = 0; i != prioritySet.Length; i++)
        {
            if(prioritySet[i] == dir)
            {
                oldDirIndex = i;
                break;
            }
        }

        for (int i = oldDirIndex; i != 0; i--)
        {
            prioritySet[i] = prioritySet[i - 1];
        }

        prioritySet[0] = dir;
    }

    private bool canMove(int dir)
    {
        switch (dir)
        {
            case 0:
                for (int top = x; top != x + size; top++)
                {
                    if (InGame.map.collide(top,y - 1))
                        return false;
                }
                return true;
            case 2:
                for (int bottom = x; bottom != x + size; bottom++)
                {
                    if (InGame.map.collide(bottom, y + size))
                        return false;
                }
                return true;

            case 1:
                for (int right = y; right!= y + size; right++)
                {
                    if (InGame.map.collide(x+size, right))
                        return false;
                }
                return true;

            case 3:
                for (int left = y; left != y + size; left++)
                {
                    if (InGame.map.collide(x - 1,left))
                        return false;
                }
                return true;

            default:
                return false;
        }
    }
    
    private void move(Input i,GameTime delta)
    {
        double pixelsPerSecond = StatSet.baseSpeed + (StatSet.skillSpeed * stats.spd());
        double pixelsToMove = pixelsPerSecond * delta.ElapsedGameTime.TotalSeconds;
        int milliPixelsToMove = (int)(DELTA_SCALE * pixelsToMove);

        //look for fresh new inputs.
        if (i.pressedUP)
        {
            prioritize(0);
        }
        if (i.pressedRIGHT)
        {
            prioritize(1);
        }
        if (i.pressedDOWN)
        {
            prioritize(2);
        }
        if (i.pressedLEFT)
        {
            prioritize(3);
        }

        //we need to decide what is actually being held down.

        //first things first: get rid of opposing direction
        int[] xy = new int[2] { -1, -1 };

        for (int x = 0; x != prioritySet.Length; x++)
        {
            if ((prioritySet[x] == 1 && i.RIGHT) || (prioritySet[x] == 3 && i.LEFT))
            {
                xy[0] = prioritySet[x];
                break;
            }
        }

        for (int y = 0; y != prioritySet.Length; y++)
        {
            if ((prioritySet[y] == 0 && i.UP) || (prioritySet[y] == 2 && i.DOWN))
            {
                xy[1] = prioritySet[y];
                break;
            }
        }

        if (xy[0] == 3 && xy[1] == 0)
        {
            dx = dx - (int)(milliPixelsToMove * rad2over2);
            dy = dy - (int)(milliPixelsToMove * rad2over2);
            dir = Dir.LEFT;
        }
        else if (xy[0] == 3 && xy[1] == 2)
        {
            dx = dx - (int)(milliPixelsToMove * rad2over2);
            dy = dy + (int)(milliPixelsToMove * rad2over2);
            dir = Dir.LEFT;
        }
        else if (xy[0] == 1 && xy[1] == 0)
        {
            dx = dx + (int)(milliPixelsToMove * rad2over2);
            dy = dy - (int)(milliPixelsToMove * rad2over2);
            dir = Dir.RIGHT;
        }
        else if (xy[0] == 1 && xy[1] == 2)
        {
            dx = dx + (int)(milliPixelsToMove * rad2over2);
            dy = dy + (int)(milliPixelsToMove * rad2over2);
            dir = Dir.RIGHT;
        }

        else if (xy[0] == 1)
        {
            dx = dx + milliPixelsToMove;
            dir = Dir.RIGHT;
        }
        else if (xy[0] == 3)
        {
            dx = dx - milliPixelsToMove;
            dir = Dir.LEFT;
        }
        else if (xy[1] == 0)
        {
            dy = dy - milliPixelsToMove;
            dir = Dir.UP;
        }
        else if (xy[1] == 2)
        {
            dy = dy + milliPixelsToMove;
            dir = Dir.DOWN;
        }

    }

    private Item getItemBelow(LinkedList<Item> floorItems)
    {
        foreach(Item i in floorItems)
        {
            if (this.collide(i))
                return i;
        }
        return null;
    }

    private void animateMove(Input i)
    {
        if (i.LEFT || i.RIGHT || i.UP || i.DOWN)
        {
            //TODO Enumerate
            anim.autoAnimate("walk", (int)dir);
        }
        else
        {
            anim.hold("idle", 0, (int)dir);
        }

        //check to see if my dx would cause a collision.
        if (dx < 0 && !canMove(3)) dx = 0;
        if (dx >= DELTA_SCALE && !canMove(1)) dx = DELTA_SCALE - 1;
        if (dy < 0 && !canMove(0)) dy = 0;
        if (dy >= DELTA_SCALE && !canMove(2)) dy = DELTA_SCALE - 1;

        while (dx >= DELTA_SCALE) { dx = dx - DELTA_SCALE; x++; } //3 , 1100 -> 4, 100
        while (dy >= DELTA_SCALE) { dy = dy - DELTA_SCALE; y++; }
        while (dx < 0) { dx = dx + DELTA_SCALE; x--; } //3, -100 -> 2, 900
        while (dy < 0) { dy = dy + DELTA_SCALE; y--; }

    }

    private void use()
    {
  //      if()



    }

}