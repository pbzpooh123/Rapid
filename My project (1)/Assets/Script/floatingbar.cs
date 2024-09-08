using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class floatingbar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateHeathbar(float cvalue , float maxvalue)
    {
        slider.value = cvalue / maxvalue;
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
