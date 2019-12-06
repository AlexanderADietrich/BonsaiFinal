using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePrimitive: MonoBehaviour {
    public Color MyColor = new Color(0.1f, 0.1f, 0.2f, 1.0f);
    public Vector3 Pivot;
    public Vector3 currentLoc;
    public NodePrimitive bladePrimitive;
    public Vector3 cachedBladePos;
    public GameObject hitbox;

    public bool genHitbox = false;
    public SceneNode manipulateIfHit = null;
    


    private void Start()
    {
        bladePrimitive = GameObject.Find("BladePrimitive").GetComponent<NodePrimitive>();
    }

    public void LoadShaderMatrix(ref Matrix4x4 nodeMatrix)
    {
        Matrix4x4 p = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 invP = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        Matrix4x4 m = nodeMatrix * p * trs * invP;
        currentLoc = m.GetColumn(3);
        if (genHitbox)
        {
            if (hitbox == null)
            {
                hitbox = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                hitbox.name = "Ref";
                hitbox.GetComponent<Renderer>().enabled = false;
            }
            
            hitbox.transform.position = currentLoc;
            hitbox.transform.localRotation = transform.rotation;
            hitbox.transform.localScale = transform.lossyScale;
            hitbox.AddComponent<ReferenceToPrimitive>().np = this;
        }
        

        GetComponent<Renderer>().material.SetMatrix("MyTRSMatrix", m);
        GetComponent<Renderer>().material.SetColor("MyColor", MyColor);
        if (bladePrimitive.currentLoc != cachedBladePos)
        {
            GetComponent<Renderer>().material.SetVector("lightPos", new Vector4(bladePrimitive.currentLoc.x, bladePrimitive.currentLoc.y, bladePrimitive.currentLoc.z, 0.5f));
            cachedBladePos = bladePrimitive.currentLoc + Vector3.zero;
        }
        
        GetComponent<Renderer>().material.SetColor("lightColor", Color.white);
        GetComponent<Renderer>().material.SetFloat("random", Random.Range(0, 0.5f));
    }

    private float getMag(Vector4 v4)
    {
        return Mathf.Sqrt(Mathf.Pow(v4[0], 2) + Mathf.Pow(v4[1], 2) + Mathf.Pow(v4[2], 2));
    }

    public Vector3[] getColSegmentCylinder(Quaternion rot, Vector3 scale)
    {
        //Debug.Log("HERE " + scale.y);
        //Debug.Log("\t" + rot.eulerAngles.normalized);
        Vector3 top;
        Vector3 bot;
        if (rot != Quaternion.identity)
        {
            top = currentLoc + rot.eulerAngles.normalized * scale.y;
            bot = currentLoc - rot.eulerAngles.normalized * scale.y;
        }
        else
        {
            top = currentLoc + new Vector3(0, 1, 0) * scale.y;
            bot = currentLoc - new Vector3(0, 1, 0) * scale.y;
        }
        return new Vector3[] { bot, top };
    }
    public Vector3[] getColSegmentCube(Quaternion rot, Vector3 scale)
    {
        //Debug.Log("HERE " + scale.y);
        //Debug.Log("\t" + rot.eulerAngles.normalized);
        Vector3 top;
        Vector3 bot;
        if (rot != Quaternion.identity)
        {
            top = currentLoc + rot.eulerAngles.normalized * scale.y;
            bot = currentLoc - rot.eulerAngles.normalized * scale.y;
        }
        else
        {
            top = currentLoc + new Vector3(0, 1, 0) * scale.y;
            bot = currentLoc - new Vector3(0, 1, 0) * scale.y;
        }
        return new Vector3[] { bot, top };
    }
}