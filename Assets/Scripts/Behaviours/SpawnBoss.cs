using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(2)]
public class SpawnBoss : MonoBehaviour
{
    public float delay;
    public GameObject BossObject;
    public DataMonsData bossdata;
    public IndividualDataMon.DataMon Boss;
    public GameObject MegaByte;
    public bool FightWon;
    // Start is called before the first frame update
    void Awake()
    {
        FightWon = false;

        bossdata.DataMons[0].DataMonPrefab.GetComponent<IndividualDataMon.DataMon>().SetDataMon(bossdata.DataMons[0]);
        StartCoroutine(ActivateBoss());
    }
    IEnumerator ActivateBoss()
    {

        Boss = BossObject.GetComponent<IndividualDataMon.DataMon>();
        Boss.SetDataMon(Boss.dataMonData.DataMons[0]);

        yield return new WaitForEndOfFrame();
        Boss.SetAttributes(new DataMonInstancedAttributes(Boss.dataMon.BaseAttributes));


        Boss.isWaveEnemy = true;
        yield return new WaitForEndOfFrame();
        Boss.SetDataMonHostile();


        Boss.dataMonAI.ChangeAttackTargetEnemy(GameManager.instance.Player.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(BossObject == null)
        {
            FightWon = true;
        }
    }
    public void LoadSceneToHub()
    {
        SceneChanger.ChangeScene("DataHub");
    }
}
