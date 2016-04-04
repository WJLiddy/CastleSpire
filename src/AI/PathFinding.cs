using CastleUtils;
using System;
using System.Collections.Generic;

//TODO: Launch two threads, doubling performace.
//TODO: Test InsertionQueue against heap
//Currently good for a search space of ~15 by 15
class PathFinding
{
    private static InsertionQueue<PixelNode> IQ = new InsertionQueue<PixelNode>(CastleSpire.BaseWidth * CastleSpire.BaseHeight);
    private static InsertionQueue<PathMeshNode> IQPRE = new InsertionQueue<PathMeshNode>(CastleSpire.BaseWidth * CastleSpire.BaseHeight);
    //1.0 is admissible.
    private static double PixelHeuristicEmphasis = 1.2;
    private static double MeshHeuristicEmphasis = 0.5;
    public static bool[,] notWalkable; 

    private class PixelNode : IComparable
    {
        public AllDir DirectionFromParent;
        public PixelNode parent = null;
        public int X, Y;
        public double Cost; // "g" score
        public double EstimateToGoal; //"heuristic"
        public double TotalEstimate; // "f" score
        public bool Closed;
     
        public PixelNode(int x, int y, int goalX, int goalY, double cost)
        {
            X = x;
            Y = y;
            Closed = false;
            Cost = cost;
            EstimateToGoal = PixelHeuristic(x, goalX, y, goalY);
            TotalEstimate = Cost + EstimateToGoal;
        }

        public PixelNode(int x, int y, AllDir directionFromParent, int goalX, int goalY, double cost) : this(x, y, goalX, goalY, cost)
        {
            DirectionFromParent = directionFromParent;
        }

        public int CompareTo(object obj)
        {
            if (TotalEstimate - ((PixelNode)obj).TotalEstimate < 0)
                return -1;
            if (TotalEstimate - ((PixelNode)obj).TotalEstimate > 0)
                return 1;
            return 0;
        }
    }

    public static Stack<AllDir> PixelPath(CollisionMap map, int xStart, int yStart, int goalCenterX, int goalCenterY, PixelSet goal, int charSize, int giveUpSteps)
    {
        if (notWalkable == null)
        {
            ///Dirty hardcoding yall
            notWalkable = generateNonWalkableMap(map, charSize);
        }
        // We will use a priority queue to store the open set.
        IQ.Clear();

        // And an array to store all nodes, open or closed. Allows for quick lookup to see if node is open or closed or unexplored.
        PixelNode[,] allNodes = new PixelNode[map.BaseMap.Width,map.BaseMap.Height];

        // Add the initial node to the set
        PixelNode startNode = new PixelNode(xStart,yStart,goalCenterX,goalCenterY,0);
        IQ.Enqueue(startNode);
        allNodes[startNode.X,startNode.Y] = startNode;
        

        while (IQ.Count() > 0)
        {
            giveUpSteps--;
            if (giveUpSteps == 0)
                return null;

            PixelNode toExplore = IQ.Dequeue();
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

    private static double MeshHeuristic(PathFindingMesh.MeshRegion m1, PathFindingMesh.MeshRegion m2)
    {
        return MeshHeuristicEmphasis*Utils.Dist(m1.centerX, m2.centerX, m2.centerY, m2.centerY);
    }

    private class PathMeshNode : IComparable
    {
        public PathMeshNode Parent = null;
        public PathFindingMesh.MeshRegion Mesh;
        public double Cost; // "g" score
        public double EstimateToGoal; //"heuristic"
        public double TotalEstimate; // "f" score
        public bool Closed;

        public PathMeshNode(PathFindingMesh.MeshRegion source, PathFindingMesh.MeshRegion goal, double cost)
        {
            this.Mesh = source;
            Closed = false;
            Cost = cost;
            EstimateToGoal = MeshHeuristic(source,goal);
            TotalEstimate = Cost + EstimateToGoal;
        }

        public PathMeshNode(PathFindingMesh.MeshRegion source, PathMeshNode parent, PathFindingMesh.MeshRegion goal, double cost) : this(source,goal, cost)
        {
            this.Parent = parent;
        }

        public int CompareTo(object obj)
        {
            if (TotalEstimate - ((PathMeshNode)obj).TotalEstimate < 0)
                return -1;
            if (TotalEstimate - ((PathMeshNode)obj).TotalEstimate > 0)
                return 1;
            return 0;
        }
    }


    public static Stack<AllDir> LongPathEstimation(CollisionMap map, int xStart, int yStart, int xEnd, int yEnd, PathFindingMesh mapMesh, int charSize)
    {
        //First thing to do is map the X and Y to regions. If there is no region, we will do a quick BFS to find the closest reachable region.
        PathFindingMesh.MeshRegion startRegion = mapMesh.pixelToRegion[xStart, yStart];
        //First thing to do is map the X and Y to regions. If there is no region, we will do a quick BFS to find the closest reachable region.
        PathFindingMesh.MeshRegion endRegion = mapMesh.pixelToRegion[xEnd, yEnd];

        if(endRegion == null)
        {
            //find closest region
            
        }
        // We will use a priority queue to store the open set.
        IQPRE.Clear();

        // And an array to store all nodes, open or closed. Allows for quick lookup to see if node is open or closed or unexplored.
        Dictionary<int,PathMeshNode> allNodes = new Dictionary<int, PathMeshNode>();

        PathMeshNode startNode = new PathMeshNode(startRegion, endRegion, 0);
        IQPRE.Enqueue(startNode);
        allNodes[startRegion.ID] = startNode;

        int searchedsteps = 0;
        while (IQPRE.Count() > 0)
        {
            searchedsteps++;
            PathMeshNode toExplore = IQPRE.Dequeue();
            toExplore.Closed = true;

            if (toExplore.Mesh.ID == endRegion.ID)
            {
                //we are in the same node as the goal, there must be a problem because a* errored. Throw assrtion.
                System.Diagnostics.Debug.Assert(toExplore.Parent != null);
                //Same if we are just one node away.
                System.Diagnostics.Debug.Assert(toExplore.Parent.Parent != null);
          
                //We are aiming for the 2nd node after the start. start.parent = null, one.parent.parent = null, two.parent.parent.parent = one.
                while (toExplore.Parent.Parent.Parent != null)
                {
                    toExplore = toExplore.Parent;
                }
                //return the path to TOExplore.
                return PathFinding.PixelPath(map, xStart, yStart, toExplore.Mesh.centerX, toExplore.Mesh.centerY, toExplore.Mesh.pixels, charSize,300);
            }

            foreach (PathFindingMesh.MeshRegion pm in toExplore.Mesh.edges)
            {

                double cost = toExplore.Cost + Utils.Dist(toExplore.Mesh.centerX, pm.centerX, toExplore.Mesh.centerY, pm.centerY);


                // Hasn't been explored yet so add.
                if (!allNodes.ContainsKey(pm.ID))
                {
                    PathMeshNode child = new PathMeshNode(pm, toExplore, endRegion, cost);
                    IQPRE.Enqueue(child);
                    allNodes[pm.ID] = child;
                }


                else if (allNodes[pm.ID].Cost > cost && !allNodes[pm.ID].Closed)
                {
                    allNodes[pm.ID].Cost = cost;
                    allNodes[pm.ID].TotalEstimate = (cost + allNodes[pm.ID].EstimateToGoal);
                    allNodes[pm.ID].Parent = toExplore;
                }
            }
        } // end openset loop

        //NO PATH FOUND! TODO: Graceful Exit
        return null;
    }

    public static  bool[,] generateNonWalkableMap(CollisionMap m, int charSize)
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
