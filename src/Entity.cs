using System.Collections.Generic;

public abstract class Entity
{

    public int X { get; protected set; }
    public int Y { get; protected set; }
    public int Size { get; protected set; }

    public bool Collide(Entity e)
    {
        return !(
        (Y + (Size - 1) < e.Y) ||
        (Y > e.Y + (e.Size - 1)) ||
        (X > e.X + (e.Size - 1) ||
        (X + (Size - 1) < e.X)));
    }

    protected bool NoWallCollide(int dir)
    {
        switch (dir)
        {
            case 0:
                for (int top = X; top != X + Size; top++)
                {
                    if (InGame.Map.Collide(top, Y - 1))
                        return false;
                }
                return true;
            case 2:
                for (int bottom = X; bottom != X + Size; bottom++)
                {
                    if (InGame.Map.Collide(bottom, Y + Size))
                        return false;
                }
                return true;

            case 1:
                for (int right = Y; right != Y + Size; right++)
                {
                    if (InGame.Map.Collide(X + Size, right))
                        return false;
                }
                return true;

            case 3:
                for (int left = Y; left != Y + Size; left++)
                {
                    if (InGame.Map.Collide(X - 1, left))
                        return false;
                }
                return true;

            default:
                return false;
        }
    }

    //check that, if our X and Y was moved by this much, the move would still be valid.
    protected bool NoEntityCollide(int dx, int dy, List<Entity> collidesWith) 
    {
        foreach(Entity e in collidesWith)
        {
            if (CastleUtils.Util.Collide(X+dx,Y+dy,Size,Size,e.X,e.Y,e.Size,e.Size))
                return false;
        }
        return true;
    }
}
