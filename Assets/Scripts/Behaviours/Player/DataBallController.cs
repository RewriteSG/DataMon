using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBallController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float forceWhenShot;
    [HideInInspector]public bool isCapturing = false;
    private float DataMonCaptureChance = 0;
    public float CaptureProgress = 0;
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
    DataDexIO CaptureTarget;
    GameObject capturingGameObj;
    DataMon.IndividualDataMon.DataMon dataMon;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "DataMon")
            return;
        dataMon = collision.transform.parent.gameObject.GetComponent<DataMon.IndividualDataMon.DataMon>();
        if (!dataMon.isBeingCaptured)
        {
            dataMon.isBeingCaptured = true;
            isCapturing = true;

            CaptureProgress = 0;
            rb.velocity = Vector2.zero;
            CaptureTarget = new DataDexIO();

            CaptureTarget.toDataDex = new DataMonHolder(dataMon);
            DataMonCaptureChance = CaptureTarget.toDataDex.dataMonAttributes.CurrentCaptureChance;

            StartCoroutine(CapturingDataMon(GameManager.instance.CaptureDelay));
            capturingGameObj = collision.transform.parent.gameObject;
            capturingGameObj.SetActive(false);
        }
    }
    float randomizedChance = 0;
    IEnumerator CapturingDataMon(float delay)
    {
        yield return new WaitForSeconds(delay);

        randomizedChance = Random.Range(1, 101);
        print("first chance " + randomizedChance + " With Chance "+DataMonCaptureChance);

        if (randomizedChance <= DataMonCaptureChance)
        {
            CaptureProgress += DataMonCaptureChance;
            DataMonCaptureChance = Mathf.Lerp(DataMonCaptureChance, 100, 0.5f);
        }
        else
        {
            capturingGameObj.SetActive(true);
            goto ReturnFromCoroutine;
        }
        if (CaptureProgress>= 100)
        {
            CaptureTarget.SendToDataDex();
            Destroy(capturingGameObj);

            goto ReturnFromCoroutine;
        }

        yield return new WaitForSeconds(delay);

        randomizedChance = Random.Range(0, 101);
        print("Second chance " + randomizedChance + " With Chance " + DataMonCaptureChance);
        if (randomizedChance <= DataMonCaptureChance)
        {
            CaptureProgress += DataMonCaptureChance;
        }
        else
        {
            capturingGameObj.SetActive(true);
            goto ReturnFromCoroutine;

        }
        print("captured");

        CaptureTarget.SendToDataDex();
        Destroy(capturingGameObj);


        ReturnFromCoroutine:
        CaptureTarget = null;
        capturingGameObj = null;
        
        Destroy(gameObject);
    }
}
