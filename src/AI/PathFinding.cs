using CastleUtils;
using System;
using System.Collections.Generic;

class PathFinding
{
    private static InsertionQueue<DijikstraNode> IQ = new InsertionQueue<DijikstraNode>(CastleSpire.BaseWidth * CastleSpire.BaseHeight);

    // NOT THREAD SAFE
    private class DijikstraNode : IComparable
    {
        public AllDir DirectionFromParent;
        public DijikstraNode parent = null;
        public int X, Y;
        public double Cost;
        public bool Closed;
     
        public DijikstraNode(int x, int y)
        {
            X = x;
            Y = y;
            Closed = false;
        }
        public DijikstraNode(int x, int y, AllDir directionFromParent)
        {
            X = x;
            Y = y;
            DirectionFromParent = directionFromParent;
            Closed = false;
        }

        public int CompareTo(object obj)
        {
            if (Cost - ((DijikstraNode)obj).Cost < 0)
                return -1;
            if (Cost - ((DijikstraNode)obj).Cost > 0)
                return 1;
            return 0;
        }
    }



    public static Stack<AllDir> DijikstraPath(CollisionMap map, int xStart, int yStart, int xEnd, int yEnd)
    {
        // We will use a priority queue to store the open set.
        IQ.Clear();

        // And an array to store all nodes, open or closed. Allows for quick lookup to see if node is open or closed.
        DijikstraNode[,] allNodes = new DijikstraNode[map.BaseMap.Width,map.BaseMap.Height];

        // Add the initial node to the set
        DijikstraNode startNode = new DijikstraNode(xStart,yStart);
        IQ.Enqueue(startNode);
        allNodes[startNode.X,startNode.Y] = startNode;
       
        while (IQ.Count() > 0)
        {
            DijikstraNode toExplore = IQ.Dequeue();
            toExplore.Closed = true;
            
            if (toExplore.X == xEnd && toExplore.Y == yEnd)
                return pathToThisNode(toExplore);

            foreach(AllDir d in Enum.GetValues(typeof(AllDir)))
            {
                    
                int childX = toExplore.X + DirectionUtils.getDeltaX(d);
                int childY = toExplore.Y + DirectionUtils.getDeltaY(d);

                //don't go out of bounds.
                if (childY < 0 || childX < 0 || childX >= map.BaseMap.Width || childY >= map.BaseMap.Height)
                    continue;

                if (map.Collide(childX,childY))
                    continue;

                double cost = toExplore.Cost + ((d == AllDir.North || d == AllDir.West || d == AllDir.South || d == AllDir.East) ? 1 : Util.Rad2);

                // Hasn't been explored yet so add.
                if (allNodes[childX,childY] == null)
                {
                    DijikstraNode child = new DijikstraNode(childX,childY,d);
                    IQ.Enqueue(child);
                    allNodes[childX,childY] = child;
                    child.parent = toExplore;
                    child.Cost = cost;
                }
                // If the node has been explored and is closed
                // High cost = more attractive
                //add in if fast
                else if (allNodes[childX,childY].Cost > cost && allNodes[childX, childY].Closed)
                {
                    allNodes[childX, childY].Cost = cost;
                    allNodes[childX,childY].DirectionFromParent = d;
                    allNodes[childX,childY].parent = toExplore;
                }
            }
        } // end openset loop

        //NO PATH FOUND! TODO: Graceful Exit
        return null;
    }

    private static Stack<AllDir> pathToThisNode(DijikstraNode d)
    {
        Stack<AllDir> path = new Stack<AllDir>();
        while (d.parent != null)
        {
            path.Push(d.DirectionFromParent);
            d = d.parent;
        }
        return path;
    }
}
