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
        LinkedList<MeshRegion> edges;
    } 

    public MeshRegion[,] pixelToRegion;

    public PathFindingMesh(string pathToXML, int baseMapWidth, int baseMapHeight)
    {
        pixelToRegion = new MeshRegion[baseMapWidth, baseMapHeight];
        Dictionary<string, LinkedList<string>> meshXML = Utils.GetXMLEntriesHash(pathToXML);
        
        int regionCount = int.Parse(meshXML["MeshRegionCount"].First.Value);
        for (int i = 0; i != regionCount; i++)
        {
            MeshRegion a = new MeshRegion();
            a.ID = i;
            //ignore edges FO NOW

            foreach (string s in meshXML["Region" + a.ID + "Pixel"])
            {
                string[] coord = s.Split(',');
                pixelToRegion[int.Parse(coord[0]), int.Parse(coord[1])] = a;
            }
        }
    }
}