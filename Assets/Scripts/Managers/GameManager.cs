using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    // Global Variables
    public static GameManager instance;
    public static int TotalDataMonIDs;

    public GameObject Player;
    [HideInInspector] public PlayerShoot playerShootScript;
    [HideInInspector] public Rigidbody2D playerRb;
    public float PlayerDataMonPatrolMinDist;
    public float PlayerDataMonPatrolMaxDist;
    public float DataMonSpawnRadiusFromPlayer;
    public float MaxDistForCompanionDataMon;
    public float CaptureDelay = 1;
    public float DataMonsRotationSpeed;
    public int MaxNumberOfWildDataMons = 15;
    public int NumberOfDataMonsInTeam = 1;
    public int Databytes_Count = 0;
    public float RenderDistance = 10f;
    public Color NeutralColor, HostileColor, CompanionColor;


    [Header("GUNS")]
    public int NumberOfBulletsInPool;
    public float ShotgunPelletPerRnd;
    public float MeleeDelay,HuntingRifleDelay, ShotgunDelay, AssaultRifleDelay;
    GameObject BulletsPoolGameObject;
    [HideInInspector] public Dictionary<bool, List<BulletInstance>> BulletsPool = new Dictionary<bool, List<BulletInstance>>();

    // Prefabs
    public GameObject Bullet;
    public GameObject Data_bytes;
    public PlayerProgress player_progress = new PlayerProgress();

    GameObject PlayerRenderDistTrigger;

    private void Awake()
    {
        TotalDataMonIDs = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        if (Player == null)
            return;
        playerRb = Player.GetComponent<Rigidbody2D>();
        playerShootScript = Player.GetComponent<PlayerShoot>();
        BulletsPoolGameObject = new GameObject("BulletPool");
        List<BulletInstance> unactiveBullets = new List<BulletInstance>();
        for (int i = 0; i < NumberOfBulletsInPool; i++)
        {
            unactiveBullets.Add(Instantiate(Bullet, BulletsPoolGameObject.transform).GetComponent<BulletInstance>());
            unactiveBullets[unactiveBullets.Count - 1].transform.SetParent(BulletsPoolGameObject.transform);
            unactiveBullets[unactiveBullets.Count - 1].gameObject.SetActive(false);
        }
        BulletsPool.Add(false, unactiveBullets);
        BulletsPool.Add(true, new List<BulletInstance>()); 
        ReferencePlayerRenderDistTrigger();

    }

    private void OnValidate()
    {
        if (Player == null)
            return;
        ReferencePlayerRenderDistTrigger();
        if (PlayerRenderDistTrigger == null)
            return;
        PlayerRenderDistTrigger.transform.localScale = Player.transform.InverseTransformVector(Vector3.one * RenderDistance);
    }

    private void ReferencePlayerRenderDistTrigger()
    {
        if (PlayerRenderDistTrigger == null || PlayerRenderDistTrigger.transform.parent != Player.transform)
        {
            PlayerRenderDistTrigger = Player.transform.FindChildByTag("PlayerRenderDist").gameObject;

        }
    }

    // Update is called once per frame
    void Update()
    {
        NumberOfDataMonsInTeam = Mathf.Clamp(NumberOfDataMonsInTeam, 1, NumberOfDataMonsInTeam+1);
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Player.transform.position, PlayerDataMonPatrolMinDist);
        Gizmos.DrawWireSphere(Player.transform.position, PlayerDataMonPatrolMaxDist);
        Gizmos.DrawWireSphere(Player.transform.position, MaxDistForCompanionDataMon);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Player.transform.position, DataMonSpawnRadiusFromPlayer);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Player.transform.position, RenderDistance);

    }
    BulletInstance b_instance;
    public BulletInstance GetAvailableBullet()
    {
        BulletsPool.TryGetValue(false, out List<BulletInstance> b);
        if (b.Count == 0)
            return null;
        b_instance = b[0];
        b.RemoveAt(0);
        b_instance.gameObject.SetActive(true);
        return b_instance;
    }
}
[System.Serializable]
public class PlayerProgress
{
    public Item Melee =  new Item();
    public Item DataBall = new Item();
    public Item Command = new Item();
    public Item HuntingRifle = new Item();
    public Item Shotgun = new Item();
    public Item AssaultRifle = new Item();
    public PlayerProgress() { }
}
[System.Serializable]
public class Item
{
    public GameObject ItemPrefab;
    [HideInInspector]public GameObject ItemInstance;
    public bool isUnlocked;
    public Item() { }
    public void InstantiatePrefab(Transform Hotbar)
    {
        ItemInstance = Object.Instantiate(ItemPrefab, Hotbar);
    }
}
