using CastleUtils;
using System;
using System.Collections.Generic;

//TODO: Launch two threads, doubling performace.
//TODO: Test InsertionQueue against heap
//Currently good for a search space of ~15 by 15
class PathFinding
{
    private static InsertionQueue<DijikstraNode> IQ = new InsertionQueue<DijikstraNode>(CastleSpire.BaseWidth * CastleSpire.BaseHeight);
    //1.0 is admissible.
    private static double HeuristicEmphasis = 1.2;
    // NOT THREAD SAFE
    private class DijikstraNode : IComparable
    {
        public AllDir DirectionFromParent;
        public DijikstraNode parent = null;
        public int X, Y;
        public double Cost; // "g" score
        public double EstimateToGoal; //"heuristic"
        public double TotalEstimate; // "f" score
        public bool Closed;
     
        public DijikstraNode(int x, int y, int goalX, int goalY, double cost)
        {
            X = x;
            Y = y;
            Closed = false;
            Cost = cost;
            EstimateToGoal = Heuristic(x, goalX, y, goalY);
            TotalEstimate = Cost + EstimateToGoal;
        }
        public DijikstraNode(int x, int y, AllDir directionFromParent, int goalX, int goalY, double cost) : this(x, y, goalX, goalY, cost)
        {
            DirectionFromParent = directionFromParent;
        }

        public int CompareTo(object obj)
        {
            if (TotalEstimate - ((DijikstraNode)obj).TotalEstimate < 0)
                return -1;
            if (TotalEstimate - ((DijikstraNode)obj).TotalEstimate > 0)
                return 1;
            return 0;
        }
    }



    public static Stack<AllDir> DijikstraPath(CollisionMap map, int xStart, int yStart, int xEnd, int yEnd)
    {
        // We will use a priority queue to store the open set.
        IQ.Clear();

        // And an array to store all nodes, open or closed. Allows for quick lookup to see if node is open or closed or unexplored.
        DijikstraNode[,] allNodes = new DijikstraNode[map.BaseMap.Width,map.BaseMap.Height];

        // Add the initial node to the set
        DijikstraNode startNode = new DijikstraNode(xStart,yStart,xEnd,yEnd,0);
        IQ.Enqueue(startNode);
        allNodes[startNode.X,startNode.Y] = startNode;

        int searchedsteps= 0;
        while (IQ.Count() > 0)
        {
            searchedsteps++;
            DijikstraNode toExplore = IQ.Dequeue();
            toExplore.Closed = true;

            if (toExplore.X == xEnd && toExplore.Y == yEnd)
            {

                Utils.Log("" + searchedsteps);
                return pathToThisNode(toExplore);
            }

            foreach(AllDir d in Enum.GetValues(typeof(AllDir)))
            {
                    
                int childX = toExplore.X + DirectionUtils.getDeltaX(d);
                int childY = toExplore.Y + DirectionUtils.getDeltaY(d);

                //don't go out of bounds.
                if (childY < 0 || childX < 0 || childX >= map.BaseMap.Width || childY >= map.BaseMap.Height)
                    continue;

                if (map.Collide(childX,childY))

                    continue;

                double cost = toExplore.Cost + ((d.Equals(AllDir.North) || d.Equals(AllDir.West) || d.Equals(AllDir.South) || d.Equals(AllDir.East)) ? 1 : Util.Rad2);

                // Hasn't been explored yet so add.
                if (allNodes[childX,childY] == null)
                {
                    DijikstraNode child = new DijikstraNode(childX,childY,d,xEnd,yEnd,cost);
                    IQ.Enqueue(child);
                    allNodes[childX,childY] = child;
                    child.parent = toExplore;
                }


                else if (allNodes[childX,childY].Cost > cost && !allNodes[childX, childY].Closed)
                {
                    allNodes[childX, childY].Cost = cost;
                    allNodes[childX, childY].TotalEstimate = (cost + allNodes[childX, childY].EstimateToGoal);
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

    //It is true that a heuristic has to be admissible to be optimal, but this is slow in the long run. 
    //Consider a long, long line that ends in a wall and has to go around the wall. Becuase the heuristic is admissible, it will prefer far-away nodes with better scores,
    //When actually all that is needed is to naviagate left and right
    //So we choose to make a heurstic that is not admissible, making it slightly suboptimal, but faster.
    private static double Heuristic(int x1, int x2, int y1, int y2)
    {
        return HeuristicEmphasis*Utils.Dist(x1, x2, y1, y2);
    }
}
