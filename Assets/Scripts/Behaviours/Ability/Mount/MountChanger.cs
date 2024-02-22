using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountChanger : MonoBehaviour
{
    public GameObject[] Mount_Evolutions;
    public int currentTier;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Mount_Evolutions.Length; i++)
        {
            Mount_Evolutions[i].SetActive(false);
        }
        Mount_Evolutions[currentTier].SetActive(true); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
