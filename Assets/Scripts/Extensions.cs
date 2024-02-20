using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static List<DamagedBy> RemoveNullReferencesFromArray(this List<DamagedBy> array)
    {
        List<DamagedBy> temp = new List<DamagedBy>();

        for (int i = 0; i < array.Count; i++)
        {
            if (array[i].byGameObject != null)
                    temp.Add(array[i]);
        }
        return temp;
    }
    public static List<T> Add<T>(this List<T> list, List<T> toAdd)
    {

        for (int i = 0; i < toAdd.Count; i++)
        {
            list.Add(toAdd[i]);
        }
        return list;
    }
}

public static class TransformExtensions
{
    public static Transform FindChildByTag<T>(this T transform, string tag) where T : Transform
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag(tag))
                return (transform.GetChild(i));
        }
        return null;
    }
}
public static class GameObjectExtensions
{
    public static bool isNull<T>(this T obj) where T : Object
    {
        return obj == null;
    }
    public static List<GameObject> RemoveNullReferencesInList(this List<GameObject> list)
    {
        List<GameObject> temp = new List<GameObject>();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null)
            temp.Add(list[i]);
        }
        return temp;
    }
    public static DataMonHolder[] GetDataMonHoldersFromArray(this List<GameObject> array)
    {
        List<DataMonHolder> temp = new List<DataMonHolder>();

        for (int i = 0; i < array.Count; i++)
        {
            temp.Add(array[i].GetComponent<DataMonButton>().dataMonHolder);
        }
        return temp.ToArray();
    }
}
public static class Collider2DExtensions
{
    public static bool ColliderArrayHasTag<T>(this T[] array, string tag) where T : Collider2D
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].gameObject.tag == tag)
            {
                return true;
            }
        }
        return false;
    }
    public static bool ColliderArrayHasGameObject<T>(this T[] array, GameObject obj) where T : Collider2D
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].gameObject == obj)
            {
                return true;
            }
        }
        return false;
    }
    public static bool ColliderArrayHasGameObject<T>(this T[] array, GameObject obj, bool isChildOfObj) where T : Collider2D
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].transform.parent == null)
                continue;
            if (array[i].transform.parent.gameObject == obj)
                return true;
        }
        return false;
    }
   
}
public static class DataMonHolderExtensions
{
    public static bool isNull(this DataMonHolder holder)
    {
        if (holder != null)
            return holder.dataMon.DataMonPrefab == null;
        return holder == null;
    }
    public static DataMonHolder[] RemoveNullReferencesFromArray(this DataMonHolder[] array)
    {
        List<DataMonHolder> temp = new List<DataMonHolder>();

        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] != null)
#if UNITY_EDITOR
                if (array[i].dataMonData != null)
#endif
                    temp.Add(array[i]);
        }
        return temp.ToArray();
    }
}
public static class ArrayExtensions
{
    public static T[] ResizeToArray<T>(this T[] array, int resizeTo)
    {
        T[] temp = new T[resizeTo];

        for (int i = 0; i < array.Length; i++)
        {
            if (i < temp.Length)
                temp[i] = array[i];
            else break;
        }
        return temp;
    }
    public static int IndexOf<T>(this T[] array, T element)
    {
        int index = -1;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].Equals(element))
            {
                index = i;
                break;
            }
        }
        return index;
    }
    public static T[] RemoveAt<T>(this T[] array, int index)
    {
        List<T> temp = new List<T>();
        for (int i = 0; i < array.Length; i++)
        {
            if (i != index)
                temp.Add(array[i]);

        }
        return temp.ToArray();
    }
    public static List<T> ToList<T>(this T[] array)
    {
        List<T> temp = new List<T>();
        for (int i = 0; i < array.Length; i++)
        {
            temp.Add(array[i]);
        }
        return temp;
    }
}
public static class DataMonsDataExtensions
{
    public static DataMonIndividualData GetDataMonInDataArray<T>(this T[] array, string dataMon) where T : DataMonIndividualData
    {
        DataMonIndividualData toReturn = null;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].DataMonName == dataMon)
            {
                toReturn = array[i];
                break;
            }
        }
        return toReturn;
    }
    public static DataMonIndividualData GetDataMonInDataArray<T>(this T[] array, GameObject dataMon) where T : DataMonIndividualData
    {
        DataMonIndividualData toReturn = null;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].DataMonPrefab == dataMon)
            {
                toReturn = array[i];
                break;
            }
        }
        return toReturn;
    }
    public static DataMonIndividualData GetDataMonInDataArray<T>(this T[] array, DataMonIndividualData dataMon) where T : DataMonIndividualData
    {
        DataMonIndividualData toReturn = null;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].DataMonPrefab == dataMon.DataMonPrefab)
            {
                toReturn = array[i];
                break;
            }
        }
        return toReturn;
    }
    public static int GetDataMonIndexInDataArray<T>(this T[] array, DataMonIndividualData dataMon) where T : DataMonIndividualData
    {
        int temp = 0;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].DataMonPrefab == dataMon.DataMonPrefab)
            {
                temp = i;
                break;
            }
        }
        return temp;
    }
    public static int GetDataMonIndexInDataArray<T>(this T[] array, string dataMon) where T : DataMonIndividualData
    {
        int temp = 0;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].DataMonName == dataMon)
            {
                temp = i;
                break;
            }
        }
        return temp;
    }
}
public static class DamageByExtension
{
    public static bool GetDamagedByGameObject(this List<DamagedBy> dmgByList,GameObject go, out DamagedBy Result)
    {
        bool isNull = false;
        Result = null;
        for (int i = 0; i < dmgByList.Count; i++)
        {
            if(dmgByList[i].byGameObject == go)
            {
                Result = dmgByList[i];
                isNull = true;
            }
        }

        return isNull;
    }
}
