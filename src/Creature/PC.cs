using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class PC : Creature
{
    public RaceUtils.Race Race { get; private set; }
    AnimationSet Anim;
    enum Dir { UP,RIGHT,DOWN,LEFT};
    Dir Direction = Dir.DOWN;
    public string Name { get; private set; }

    public PC(int racei)
    {
        //TODO Clean
        X = 225;
        Y = 500;
        Race = (RaceUtils.Race)racei;
        switch (Race)
        {
            case RaceUtils.Race.Pirate:
                Anim = new AnimationSet(@"creatures\pc\pirate\anim.xml");
                Stats = new StatSet(@"creatures\pc\pirate\stat.xml");
                Size = 12;
                Name = "NANCY";
                break;
            case RaceUtils.Race.Dragon:
                Anim = new AnimationSet(@"creatures\pc\dragon\anim.xml");
                Stats = new StatSet(@"creatures\pc\dragon\stat.xml");
                Size = 16;
                Name = "ALESSIA";
                break;
            case RaceUtils.Race.Meximage:
                Anim = new AnimationSet(@"creatures\pc\meximage\anim.xml");
                Stats = new StatSet(@"creatures\pc\meximage\stat.xml");
                Size = 14;
                Name = "JESUS";
                break;
            case RaceUtils.Race.Ninja:
                Anim = new AnimationSet(@"creatures\pc\ninja\anim.xml");
                Stats = new StatSet(@"creatures\pc\ninja\stat.xml");
                Size = 12;
                Name = "BIP";
                break;


        }

        //fix!
        Anim.Speed = 5;
        HP = Stats.Vit();
        MP = Stats.Aff();
        FA = MaxFatigue;
    }

    public void Update(Input i, int ms)
    {
        //physically move character.
        Move(i,ms);
        AnimateMove(i);

        if (i.PressedUse)
            Use();
    
        Anim.Update();
    }

    //Consider a camera
    public void Draw(AD2SpriteBatch sb, int cameraX, int cameraY )
    {
        Anim.Draw(sb, X + - cameraX, Y + - cameraY);
    }

    private void Prioritize(int dir)
    {
        int oldDirIndex = 0;

        for(int i = 0; i != PrioritySet.Length; i++)
        {
            if(PrioritySet[i] == dir)
            {
                oldDirIndex = i;
                break;
            }
        }

        for (int i = oldDirIndex; i != 0; i--)
        {
            PrioritySet[i] = PrioritySet[i - 1];
        }

        PrioritySet[0] = dir;
    }

    private bool CanMove(int dir)
    {
        switch (dir)
        {
            case 0:
                for (int top = X; top != X + Size; top++)
                {
                    if (InGame.Map.collide(top,Y - 1))
                        return false;
                }
                return true;
            case 2:
                for (int bottom = X; bottom != X + Size; bottom++)
                {
                    if (InGame.Map.collide(bottom, Y + Size))
                        return false;
                }
                return true;

            case 1:
                for (int right = Y; right!= Y + Size; right++)
                {
                    if (InGame.Map.collide(X+Size, right))
                        return false;
                }
                return true;

            case 3:
                for (int left = Y; left != Y + Size; left++)
                {
                    if (InGame.Map.collide(X - 1,left))
                        return false;
                }
                return true;

            default:
                return false;
        }
    }
    
    private void Move(Input i, int ms)
    {
        double pixelsPerSecond = StatSet.BaseSpeed + (StatSet.SkillSpeed * Stats.Spd());
        double pixelsToMove = pixelsPerSecond * ((double)ms / 1000);
        int milliPixelsToMove = (int)(DeltaScale * pixelsToMove);

        //look for fresh new inputs.
        if (i.PressedUp)
        {
            Prioritize(0);
        }
        if (i.PressedRight)
        {
            Prioritize(1);
        }
        if (i.PressedDown)
        {
            Prioritize(2);
        }
        if (i.PressedLeft)
        {
            Prioritize(3);
        }

        //we need to decide what is actually being held down.

        //first things first: get rid of opposing direction
        int[] xy = new int[2] { -1, -1 };

        for (int x = 0; x != PrioritySet.Length; x++)
        {
            if ((PrioritySet[x] == 1 && i.Right) || (PrioritySet[x] == 3 && i.Left))
            {
                xy[0] = PrioritySet[x];
                break;
            }
        }

        for (int y = 0; y != PrioritySet.Length; y++)
        {
            if ((PrioritySet[y] == 0 && i.Up) || (PrioritySet[y] == 2 && i.Down))
            {
                xy[1] = PrioritySet[y];
                break;
            }
        }

        if (xy[0] == 3 && xy[1] == 0)
        {
            DX = DX - (int)(milliPixelsToMove * Rad2Over2);
            DY = DY - (int)(milliPixelsToMove * Rad2Over2);
            Direction = Dir.LEFT;
        }
        else if (xy[0] == 3 && xy[1] == 2)
        {
            DX = DX - (int)(milliPixelsToMove * Rad2Over2);
            DY = DY + (int)(milliPixelsToMove * Rad2Over2);
            Direction = Dir.LEFT;
        }
        else if (xy[0] == 1 && xy[1] == 0)
        {
            DX = DX + (int)(milliPixelsToMove * Rad2Over2);
            DY = DY - (int)(milliPixelsToMove * Rad2Over2);
            Direction = Dir.RIGHT;
        }
        else if (xy[0] == 1 && xy[1] == 2)
        {
            DX = DX + (int)(milliPixelsToMove * Rad2Over2);
            DY = DY + (int)(milliPixelsToMove * Rad2Over2);
            Direction = Dir.RIGHT;
        }

        else if (xy[0] == 1)
        {
            DX = DX + milliPixelsToMove;
            Direction = Dir.RIGHT;
        }
        else if (xy[0] == 3)
        {
            DX = DX - milliPixelsToMove;
            Direction = Dir.LEFT;
        }
        else if (xy[1] == 0)
        {
            DY = DY - milliPixelsToMove;
            Direction = Dir.UP;
        }
        else if (xy[1] == 2)
        {
            DY = DY + milliPixelsToMove;
            Direction = Dir.DOWN;
        }

    }

    private Item GetItemBelow(LinkedList<Item> floorItems)
    {
        foreach(Item i in floorItems)
        {
            if (this.collide(i))
                return i;
        }
        return null;
    }

    private void AnimateMove(Input i)
    {
        if (i.Left || i.Right || i.Up || i.Down)
        {
            //TODO Enumerate
            Anim.AutoAnimate("walk", (int)Direction);
        }

        else
        {
            Anim.Hold("idle", 0, (int)Direction);
        }

        //check to see if my dx would cause a collision.
        if (DX < 0 && !CanMove(3)) DX = 0;
        if (DX >= DeltaScale && !CanMove(1)) DX = DeltaScale - 1;
        if (DY < 0 && !CanMove(0)) DY = 0;
        if (DY >= DeltaScale && !CanMove(2)) DY = DeltaScale - 1;

        while (DX >= DeltaScale) { DX = DX - DeltaScale; X++; } //3 , 1100 -> 4, 100
        while (DY >= DeltaScale) { DY = DY - DeltaScale; Y++; }
        while (DX < 0) { DX = DX + DeltaScale; X--; } //3, -100 -> 2, 900
        while (DY < 0) { DY = DY + DeltaScale; Y--; }

    }

    private void Use()
    {
  //      if()



    }

}
