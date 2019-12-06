using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chopper : MonoBehaviour
{
    public SceneNode RootNode;
    public Camera NodeCam;
    public float kSightLength = 20f;
    public float CameraDistance = 3f;

    // Use this for initialization
    void Start()
    {
        Debug.Assert(RootNode != null);
        Debug.Assert(NodeCam != null);
    }

    void Update()
    {
        Vector3 pos, dir;
        Matrix4x4 m = Matrix4x4.identity;
        RootNode.CompositeXform(ref m, out pos, out dir);

        Vector3 p1 = pos;
        Vector3 p2 = pos + kSightLength * dir;

        // Now update NodeCam
        NodeCam.transform.localPosition = pos + CameraDistance * dir;
        // NodeCam.transform.LookAt(p2, Vector3.up);
        NodeCam.transform.forward = (p2 - NodeCam.transform.localPosition).normalized;
    }

    public SceneNode GetRootNode() { return RootNode; }

}
