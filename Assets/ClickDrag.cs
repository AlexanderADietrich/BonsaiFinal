using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ClickDrag : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject mSelectedObject;
    float prevX;
    float prevY;
    bool dragging = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftAlt))
        {
            Debug.Log("Mouse clicked");
            prevX = Input.mousePosition.x;
            prevY = Input.mousePosition.y;

            // Ensure not clicking GUI
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Raycasting...");
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, 1);
                

                if (hit)
                {
                    Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                    if (hitInfo.transform.name.Contains("Ref"))
                    {
                        Debug.Log("Selected " + hitInfo.transform.name);
                        mSelectedObject = hitInfo.transform.gameObject;
                        dragging = true;
                    }
                }
            }
            else
            {
                Debug.Log("Did not raycast: over GUI");
            }
        }
        //user is dragging as long as getMouseButton
        else if (dragging && (Input.GetMouseButton(0)))
        {
            float dx = prevX - Input.mousePosition.x;
            float dy = prevY - Input.mousePosition.y;
            prevX = Input.mousePosition.x;
            prevY = Input.mousePosition.y;

            Debug.Log("Dragged by {" + dx + ", " + dy + "}");

            if (mSelectedObject != null)
            {
                NodePrimitive np = mSelectedObject.GetComponent<ReferenceToPrimitive>().np;
                Vector3 q = np.manipulateIfHit.transform.localRotation.eulerAngles;
                if (np.name == "BasePrimitive")
                    q.y += dx;
                else
                    q.z += dx;
                mSelectedObject.GetComponent<ReferenceToPrimitive>().np.manipulateIfHit.transform.localRotation = Quaternion.Euler(q.x, q.y, q.z);
            }
        }
        else
        {
            if (dragging)
                Debug.Log("Stopped dragging");
            dragging = false;
        }
    }
}
