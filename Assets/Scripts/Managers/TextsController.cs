using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextsController : MonoBehaviour
{
    public TextMeshProUGUI Text_Databytes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Text_Databytes.text = "x"+GameManager.instance.Databytes_Count;
    }
}
