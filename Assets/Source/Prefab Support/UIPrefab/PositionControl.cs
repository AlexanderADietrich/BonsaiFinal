using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionControl : MonoBehaviour {

    public SliderWithEcho AtX = null;
    public SliderWithEcho AtY = null;
    public SliderWithEcho AtZ = null;
    public Transform ThePosition = null;  // to be controlled
    public Text Label;

    // Use this for initialization
    void Start () {
        Debug.Assert(AtX != null);
        Debug.Assert(AtY != null);
        Debug.Assert(AtZ != null);
        Debug.Assert(Label != null);

        AtX.SetSliderLabel("X");
        AtY.SetSliderLabel("Y");
        AtZ.SetSliderLabel("Z");
        AtX.InitSliderRange(-30, 30, 0);
        AtY.InitSliderRange(-30, 30, 0);
        AtZ.InitSliderRange(-30, 30, 0);

        AtX.SetSliderListener(AtXChange);
        AtY.SetSliderListener(AtYChange);
        AtZ.SetSliderListener(AtZChange);

    }
	
	// Update is called once per frame
	public void SetControlPosition(Transform t) {
        ThePosition = t;
        ObjectSetUI();
	}

    void ObjectSetUI()
    {
        AtX.SetSliderValue(ThePosition.localPosition.x);
        AtY.SetSliderValue(ThePosition.localPosition.y);
        AtZ.SetSliderValue(ThePosition.localPosition.z);
    }

    void UISetObject(Vector3 p)
    {
        ThePosition.localPosition = p;
    }

    void AtXChange(float newValue)
    {
        Vector3 p = ThePosition.localPosition;
        p.x = newValue;
        UISetObject(p);
    }

    void AtYChange(float newValue)
    {
        Vector3 p = ThePosition.localPosition;
        p.y = newValue;
        UISetObject(p);
    }

    void AtZChange(float newValue)
    {
        Vector3 p = ThePosition.localPosition;
        p.z = newValue;
        UISetObject(p);
    }
}
