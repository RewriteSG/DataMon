using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBallController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float forceWhenShot;
    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce((transform.up * forceWhenShot)+(Vector3)GameManager.instance.playerRb.velocity, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
