using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// This script is to convert the Data from DataMonsData to raw data for the scripts to use.
/// </summary>
namespace IndividualDataMon
{
    public class DataMon : MonoBehaviour
    {
        public int tier = 0;
        [HideInInspector]public bool isBeingCaptured = false;
        [HideInInspector] public bool isCompanion = false;
        public DataMonsData dataMonData;
        public GameObject test;
        [HideInInspector] public int selected;
        public DataMonIndividualData dataMon;
        [HideInInspector]public DataMonAI dataMonAI;
        public DataMonInstancedAttributes dataMonCurrentAttributes;
        public GameObject NamePlate;
        public TextMeshProUGUI NamePlateText;
        HealthBarScript healthBar;
        //[SerializeField]private GameObject test;
        private void Awake()
        {
            //if (gameObject.name.Contains("(Clone)"))
            //    SetDataMon(gameObject.name.Replace("(Clone)", ""));
            ////SetDataMonCompanion();
            //else
            //    SetDataMon(test);
            if (NamePlateText != null)
            {
                NamePlateText.text = dataMon.DataMonName;
                healthBar = NamePlate.GetComponentInChildren<HealthBarScript>();
                healthBar.SetMaxHealth(Mathf.RoundToInt(dataMon.BaseAttributes.BaseHealth));

            }


        }

        private void Start()
        {
            if (Vector2.Distance(transform.position, GameManager.instance.Player.transform.position) > GameManager.instance.RenderDistance
                && dataMon.MonBehaviourState != DataMonBehaviourState.isCompanion)
            {
                RoamingSpawner.doot_doot--;
                isBeingCaptured = true;
                DestroyDataMon();

            }
        }
        void Update()
        {
            if (NamePlateText != null)
            {
                NamePlate.transform.rotation = Quaternion.Euler(Vector3.zero);
                healthBar.SetHealth(Mathf.RoundToInt(dataMonCurrentAttributes.CurrentHealth));
            }
            
        }
        private void OnEnable()
        {
            isBeingCaptured = false;
        }
        /// <summary> 
        /// Returns false if dataMonData is null
        /// </summary>
        /// <param name="ToDataMon"></param>
        /// <returns></returns>
        public bool SetDataMon(GameObject ToDataMon)
        {
            if (dataMonData == null)
            {
                return false;
            }
            dataMon = new DataMonIndividualData(dataMonData._DataMon.GetDataMonInDataArray(ToDataMon));
            dataMonCurrentAttributes = new DataMonInstancedAttributes(dataMon.BaseAttributes);
            
            return true;
        }
        public bool SetDataMon(string ToDataMon)
        {
            if (dataMonData == null)
            {
                return false;
            }
            else
            {
                dataMon = new DataMonIndividualData(dataMonData._DataMon.GetDataMonInDataArray(ToDataMon));
                dataMonCurrentAttributes = new DataMonInstancedAttributes(dataMon.BaseAttributes);
                return true;
            }
        }
        public void SetDataMon(DataMonIndividualData _datamon)
        {
            dataMon = new DataMonIndividualData(_datamon);
            dataMonCurrentAttributes = new DataMonInstancedAttributes(dataMon.BaseAttributes);
        }
        //public int GetDataMonIndexInData(DataMonIndividualData[] array, GameObject DataMon)
        //{
        //    int toReturn = -1;
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        if (array[i].DataMonPrefab == DataMon)
        //        {
        //            toReturn = i;
        //            break;
        //        }
        //    }
        //    return toReturn;
        //}
        public void SetDataMonData(DataMonsData toData)
        {
            dataMonData = toData;
        }
        public void SetDataMonCompanion()
        {
            dataMon.MonBehaviourState = DataMonBehaviourState.isCompanion;
            RoamingSpawner.doot_doot--;
            NamePlateText.color = GameManager.instance.CompanionColor;
        }
        public void SetDataMonHostile()
        {
            dataMon.MonBehaviourState = DataMonBehaviourState.isHostile;
            NamePlateText.color = GameManager.instance.HostileColor;

        }
        public void SetDataMonNeutral()
        {
            dataMon.MonBehaviourState = DataMonBehaviourState.isNeutral;
            NamePlateText.color = GameManager.instance.NeutralColor;

        }
        //private void OnDestroy()
        //{
        //    DestroyPatrolAnchor();
        //}
        public void DestroyDataMon()
        {
            gameObject.SetActive(false);
            if (dataMonAI == null)
                return;

            if (dataMonAI.patrollingAnchor != null)
                dataMonAI.patrollingAnchor.SetActive(false);

            RoamingSpawner.DataMonsPool.TryGetValue(dataMon.DataMonName, out List<DataMon> temp);
            temp.Add(this);
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("PlayerRenderDist"))
            {
                RoamingSpawner.doot_doot--;
                isBeingCaptured = true;
                DestroyDataMon();


            }
        }
    }
    
}


