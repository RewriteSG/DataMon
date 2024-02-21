using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Databytes : MonoBehaviour
{
    IndividualDataMon.DataMon dataMon;
    [HideInInspector] public bool isQuitting;
    public bool isItemPickup;
    [SerializeField]Transform DataByteSprite;
    public float radius, damp;
    public Vector2 randomPos, smoothVelocity = Vector2.zero;
    Vector3 randomRotation;

    // Start is called before the first frame update
    void Start()
    {
        isQuitting = false;
        if (isItemPickup)
        {
            randomPos = Random.insideUnitCircle * Random.Range(0, radius);
            //randomPos += (Vector2)transform.position;
            randomRotation = new Vector3(0, 0, Random.Range(0, 361));
            DataByteSprite.transform.rotation = Quaternion.identity;
            return;
        }
        dataMon = GetComponent<IndividualDataMon.DataMon>();
        dataMon._databytes = this;
    }
    private void Update()
    {
        if (!isItemPickup)
            return;
        DataByteSprite.transform.localPosition = Vector2.SmoothDamp(DataByteSprite.transform.localPosition, randomPos, ref smoothVelocity, damp);
        DataByteSprite.transform.localRotation = Quaternion.RotateTowards(DataByteSprite.localRotation, Quaternion.Euler(randomRotation), 25);

    }
    //toRotate = Quaternion.LookRotation(transform.forward, -Dir);
                //newRotation = Quaternion.RotateTowards(transform.rotation, toRotate, GameManager.instance.DataMonsTargetingRotationSpeed * Time.fixedDeltaTime);
    //offset = Vector3.forward* transform.position.z;
    //targetPos = toFollow.position + offset;
    //    newCamPos = Vector3.SmoothDamp(transform.position, targetPos, ref smoothVelocity, Damp);
    //    // Update is called once per frame
    //    void Update()
    //    {
    //        if(Input.GetMouseButtonDown(0))
    //        {

    //            Destroy(this.gameObject);

    //        }
    //    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }
    int chanced;
    public void DataMonIsDestroyed()
    {
        if (dataMon == null || isQuitting)
            return;
        if (!dataMon.isBeingCaptured)
            Instantiate(GameManager.instance.DatabytesPrefab, transform.position, Quaternion.identity);
        chanced = Random.Range(0, 100);
        
        if (!dataMon.isBeingCaptured && chanced <= GameManager.instance.ChanceForDoubleDrop)
        {

            Instantiate(GameManager.instance.DatabytesPrefab, transform.position, Quaternion.identity);
        }
    }
}
