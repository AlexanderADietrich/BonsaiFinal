using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBranch : MonoBehaviour
{
    public float duration = 2.0f;
    public float fallSpeed = 1.0f;

    private float elapsedTime;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > duration)
        {
            recursiveDestroy(transform);
        }

        transform.localPosition -= new Vector3(0, Time.deltaTime * fallSpeed, 0);
    }

    void recursiveDestroy(Transform g) 
    {
        foreach (Transform child in g)
        {
            recursiveDestroy(child);
        }
        Destroy(g.gameObject);
    }
}
