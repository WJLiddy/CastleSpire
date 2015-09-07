using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class Creature
{


    public static readonly double rad2over2 = .7071067812;
    
    protected int[] prioritySet = new int[4]{ 0, 1, 2, 3 };

    StatSet set;
    //In this game engine, creatures are graphically locked to a discrete grid which is the set of all whole numbers.
    //Collisions are done based on this as well.
    //Howeever, for movement purposes, there exists a delta-X which is based on a 1/1000 (delta-scale) scale. Think millipixels. That way characters can move at different 'speeds'
    //This accumulator is really only used for movement.
    public int x { get; protected set; }
    public int y { get; protected set; }

    //for movement/speed purposes
    public static readonly int DELTA_SCALE = 1000;
    protected int dx { get; set; }
    protected int dy { get; set; }
    
    //size of boundingbox of character
    public int size { get; protected set; }
   
}

