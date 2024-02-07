using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [SerializeField] public float MovementSpeed;
    private Rigidbody2D rb;
    private Vector2 MovementDirection;

    public GameManager gm;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        MovementDirection = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));

        
    }
    void FixedUpdate()
    {
        rb.MovePosition((Vector2)transform.position+(MovementDirection * MovementSpeed*Time.deltaTime));
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Databytes")
        {
            gm.Databytes_Count += 10;
            Destroy(collision.gameObject);
        }
    }
}
