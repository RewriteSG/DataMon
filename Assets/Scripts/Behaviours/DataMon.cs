using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script is to convert the Data from DataMonsData to raw data for the scripts to use.
/// </summary>
namespace DataMon.IndividualDataMon
{
    public class DataMon : MonoBehaviour
    {
        public DataMonsData dataMonData;
        public DataMonIndividualData dataMon;
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
                dataMon = dataMonData._DataMon.GetDataMonInDataArray(ToDataMon);
                return true;
            }
        }
        public int GetDataMonIndexInData(DataMonIndividualData[] array, GameObject DataMon)
        {
            int toReturn = -1;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].DataMonPrefab == DataMon)
                {
                    toReturn = i;
                    break;
                }
            }
            return toReturn;
        }
        public void SetDataMonData(DataMonsData toData)
        {
            dataMonData = toData;
        }
    }
}

