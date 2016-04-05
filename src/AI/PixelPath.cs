using CastleUtils;
using Priority_Queue;
using System;
using System.Collections.Generic;

//TODO: Launch two threads, doubling performace.
class PathFinding
{
    private static FastPriorityQueue<PixelNode> PriorityQueue = new FastPriorityQueue<PixelNode>(CastleSpire.BaseWidth * CastleSpire.BaseHeight);
    //1.0 is admissible. 1.2 is not admissible, resulting in suboptimal paths, but it is faster.
    private static double PixelHeuristicEmphasis = 1.2;
    public static bool[,] notWalkable; 

    private class PixelNode : FastPriorityQueueNode
    {
        public AllDir DirectionFromParent;
        public PixelNode parent = null;
        public int X, Y;
        public double Cost; // "g" score
        public double EstimateToGoal; //"heuristic"
        public bool Closed;
     
        public PixelNode(int x, int y, int goalX, int goalY, double cost)
        {
            X = x;
            Y = y;
            Closed = false;
            Cost = cost;
            EstimateToGoal = PixelHeuristic(x, goalX, y, goalY);
        }

        public PixelNode(int x, int y, AllDir directionFromParent, int goalX, int goalY, double cost) : this(x, y, goalX, goalY, cost)
        {
            DirectionFromParent = directionFromParent;
        }

        public double getEstimate()
        {
            return Cost + EstimateToGoal; 
        }
    }

    public static Stack<AllDir> PixelPath(CollisionMap map, int xStart, int yStart, int goalCenterX, int goalCenterY, PixelSet goal, int charSize, int giveUpSteps)
    {
        // Generate a nowalkmap if one has not yet been generated.
        if (notWalkable == null)
        {
            notWalkable = generateNonWalkableMap(map, charSize);
        }

        // We will use a priority queue to store the open set.
        PriorityQueue.Clear();

        // And an array to store all nodes, open or closed. Allows for quick lookup to see if node is open or closed or unexplored.
        PixelNode[,] allNodes = new PixelNode[map.BaseMap.Width,map.BaseMap.Height];

        // Add the initial node to the set
        PixelNode startNode = new PixelNode(xStart,yStart,goalCenterX,goalCenterY,0);
        PriorityQueue.Enqueue(startNode,startNode.getEstimate());
        allNodes[startNode.X,startNode.Y] = startNode;

        while (PriorityQueue.Count > 0)
        {
            giveUpSteps--;
            if (giveUpSteps == 0)
                return null;

            PixelNode toExplore = PriorityQueue.Dequeue();
            toExplore.Closed = true;

            if (goal.Contains(toExplore.X,toExplore.Y))
            {
                return pathToThisPixelNode(toExplore);
            }

            foreach(AllDir d in Enum.GetValues(typeof(AllDir)))
            {
                int childX = toExplore.X + DirectionUtils.getDeltaX(d);
                int childY = toExplore.Y + DirectionUtils.getDeltaY(d);

                //don't go out of bounds.
                if (childY < 0 || childX < 0 || childX >= map.BaseMap.Width || childY >= map.BaseMap.Height)
                    continue;

                if (notWalkable[childX,childY])
                    continue;

                double cost = toExplore.Cost + ((d.Equals(AllDir.North) || d.Equals(AllDir.West) || d.Equals(AllDir.South) || d.Equals(AllDir.East)) ? 1 : Util.Rad2);

                // Hasn't been explored yet so add.
                if (allNodes[childX,childY] == null)
                {
                    PixelNode child = new PixelNode(childX,childY,d,goalCenterX,goalCenterY,cost);
                    PriorityQueue.Enqueue(child,child.getEstimate());
                    allNodes[childX,childY] = child;
                    child.parent = toExplore;
                }


                else if (allNodes[childX,childY].Cost > cost && !allNodes[childX, childY].Closed)
                {
                    allNodes[childX, childY].Cost = cost;
                    PriorityQueue.UpdatePriority(allNodes[childX, childY], allNodes[childX, childY].getEstimate());
                    allNodes[childX,childY].DirectionFromParent = d;
                    allNodes[childX,childY].parent = toExplore;
                }
            }
        }
        return null;
    }

    private static Stack<AllDir> pathToThisPixelNode(PixelNode d)
    {
        Stack<AllDir> path = new Stack<AllDir>();
        while (d.parent != null)
        {
            path.Push(d.DirectionFromParent);
            d = d.parent;
        }

        return path;
    }

    //It is true that a heuristic has to be admissible to be optimal, but this is slow in the long run. 
    //Consider a long, long line that ends in a wall and has to go around the wall. Becuase the heuristic is admissible, it will prefer far-away nodes with better scores,
    //When actually all that is needed is to naviagate left and right
    //So we choose to make a heurstic that is not admissible, making it slightly suboptimal, but faster.
    private static double PixelHeuristic(int x1, int x2, int y1, int y2)
    {
        return PixelHeuristicEmphasis*Utils.Dist(x1, x2, y1, y2);
    }

   // Given a map and a charsize, my top left pixels CANNOT be at return[x][y], otherwise they can.
   // TODO: This might have an off-by-1
    public static bool[,] generateNonWalkableMap(CollisionMap m, int charSize)
    {
        bool[,] notWalkable = new bool[m.BaseMap.Width, m.BaseMap.Height];
        for (int x = 0; x != m.BaseMap.Width; x++)
        {
            for (int y = 0; y != m.BaseMap.Height; y++)
            {
                if (m.Collide(x, y))
                {
                    for (int height = charSize; height >= 0; height--)
                    {
                        for (int width = charSize; width >= 0; width--)
                        {
                            if (y - height < 0 || x - width < 0)
                                continue;
                            notWalkable[x - width, y - height] = true;
                        }
                    }
                }
            }
        }

        //Cannot walk along the far sides of map either.
        for (int x = 0; x != m.BaseMap.Width; x++)
        {
            for (int y = 0; y != m.BaseMap.Height; y++)
            {
                if (x > m.BaseMap.Width - charSize || y > m.BaseMap.Height - charSize)
                {
                    notWalkable[x, y] = true;
                }
            }
        }
        return notWalkable;
    }
}
