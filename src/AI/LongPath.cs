using CastleUtils;
using Priority_Queue;
using System;
using System.Collections.Generic;

//TODO: Launch two threads, doubling performace.
class LongPath
{
    private static FastPriorityQueue<PathMeshNode> PriorityQueue = new FastPriorityQueue<PathMeshNode>(CastleSpire.BaseWidth * CastleSpire.BaseHeight);
    // It's very admissible.
    private static double MeshHeuristicEmphasis = 0.5;
    // How long should I look for a pixel path to the goal before giving up?
    private static readonly int PixelPathIterations = 300;

    private static double MeshHeuristic(PathFindingMesh.MeshRegion m1, PathFindingMesh.MeshRegion m2)
    {
        return MeshHeuristicEmphasis*Utils.Dist(m1.centerX, m2.centerX, m2.centerY, m2.centerY);
    }

    private class PathMeshNode : FastPriorityQueueNode
    {
        public PathMeshNode Parent = null;
        public PathFindingMesh.MeshRegion Mesh;
        public double Cost; // "g" score
        public double EstimateToGoal; //"heuristic"
        public bool Closed;

        public PathMeshNode(PathFindingMesh.MeshRegion source, PathFindingMesh.MeshRegion goal, double cost)
        {
            Mesh = source;
            Closed = false;
            Cost = cost;
            EstimateToGoal = MeshHeuristic(source,goal);
        }

        public PathMeshNode(PathFindingMesh.MeshRegion source, PathMeshNode parent, PathFindingMesh.MeshRegion goal, double cost) : this(source,goal, cost)
        {
            this.Parent = parent;
        }

        public double Estimate()
        {
            return Cost + EstimateToGoal;
        }
    }


    public static Stack<AllDir> LongPathEstimation(CollisionMap map, int xStart, int yStart, int xEnd, int yEnd, PathFindingMesh mapMesh, int charSize)
    {
        //First thing to do is map the X and Y to regions. If there is no region, we will do a quick BFS to find the closest reachable region.
        //TODO this BFS.
        PathFindingMesh.MeshRegion startRegion = mapMesh.pixelToRegion[xStart, yStart];
        PathFindingMesh.MeshRegion endRegion = mapMesh.pixelToRegion[xEnd, yEnd];

        //BFS yo
        if(endRegion == null)
        {
            endRegion = findClosestRegion(map, mapMesh, xEnd, yEnd);
        }

        if (startRegion == null)
        {
            startRegion = findClosestRegion(map, mapMesh, xStart, yStart);
        }

        // We will use a priority queue to store the open set.
        PriorityQueue.Clear();

        // A dictionary to hold the nodes by ID, allows for fast lookup if it has been explored or not.
        Dictionary<int,PathMeshNode> allNodes = new Dictionary<int, PathMeshNode>();

        // Put the first region in a node
        PathMeshNode startNode = new PathMeshNode(startRegion, endRegion, 0);
        PriorityQueue.Enqueue(startNode,startNode.Estimate());
        
        // Register the node in the dictionary.
        allNodes[startRegion.ID] = startNode;
        
        while (PriorityQueue.Count > 0)
        {
            // Get the best node and close it.
            PathMeshNode toExplore = PriorityQueue.Dequeue();
            toExplore.Closed = true;

            if (toExplore.Mesh.ID == endRegion.ID)
            {
                //We are in the same node as the goal, there must be a problem because the local serch couldn't find a solution when we were so close. 
                //Throw assertion error.
                System.Diagnostics.Debug.Assert(toExplore.Parent != null);

                //Same if we are just one node away.
                System.Diagnostics.Debug.Assert(toExplore.Parent.Parent != null);
          
                //We are aiming for the 2nd node after the start. start.parent = null, one.parent.parent = null, two.parent.parent.parent = one.
                while (toExplore.Parent.Parent.Parent != null)
                {
                    toExplore = toExplore.Parent;
                }

                //return the path to TOExplore, from our node to 2 nodes ahead.
                return PathFinding.PixelPath(map, xStart, yStart, toExplore.Mesh.centerX, toExplore.Mesh.centerY, toExplore.Mesh.pixels, charSize,PixelPathIterations);
            }

            foreach (PathFindingMesh.MeshRegion pm in toExplore.Mesh.edges)
            {

                double cost = toExplore.Cost + Utils.Dist(toExplore.Mesh.centerX, pm.centerX, toExplore.Mesh.centerY, pm.centerY);

                // Hasn't been explored yet so add.
                if (!allNodes.ContainsKey(pm.ID))
                {
                    PathMeshNode child = new PathMeshNode(pm, toExplore, endRegion, cost);
                    PriorityQueue.Enqueue(child,child.Estimate());
                    allNodes[pm.ID] = child;
                }

                // Update otherwise.
                else if (allNodes[pm.ID].Cost > cost && !allNodes[pm.ID].Closed)
                {
                    allNodes[pm.ID].Cost = cost;
                    PriorityQueue.UpdatePriority(allNodes[pm.ID], allNodes[pm.ID].Estimate());
                    allNodes[pm.ID].Parent = toExplore;
                }
            }
        }
        //NO PATH FOUND! This should never happen.
        System.Diagnostics.Debug.Assert(false);
        return null;
    }

    private static PathFindingMesh.MeshRegion findClosestRegion(CollisionMap m, PathFindingMesh pm, int x, int y)
    {
        LinkedList<int[]> frontier = new LinkedList<int[]>();
        PixelSet seen = new PixelSet();
        frontier.AddFirst(new int[2]{x,y});
        seen.Add(frontier.First.Value[0], frontier.First.Value[1]);

        while (frontier.Count > 0)
        {
            int[] pixel = frontier.First.Value;
            frontier.RemoveFirst();

            if (pm.pixelToRegion[pixel[0], pixel[1]] != null)
                return pm.pixelToRegion[pixel[0], pixel[1]];

            foreach (AllDir d in Enum.GetValues(typeof(AllDir)))
            {
                int newX = pixel[0] + DirectionUtils.getDeltaX(d);
                int newY = pixel[1] + DirectionUtils.getDeltaY(d);

                if (!seen.Contains(newX, newY) && !m.Collide(newX, newY)) 
                {
                    frontier.AddLast(new int[2] { newX, newY });
                    seen.Add(newX, newY);
                }
            }
        }
        return null;
    }
}
