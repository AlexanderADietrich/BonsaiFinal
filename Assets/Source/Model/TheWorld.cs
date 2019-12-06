using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TheWorld : MonoBehaviour {

    public SceneNode RootNode;

    	// Use this for initialization
	void Start () {
        Debug.Assert(RootNode != null);
    }

    void Update()
    {
        Vector3 pos, dir;
        Matrix4x4 m = Matrix4x4.identity;
        if (RootNode != null)
            RootNode.CompositeXform(ref m, out pos, out dir);
    }

    public SceneNode GetRootNode() { return RootNode; }
    
}
