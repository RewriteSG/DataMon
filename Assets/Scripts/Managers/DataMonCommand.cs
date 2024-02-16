using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commands;
public class DataMonCommand : MonoBehaviour
{
    [SerializeField] DataMonCommands SerializedCommand;
    public static DataMonCommands command;
    public static DataMonCommand instance;
    public static GameObject ToTarget;
    public static IndividualDataMon.DataMon dataMon;
    public static LayerMask DataMonLayer;
    public static Vector3 gotoCoord;
    //public GameObject CameraToOffset;
    //public GameObject Evolution_canvas;
    public LayerMask _DataMonLayer;
    //float defaultCamSize;
    //Transform CamOnDataMon;
    //Camera CamPos, mainCam;
    //CameraFollow camFollow;
    private void Start()
    {
        instance = this; 
        //Evolution_canvas.SetActive(false);
        DataMonLayer = _DataMonLayer;
        //CamPos = Instantiate(CameraToOffset, transform).transform.GetComponentInChildren<Camera>();
        //CamOnDataMon = CamPos.transform.parent;
        //mainCam = Camera.main;
        //camFollow = mainCam.GetComponent<CameraFollow>();
        //defaultCamSize = mainCam.orthographicSize;
    }
    private void Update()
    {
        SerializedCommand = command;
        if(ToTarget == null && command == DataMonCommands.TargetEnemy)
        {
            command = DataMonCommands.Patrol;
        }
        //if (Evolution_canvas.activeSelf)
        //{
        //    camFollow.toFollow = CamPos.transform;
        //    CamOnDataMon.position = dataMon.transform.position;
        //    mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, CamPos.orthographicSize, 0.3f);
        //    DataDex.instance.DisplayDataDex = false;
        //}
        //else
        //{

        //    camFollow.toFollow = GameManager.instance.Player.transform;
        //    mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, defaultCamSize, 0.3f);
        //}
    }
    public static void OnCommandLeftClick()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,Mathf.Infinity,DataMonLayer);

        if (hit.collider == null)
            return;
        dataMon = hit.collider.transform.parent.GetComponent<IndividualDataMon.DataMon>();
        //if(dataMon.dataMon.MonBehaviourState == DataMonBehaviourState.isCompanion && !instance.Evolution_canvas.activeSelf)
        //{
        //    instance.Evolution_canvas.SetActive(true);
        //}
        if(dataMon.dataMon.MonBehaviourState != DataMonBehaviourState.isCompanion)
        {
            TargetEnemy(hit.collider.transform.parent.gameObject);
        }
    }
    public static void OnCommandMiddleClick()
    {

    }
    public void EvolveDataMon()
    {
        dataMon.tier += 1;
    }
    public static void TargetEnemy(GameObject enemy)
    {
        command = DataMonCommands.TargetEnemy;
        ToTarget = enemy;


    }
    public static void DontAttack()
    {
        command = DataMonCommands.DontAttack;
    }
    public static void AttackAggressive()
    {
        command = DataMonCommands.AttackAggressive;
    }
    public static void Patrol()
    {
        command = DataMonCommands.Patrol;
    }
    public static void GoTo()
    {
        command = DataMonCommands.Patrol;
    }
}
namespace Commands
{
    public enum DataMonCommands
    {
        TargetEnemy, DontAttack, AttackAggressive, Patrol, GoTo
    }

}
