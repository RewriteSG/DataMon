﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script is to convert the Data from DataMonsData to raw data for the scripts to use.
/// </summary>
namespace DataMon.IndividualDataMon
{
    public class DataMon : MonoBehaviour
    {
        public int tier = 0;
        [HideInInspector]public bool isBeingCaptured = false;
        public DataMonsData dataMonData;
        public DataMonIndividualData dataMon;
        [HideInInspector]public DataMonAI dataMonAI;
        public DataMonInstancedAttributes dataMonCurrentAttributes;

        [SerializeField]private GameObject test;
        private void Start()
        {
            SetDataMon(test);
            //SetDataMonCompanion();
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
            else
            {
                dataMon = new DataMonIndividualData(dataMonData._DataMon.GetDataMonInDataArray(ToDataMon));
                dataMonCurrentAttributes = new DataMonInstancedAttributes(dataMon.BaseAttributes);
                return true;
            }
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
        }
        public void SetDataMonHostile()
        {
            dataMon.MonBehaviourState = DataMonBehaviourState.isHostile;
        }
        public void SetDataMonNeutral()
        {
            dataMon.MonBehaviourState = DataMonBehaviourState.isNeutral;
        }
        private void OnDestroy()
        {
            if (dataMonAI != null)
            {
                Destroy(dataMonAI.patrollingAnchor);
            }
        }
    }
    
}

