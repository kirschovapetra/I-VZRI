using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class RotateModel : MonoBehaviour
{
 
    public Slider slider;
    public GameObject model;
 
    public void SliderRotatingModel()
    {
        model.transform.rotation = Quaternion.Euler(model.transform.rotation.x, slider.value, model.transform.rotation.z);
    }
}
 