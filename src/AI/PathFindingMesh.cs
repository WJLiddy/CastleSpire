using System.Collections.Generic;

class PathFindingMesh
{
    //ok
    //the process is to figure out what poly you are in
    // Long A* to the goal poly
    // then local A* to the next poly.

        //!!NEED a mesh-region counter;
    public class MeshRegion
    {
        // not needed but helps for debug
        public int ID;
        public LinkedList<MeshRegion> edges = new LinkedList<MeshRegion>();
        public int centerX;
        public int centerY;
        public CastleUtils.PixelSet pixels;
    } 

    public MeshRegion[,] pixelToRegion;
    public LinkedList<MeshRegion> allRegions = new LinkedList<MeshRegion>();
    public Dictionary<int, MeshRegion> IDToRegion = new Dictionary<int, MeshRegion>();

    public PathFindingMesh(string pathToXML, int baseMapWidth, int baseMapHeight)
    {
        pixelToRegion = new MeshRegion[baseMapWidth, baseMapHeight];
        Dictionary<string, LinkedList<string>> meshXML = Utils.GetXMLEntriesHash(pathToXML);
 
        int regionCount = int.Parse(meshXML["MeshRegionCount"].First.Value);
        for (int i = 0; i != regionCount; i++)
        {
            MeshRegion a = new MeshRegion();
            a.pixels = new CastleUtils.PixelSet();
            IDToRegion[i] = a;
            allRegions.AddFirst(a);
            a.ID = i;

            int pixelsInRegion = 0;
            foreach (string s in meshXML["Region" + a.ID + "Pixel"])
            {
                string[] coord = s.Split(',');
                pixelToRegion[int.Parse(coord[0]), int.Parse(coord[1])] = a;
                a.centerX += int.Parse(coord[0]);
                a.centerY += int.Parse(coord[1]);
                pixelsInRegion++;
                a.pixels.Add(int.Parse(coord[0]), int.Parse(coord[1]));
            }

            a.centerX = a.centerX / pixelsInRegion;
            a.centerY = a.centerY / pixelsInRegion;
            
        }

        // now, iterate again and get edges.
        for (int i = 0; i != regionCount; i++)
        {
            foreach (string s in meshXML["Region" + i + "Edge"])
            {
                IDToRegion[i].edges.AddFirst(IDToRegion[int.Parse(s)]);
            }
        }

    }
}