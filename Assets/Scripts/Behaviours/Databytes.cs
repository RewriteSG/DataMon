using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Databytes : MonoBehaviour
{
    public GameObject Data_bytes;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            
            Instantiate(Data_bytes, transform.position, Quaternion.identity);
            Destroy(this.gameObject);

        }
    }
}
