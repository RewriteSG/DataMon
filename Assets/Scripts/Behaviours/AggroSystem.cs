using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DataMonAI))]
public class AggroSystem : MonoBehaviour
{
    public DataMonAI datamonAI;
    public AggroList ListOfTargets= new AggroList();
    // Start is called before the first frame update
    void Start()
    {
        datamonAI = GetComponent<DataMonAI>();
        datamonAI.aggroSystem = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public class AggroList
{
    public List<DamagedBy> ListOfTargets = new List<DamagedBy>();
    public AggroList()
    {
        ListOfTargets.Clear();
    }
    private float HighDamageCount;
    private GameObject HighestDamageDealer;
    public GameObject FindHighestDamageDealer()
    {
        HighDamageCount = -999;
        HighestDamageDealer = null;
        for (int i = 0; i < ListOfTargets.Count; i++)
        {
            if (ListOfTargets[i].damage > HighDamageCount)
            {
                HighDamageCount = ListOfTargets[i].damage;
                HighestDamageDealer = ListOfTargets[i].byGameObject;
            }
        }
        return HighestDamageDealer;
    }
}
public class DamagedBy
{
    public GameObject byGameObject;
    public float damage;
}
