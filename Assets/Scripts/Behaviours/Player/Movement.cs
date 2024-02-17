using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [SerializeField] public float MovementSpeed;
    private Rigidbody2D rb;
    private Vector2 MovementDirection;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameManager.instance.Entity_Updates += ToUpdate;
        GameManager.instance.Entity_FixedUpdates += ToFixedUpdate;
    }

    // Update is called once per frame
    void ToUpdate()
    {
        MovementDirection = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));

        
    }
    void ToFixedUpdate()
    {
        rb.MovePosition((Vector2)transform.position+(MovementDirection * MovementSpeed*Time.deltaTime));
    }
    private void OnDestroy()
    {
        GameManager.instance.Entity_Updates -= ToUpdate;
        GameManager.instance.Entity_FixedUpdates -= ToFixedUpdate;

    }
}
