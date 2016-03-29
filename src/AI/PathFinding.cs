using CastleUtils;
using System;
using System.Collections.Generic;

class PathFinding
{
    private class DijikstraNode
    {
        public AllDir DirectionFromParent;
        public DijikstraNode parent = null;
        public int X, Y;
        public double Cost;
        public DijikstraNode(int x, int y)
        {
            X = x;
            Y = y;
        }
        public DijikstraNode(int x, int y, AllDir directionFromParent)
        {
            X = x;
            Y = y;
            DirectionFromParent = directionFromParent;
        }
    }



    public static Stack<AllDir> DijikstraPath(CollisionMap map, int xStart, int yStart, int xEnd, int yEnd)
    {
            // We will use a priority queue to store the open set.
            Queue<DijikstraNode> openSet = new Queue<DijikstraNode>();

            // And an array to store all nodes, open or closed. Allows for quick lookup to see if node is open or closed.
            DijikstraNode[,] allNodes = new DijikstraNode[map.BaseMap.Width,map.BaseMap.Height];

            // Add the initial node to the set
            DijikstraNode startNode = new DijikstraNode(xStart,yStart);
            openSet.Enqueue(startNode);
            allNodes[startNode.X,startNode.Y] = startNode;
        

            // Search 
            while (openSet.Count > 0)
            {
                DijikstraNode toExplore = openSet.Dequeue();

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

                    double cost = toExplore.Cost + ((d == AllDir.North || d == AllDir.West || d == AllDir.South || d == AllDir.East) ? 1 : Util.Rad2Over2);

                    // Hasn't been explored yet so add.
                    if (allNodes[childX,childY] == null)
                    {
                        DijikstraNode child = new DijikstraNode(childX,childY,d);
                        openSet.Enqueue(child);
                        allNodes[childX,childY] = child;
                        child.parent = toExplore;
                    }
                    // If the node has been explored and is closed
                    // High cost = more attractive
                    else if (allNodes[childX,childY].Cost > cost && !openSet.Contains(allNodes[childX, childY]))
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
