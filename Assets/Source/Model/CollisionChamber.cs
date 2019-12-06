using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChamber : MonoBehaviour
{
    public List<GrowingSceneNode> plantParts;
    public SceneNode nBlade;
    public NodePrimitive blade;
    private GameObject cylinderCheck;
    private GameObject thePlantWold;
    // Start is called before the first frame update
    void Start()
    {
        cylinderCheck = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cylinderCheck.GetComponent<Renderer>().enabled = false;
        thePlantWold = GameObject.Find("ThePlantWorld");
        Debug.Assert(thePlantWold != null);
    }

    private void CutSegment(GrowingSceneNode targetNode)
    {
        // Create a new TheWorld so that the cut segment can remain visible
        GameObject tempParent = new GameObject("tempParent");
        tempParent.AddComponent<TheWorld>();
        tempParent.AddComponent<KillScript>().killAfter = 32;
        tempParent.GetComponent<TheWorld>().RootNode = targetNode.GetComponent<SceneNode>();
        Debug.Log("Cut");


        NodePrimitive primitive = targetNode.transform.GetChild(0).GetChild(0).GetComponent<NodePrimitive>();
        Vector3 lastAttachedPosition;
        lastAttachedPosition = primitive.currentLoc;
        tempParent.transform.localScale = new Vector3(1337, 1337, 1337);
        targetNode.NodeOrigin = lastAttachedPosition;
        targetNode.transform.parent = tempParent.transform;
        plantParts.Remove(targetNode);
        targetNode.gameObject.AddComponent<FallingBranch>();
    }
    private List<GrowingSceneNode> removeThese = new List<GrowingSceneNode>();

    // Update is called once per frame
    void Update()
    {
        Vector3[] tempB = blade.getColSegmentCylinder(nBlade.transform.rotation, nBlade.transform.localScale);
        tempB = getSegment(tempB[0], tempB[1], blade.transform.rotation);
        //DrawHitbox(tempB[0], tempB[1], nBlade.transform.rotation);
        removeThese.Clear();
        foreach (GrowingSceneNode gsn in plantParts)
        {
            if (gsn != null)
            {
                Vector3[] tempG = gsn.getCollisionSegment();
                tempG = getSegment(tempG[0], tempG[1], gsn.transform.localRotation);
                //DrawHitbox(tempG[0], tempG[1], gsn.transform.rotation);
                if (LazyCollideLineSegments(tempB[0], tempB[1], tempG[0], tempG[1]))
                {
                    removeThese.Add(gsn);
                }
            }
        }
        foreach (GrowingSceneNode gsn in removeThese)
        {
            CutSegment(gsn);
        }
    }

    public static int RES = 100;
    public static float THRESH = 1.0f;
    //this is not a great solution but line segment collision is very difficult
    bool LazyCollideLineSegments(Vector3 a1, Vector3 a2, Vector3 b1, Vector3 b2)
    {
        //drawClosest(a1, Color.red);
       // drawClosest(a2, Color.red);
        //drawClosest(b1, Color.red);
        //drawClosest(b2, Color.red);

        //vectors
        Vector3 av = (a2 - a1);
        Vector3 bv = (b2 - b1);
        //define vectors along segments
        Vector3 an = (av).normalized;
        Vector3 bn = (bv).normalized;
        //define vector from one endpoint to another
        Vector3 ab = (b1 - a1);
        //project -ab onto bv
        float bProjDist = Vector3.Dot(bn, -ab);
        if (bProjDist < 0 || bProjDist > bv.magnitude)
            return false;
        float aProjDist = Vector3.Dot(an, ab);
        if (aProjDist < 0 || aProjDist > av.magnitude)
            return false;

        Vector3 bpoint = b1 + bn * bProjDist;
        Vector3 apoint = a1 + an * aProjDist;
        //drawClosest(bpoint, Color.red);
        //drawClosest(apoint, Color.blue); 
        float dist = (bpoint - apoint).magnitude;
        Debug.Log(dist);
        return (dist < THRESH);
        
        //define axis
        //Vector3 yn = Vector3.Cross(an, bn).normalized;


        /*//find vector from point on a to point on B that is nearly parallel to yn
        Vector3 pointOnB = b1 + bv / 2;
        double min = 999999;
        float minDist = 999999;
        for (int b = 0;)
        for (int i = 0; i < RES; i++)
        {
            Vector3 pointOnA = a1 + av / RES;
            Vector3 pointToB = pointOnA - bmid;
            float dot = Vector3.Dot(pointToB, yn);
            //dot closest to 1 is dot closest
            if (Mathf.Abs(dot - 1) < min)
            {
                min = Mathf.Abs(dot - 1);
                minDist = pointToB.magnitude;
            }
        }
        Debug.Log("\t" + minDist);*/
        //return (minDist < THRESH);
    }

    //for some reason only cylinders interact well to generate the points.
    Vector3[] getSegment(Vector3 from, Vector3 to, Quaternion rotation)
    {
        cylinderCheck.transform.position = (from + to) / 2;
        cylinderCheck.transform.rotation = rotation;
        cylinderCheck.transform.localScale = new Vector3(0.1f, (to - from).magnitude / 2, 0.1f);
        return new Vector3[] {cylinderCheck.transform.position + cylinderCheck.transform.up * cylinderCheck.transform.localScale.y,
            cylinderCheck.transform.position - cylinderCheck.transform.up * cylinderCheck.transform.localScale.y};
    }

    void DrawHitbox(Vector3 from, Vector3 to, Quaternion rotation)
    {
        GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        temp.transform.position = (from + to) / 2;
        temp.transform.rotation = rotation;
        //Debug.Log(to == from);
        temp.transform.localScale = new Vector3(0.1f, (to - from).magnitude / 2, 0.1f);
        temp.AddComponent<KillScript>();
    }

    void drawClosest(Vector3 point)
    {
        drawClosest(point, Color.white);
    }

    void drawClosest(Vector3 point, Color c)
    {
        GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        temp.transform.position = point;
        temp.transform.GetComponent<Renderer>().material.color = c;
        temp.AddComponent<KillScript>();
    }
}
