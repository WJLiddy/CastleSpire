using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class Creature
{
    StatSet set;
    //In this game engine, creatures are graphically locked to a discrete grid which is the set of all whole numbers.
    //Collisions are done based on this as well.
    //Howeever, for movement purposes, there exists a delta-X which is based on a 1/1000 (delta-scale) scale. Think millipixels. That way characters can move at different 'speeds'
    //This accumulator is really only used for movement.
    public uint x { get; private set; }
    public uint y { get; private set; }

    //for movement/speed purposes
    private static readonly uint DELTA_SCALE = 1000;
    private uint dx;
    private uint dy;
    
    //size of boundingbox of character
    public uint size { get; private set; }


   

    
}

