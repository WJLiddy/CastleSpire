using Microsoft.Xna.Framework;

class PC : Creature
{
    enum Races { Pirate, Dragon,Meximage,Ninja}

    Races race;
    AnimSet anim;
    StatSet stats;
    int dir = 0;

    public PC(int racei)
    {
        race = (Races)racei;
        switch (race)
        {
            case Races.Pirate:
                anim = new AnimSet(@"creatures\pc\pirate\anim.xml");
                stats = new StatSet(@"creatures\pc\pirate\stat.xml");
                break;
            case Races.Dragon:
                anim = new AnimSet(@"creatures\pc\dragon\anim.xml");
                stats = new StatSet(@"creatures\pc\dragon\stat.xml");
                break;
            case Races.Meximage:
                anim = new AnimSet(@"creatures\pc\meximage\anim.xml");
                stats = new StatSet(@"creatures\pc\meximage\stat.xml");
                break;
            case Races.Ninja:
                anim = new AnimSet(@"creatures\pc\ninja\anim.xml");
                stats = new StatSet(@"creatures\pc\ninja\stat.xml");
                break;
        }
        anim.startAnim("walk", 1);
    }

    public void update(Input i, GameTime delta)
    {
        //pixels per second
        double pixelsPerSecond = StatSet.baseSpeed + (StatSet.skillSpeed * stats.spd());
        double pixelsToMove = pixelsPerSecond * delta.ElapsedGameTime.TotalSeconds;
        int milliPixelsToMove = (int)(DELTA_SCALE * pixelsToMove);

        System.Console.WriteLine(milliPixelsToMove);

        //TODO make diagonal move same speed.
        if (i.LEFT)
        {
            dx = dx - milliPixelsToMove;
            dir = 3;
        }
        if (i.RIGHT)
        {
            dx = dx + milliPixelsToMove;
            dir = 1;
        }
        if (i.UP)
        {
            dy = dy - milliPixelsToMove;
            dir = 0;
        }
        if (i.DOWN)
        {
            dy = dy + milliPixelsToMove;
            dir = 2;
        }
        
        while (dx > DELTA_SCALE) { dx = dx - DELTA_SCALE; x++; } //3 , 1100 -> 4, 100
        while (dy > DELTA_SCALE) { dy = dy - DELTA_SCALE; y++; }
        while (dx < 0) {dx = dx + DELTA_SCALE; x--; } //3, -100 -> 2, 900
        while (dy < 0) {dy = dy + DELTA_SCALE; y--; }

        anim.update();
    }

    //Consider a camera
    public void draw()
    {
        anim.draw(x, y);
    }
    
}
