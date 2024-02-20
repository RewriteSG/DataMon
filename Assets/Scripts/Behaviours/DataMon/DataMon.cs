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
        [HideInInspector] public List<string> DataMonNames = new List<string>();

        public DataMonIndividualData dataMon;
        [HideInInspector]public DataMonAI dataMonAI;
        [HideInInspector] public Databytes _databytes;
        public DataMonAttributes baseAttributes;
        public DataMonInstancedAttributes CurrentAttributes;
        public GameObject NamePlate;
        public TextMeshProUGUI NamePlateText, TaskText;
        HealthBarScript healthBar;


        public int DataMonAttacksID;
        public GameObject DataMonAttacksParentObj;

        [HideInInspector] public Vector2 SpawnedFromChunk;


        public AttackObjects[] attackObjects;

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
                baseAttributes = DataMonAttributes.CopyDataMonAttributes(dataMon.BaseAttributes);
                healthBar.SetMaxHealth(Mathf.RoundToInt(baseAttributes.BaseHealth));

            }
            DataMonAttacksID = GameManager.TotalDataMonIDs++;
            DataMonAttacksParentObj = new GameObject(DataMonAttacksID.ToString());
            DataMonAttacksParentObj.transform.SetParent(transform.parent);
        }

        private void Start()
        {
            //if (Vector2.Distance(transform.position, GameManager.instance.Player.transform.position) > GameManager.instance.RenderDistance
            //    && dataMon.MonBehaviourState != DataMonBehaviourState.isCompanion)
            //{
            //    RoamingSpawner.doot_doot--;
            //    isBeingCaptured = true;
            //    DestroyDataMon();

            //}
            GameManager.instance.Entity_Updates += ToUpdate;
        }
        void ToUpdate()
        {
            if (gameObject == null)
                return;
            if (NamePlateText != null)
            {
                NamePlate.transform.rotation = Quaternion.Euler(Vector3.zero);
                healthBar.SetHealth(Mathf.RoundToInt(CurrentAttributes.CurrentHealth));
            }
            if (!gameObject.activeSelf)
                return;
            if(CurrentAttributes.CurrentHealth <= 0 && dataMon.MonBehaviourState != DataMonBehaviourState.isCompanion)

            {
                GetComponent<Databytes>().DataMonIsDestroyed();
                DestroyDataMon();
            }
            if (CurrentAttributes.CurrentHealth <= 0 && dataMon.MonBehaviourState == DataMonBehaviourState.isCompanion)

            {
                Destroy(gameObject);
            }
            if (dataMonAI == null)
            {
                return;
            }
            if (dataMon.MonBehaviourState == DataMonBehaviourState.isCompanion)
            switch (dataMonAI.CurrentTask)
            {
                case AI_Tasks.Attacking:
                    TaskText.text = "Task: Attacking";
                    break;
                case AI_Tasks.Patrolling:
                    TaskText.text = "Task: Patrolling";
                    break;
                case AI_Tasks.ProducingAmmo:
                    TaskText.text = string.Format("Task: Producing Ammo " +"({0}s / {1}s)", 
                        dataMonAI.CurrentProductionProgress.ToString("#.0") ,Mathf.RoundToInt(dataMonAI.DataMonProductionTime).ToString("#.0"));
                    break;
                case AI_Tasks.ProducingHuntingRifle:
                    TaskText.text = string.Format("Task: Producing Hunting Rifle" + "({0}s / {1}s)",
                        dataMonAI.CurrentProductionProgress.ToString("#.0") ,Mathf.RoundToInt(dataMonAI.DataMonProductionTime).ToString("#.0"));
                    break;
                case AI_Tasks.ProducingShotgun:
                    TaskText.text = string.Format("Task: Producing Shotgun" + "({0}s / {1}s)",
                        dataMonAI.CurrentProductionProgress.ToString("#.0") ,Mathf.RoundToInt(dataMonAI.DataMonProductionTime).ToString("#.0"));
                    break;
                case AI_Tasks.ProducingAssaultRifle:
                    TaskText.text = string.Format("Task: Producing Assault Rifle" + "({0}s / {1}s)",
                        dataMonAI.CurrentProductionProgress.ToString("#.0") ,Mathf.RoundToInt(dataMonAI.DataMonProductionTime).ToString("#.0"));
                    break;
                default:
                    TaskText.text = "";

                    break;
            }
            else
                TaskText.text = "";
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
        //public bool SetDataMon(GameObject ToDataMon)
        //{
        //    if (dataMonData == null)
        //    {
        //        return false;
        //    }
        //    dataMon = new DataMonIndividualData(dataMonData.DataMons.GetDataMonInDataArray(ToDataMon));
        //    dataMonCurrentAttributes = new DataMonInstancedAttributes(dataMon.BaseAttributes);
            
        //    return true;
        //}
        //public bool SetDataMon(string ToDataMon)
        //{
        //    if (dataMonData == null)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        dataMon = new DataMonIndividualData(dataMonData.DataMons.GetDataMonInDataArray(ToDataMon));
        //        dataMonCurrentAttributes = new DataMonInstancedAttributes(dataMon.BaseAttributes);
        //        return true;
        //    }
        //}
        public void SetDataMon(DataMonIndividualData ToDataMon)
        {
            tier = dataMonData.DataMons.GetDataMonIndexInDataArray(ToDataMon);

            dataMon = DataMonIndividualData.CloneDataMonClass(ToDataMon);
            CurrentAttributes = new DataMonInstancedAttributes(baseAttributes);

        }
        public void SetAttributes(DataMonInstancedAttributes instancedAttributes)
        {
            CurrentAttributes = instancedAttributes;

            if (dataMonAI == null)
                return;
            if (dataMonAI.aggroSystem == null)
                return;
            dataMonAI.aggroSystem.ListOfTargets.ListOfTargets.Clear();
        }
        public void ResetAttributes()
        {
            CurrentAttributes.ResetAttributes(baseAttributes);
            if (dataMonAI == null)
                return;
            if (dataMonAI.aggroSystem == null)
                return;
            dataMonAI.aggroSystem.ListOfTargets.ListOfTargets.Clear();
            dataMonAI.AI_state = AI_State.Patrol;
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
            NamePlateText.color = GameManager.instance.CompanionColor;
        }

        public void StartPassive()
        {
            DataMon _this = this;
            for (int i = 0; i < dataMonData.InherentPassives.Length; i++)
            {
                dataMonData.InherentPassives[i].OwnPassives();
                dataMonData.InherentPassives[i].CallDelegates(ref _this, ref GameManager.instance, true);
            }
        }

        public void SetDataMonHostile()
        {
            if (dataMon.MonBehaviourState != DataMonBehaviourState.isHostile)
                GameManager.HostileDataMons++;
            dataMon.MonBehaviourState = DataMonBehaviourState.isHostile;
            NamePlateText.color = GameManager.instance.HostileColor;
        }
        public void SetDataMonNeutral()
        {
            dataMon.MonBehaviourState = DataMonBehaviourState.isNeutral;
            NamePlateText.color = GameManager.instance.NeutralColor;

        }
        private void OnDestroy()
        {
            if (dataMonAI == null || dataMon.MonBehaviourState != DataMonBehaviourState.isCompanion || _databytes == null)
                return;
            if (_databytes.isQuitting)
                return;

            StartPassive();

            Destroy(dataMonAI.patrollingAnchor);
            Destroy(DataMonAttacksParentObj);
            GameManager.instance.Entity_Updates -= ToUpdate;

        }
        public void DestroyDataMon()
        {
            gameObject.SetActive(false);
            ResetAttributes();
            RoamingSpawner.doot_doot--;
            if(RoamingSpawner.MonsInChunk.TryGetValue(SpawnedFromChunk,out DataMonsInChunk value))
            {
                value.datamons--;
            }

            if (dataMonAI == null)
                return;
            
            if (dataMon.MonBehaviourState == DataMonBehaviourState.isHostile && GameManager.instance.HostileDataMonsGOs.Contains(gameObject))
            {
                GameManager.HostileDataMons--;
                GameManager.instance.HostileDataMonsGOs.Remove(gameObject);
            }
            if (dataMonAI.patrollingAnchor != null)
                dataMonAI.patrollingAnchor.SetActive(false);

            RoamingSpawner.DataMonsPool.TryGetValue(dataMon.DataMonName, out List<DataMon> temp);
            temp.Add(this);
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("PlayerRenderDist"))
            {
                isBeingCaptured = true;
                DestroyDataMon();


            }
        }
        public void SetAttributesByModifier(float modifier)
        {
            baseAttributes.BaseHealth = dataMon.BaseAttributes.BaseHealth * modifier;
            baseAttributes.BaseAttack = dataMon.BaseAttributes.BaseAttack * modifier;
            healthBar.SetMaxHealth(Mathf.RoundToInt(baseAttributes.BaseHealth));

            if (CurrentAttributes.CurrentHealth >= dataMon.BaseAttributes.BaseHealth)
            {
                CurrentAttributes.CurrentHealth = baseAttributes.BaseHealth;
            }
        }
        public void DataMonStartBuff(float buffDuration, float Modifer)
        {
            StartCoroutine(BuffDataMon(buffDuration, Modifer));
        }
        IEnumerator BuffDataMon(float buffDuration,float Modifier)
        {
            SetAttributesByModifier(Modifier);
            yield return new WaitForSeconds(buffDuration);
            SetAttributesByModifier(1);

        }
    }
    
}


