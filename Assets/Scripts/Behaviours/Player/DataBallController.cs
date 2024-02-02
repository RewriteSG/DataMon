using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBallController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float forceWhenShot;
    [HideInInspector]public bool isCapturing = false;
    private float DataMonCaptureChance = 0;
    // Start is called before the first frame update
    void Start()
    {
        isCapturing = false;
        rb.AddForce((transform.up * forceWhenShot)+(Vector3)GameManager.instance.playerRb.velocity, ForceMode2D.Impulse);
        StartCoroutine(DestroyBall(PlayerShoot.DestroyBallAtDelay));
    }
    IEnumerator DestroyBall(float destroyAtDelay)
    {
        yield return new WaitForSeconds(destroyAtDelay);
        if (!isCapturing)
            Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DataMon")
        {
            rb.velocity = Vector2.zero;
            isCapturing = true;
            Destroy(collision.transform.parent.gameObject);
        }
    }
}
