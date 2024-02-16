using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DatamonEvolution : MonoBehaviour
{
    IndividualDataMon.DataMon dataMon;
    public GameObject Evolution_canvas;

    public GameManager gamemanager;

    // Start is called before the first frame update
    void Start()
    {
        Evolution_canvas.SetActive(false);
        dataMon = GetComponent<IndividualDataMon.DataMon>();
    }

    // Update is called once per frame
    void Update()
    {

        Evolution_canvas.transform.rotation = Quaternion.Euler(Vector3.zero);
        
        if(gamemanager.Databytes_Count>=0)
        {
            if(Input.GetMouseButtonDown(0))
            {
                //Instantiate(Evolution_canvas, transform, false);
                Evolution_canvas.SetActive(true);
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            Evolution_canvas.SetActive(false);
        }
        

    }
    public void TierIncrease()
    {
        dataMon.tier += 1;
    }
}
