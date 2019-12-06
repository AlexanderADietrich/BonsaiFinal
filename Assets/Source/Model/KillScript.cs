using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillScript : MonoBehaviour
{
    public int killAfter = 1;
    // Update is called once per frame
    void Update()
    {
        if (killAfter <= 0)
            Destroy(gameObject);
        killAfter--;
    }
}
