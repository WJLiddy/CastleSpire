﻿using System.Collections.Generic;

namespace CastleUtils
{ 
    public class Util
    {
        public static readonly double Rad2 = 1.414214;
        public static bool Collide(int x1, int y1, int w1, int h1, int x2, int y2, int w2, int h2)
        {
            //Check for any collision.
            return !(x1 > x2 + w2 ||
                    y1 > y2 + h2 ||
                    x2 > x1 + w1 ||
                    y2 > y1 + h1);
        }
    }

    public class PixelSet
    {
        private Dictionary<int, HashSet<int>> regionPixels = new Dictionary<int, HashSet<int>>();

        public void Add(int x, int y)
        {
            if (!regionPixels.ContainsKey(x))
                regionPixels.Add(x,new HashSet<int>());
            regionPixels[x].Add(y);
        }

        public bool Contains(int x, int y)
        {
            if (!regionPixels.ContainsKey(x))
                return false;
            return regionPixels[x].Contains(y);
        }
    }

    public enum CardinalDir { North, East, South, West };
    public enum AllDir { North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest};

    public class DirectionUtils
    {
        public static int getDeltaX(AllDir d)
        {
            if (d.Equals(AllDir.NorthEast) || d.Equals(AllDir.East) || d.Equals(AllDir.SouthEast))
                return 1;
            if (d.Equals(AllDir.SouthWest) || d.Equals(AllDir.West) || d.Equals(AllDir.NorthWest))
                return -1;
            return 0;
        }

        public static int getDeltaY(AllDir d)
        {
            if (d.Equals(AllDir.North) || d.Equals(AllDir.NorthEast) || d.Equals(AllDir.NorthWest))
                return -1;
            if (d.Equals(AllDir.South) || d.Equals(AllDir.SouthWest) || d.Equals(AllDir.SouthEast))
                return 1;
            return 0;
        }

        public static CardinalDir ConvertAllDir(AllDir d)
        {
            if (d.Equals(AllDir.North))
                return CardinalDir.North;
            if (d.Equals(AllDir.South)) 
                return CardinalDir.South;
            if (d.Equals(AllDir.East) || d.Equals(AllDir.SouthEast) || d.Equals(AllDir.NorthEast))
                return CardinalDir.East;
            return CardinalDir.West;
        }
    }


}
