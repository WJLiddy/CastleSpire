namespace CastleUtils
{ 
    public class Util
    {
        public static readonly double Rad2 = 1.414214;
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
    }
}
