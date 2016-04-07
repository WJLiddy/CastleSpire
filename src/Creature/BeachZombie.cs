using CastleUtils;
using System;
using System.Collections.Generic;

class BeachZombie : NPC
{
    private AnimationSet Anim;
    private Stack<AllDir> Plan;
    int LeftoverMilliPixels = 0;

    public BeachZombie(int x, int y)
    {
        Size = 10;
        X = x;
        Y = y;
        Anim = new AnimationSet(@"creatures\npc\zombie\anim.xml");
        Anim.Hold("idle", 0, 0);
        Stats = new StatSet(@"creatures\npc\zombie\stat.xml");
    }

    public override void Draw(AD2SpriteBatch sb, int cameraX, int cameraY, int floor)
    {
        if (Y + (Size - 1) != floor)
            return;
        Anim.Draw(sb, X + -cameraX, Y + -cameraY);
    }

    public override void Update(int ms)
    {
        LeftoverMilliPixels += getMilliPixelsToMove(ms);

        if(Plan != null)
        {
            while (Plan.Count > 0)
            {
                // Diagonal move.
                if (Math.Abs(DirectionUtils.getDeltaX(Plan.Peek())) + Math.Abs(DirectionUtils.getDeltaY(Plan.Peek())) == 2)
                {
                    if (LeftoverMilliPixels >= 1000*(Util.Rad2))
                    {
                        LeftoverMilliPixels -= (int)(1000*Util.Rad2);
                        X += DirectionUtils.getDeltaX(Plan.Peek());
                        Y += DirectionUtils.getDeltaY(Plan.Peek());
                        Plan.Pop();
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if (LeftoverMilliPixels >= 1000)
                    {
                        LeftoverMilliPixels -= 1000;
                        X += DirectionUtils.getDeltaX(Plan.Peek());
                        Y += DirectionUtils.getDeltaY(Plan.Peek());
                        Plan.Pop();
                    }
                    else
                    {
                        break;
                    }
                }
            }
            // we ran out of bulk moves, save delta millipixels
            if (Plan.Count == 0)
                LeftoverMilliPixels = 0;
        }
    }

    public override void UpdatePlan()
    {
        Plan = new Stack<AllDir>();
        PC target = InGame.PlayerList.First.Value;
        PixelSet ps = new PixelSet();
        ps.Add(target.X, target.Y);
        
        Plan = PathFinding.PixelPath(InGame.Map, X, Y, target.X, target.Y, ps, Size, 100);
        if(Plan == null)
        {
            Plan = LongPath.LongPathEstimation(InGame.Map, X, Y, target.X, target.Y, InGame.MapMesh, Size);
        }
    }
}