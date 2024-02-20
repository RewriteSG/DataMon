using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    // Global Variables
    public static GameManager instance;
    public static int TotalDataMonIDs;
    public static int HostileDataMons;
    [Header("Player Variables")]
    public GameObject Player;
    [HideInInspector] public PlayerShoot playerShootScript;
    [HideInInspector] public Rigidbody2D playerRb;
    public List<GameObject> HostileDataMonsGOs = new List<GameObject>();
    public float PlayerDataMonPatrolMinDist;
    public float PlayerDataMonPatrolMaxDist;
    public float DataMonSpawnRadiusFromPlayer, DataMonEnableRbInRadius;
    public float MaxDistForCompanionDataMon;
    public float CaptureDelay = 1;
    public int NumberOfDataMonsInTeam = 1;
    [Header("====================")]

    public float DataMonsTargetingRotationSpeed = 40;
    public float DataMonsRotationSpeed;
    public float DataMonInDataDexHPRegen = 2;
    public int MaxNumberOfWildDataMons = 15;
    [Header("WorldBorders")]
    public int DataWorldBorderLeftX,DataWorldBorderRightX, DataWorldBorderDownY, DataWorldBorderUpY;
    public float GlitchRespawnTime = 45;

    public int Databytes = 0;
    public GameObject RenderDistanceTrigger;
    public float RenderDistance = 10f;
    public Color NeutralColor, HostileColor, CompanionColor;
    public LayerMask GlitchLayerMask;
    public delegate void DataMonAIBehaviourStart(DataMonAI dataMonAI);
    public delegate void DataMonAIBehaviourUpdate();
    public delegate void EntityUpdates();
    public DataMonAIBehaviourStart DataMon_StartAI;
    public DataMonAIBehaviourUpdate DataMon_UpdateAI;
    public EntityUpdates Entity_Updates;
    public EntityUpdates Entity_FixedUpdates;

    [Header("Affects by DataMons Passives")]
    public bool isShielded;
    public float MaxShieldHealth;
    public float CurrentShieldHealth;
    public float ChanceForDoubleDrop = 0;
    public float PlayerRegenerationRatePerSecond = 1;
    public float AllDamageModifier = 1;

    [Header("GUNS")]
    public int NumberOfBulletsInPool;
    public float ShotgunPelletPerRnd;
    public float MeleeDelay,HuntingRifleDelay, ShotgunDelay, AssaultRifleDelay;
    GameObject BulletsPoolGameObject;
    [HideInInspector] public Dictionary<bool, List<BulletInstance>> BulletsPool = new Dictionary<bool, List<BulletInstance>>();

    public WeaponType Fists_weapon = new WeaponType();
    public WeaponType DataBallLauncher = new WeaponType();
    public WeaponType huntingRifle = new WeaponType();
    public WeaponType shotgun = new WeaponType();
    public WeaponType assaultRifle = new WeaponType();

    // Prefabs
    public GameObject Bullet;
    public GameObject DatabytesPrefab;
    [Header("Player progress")]
    public PlayerProgress player_progress = new PlayerProgress();

    public const int MaxLimitOfAbilities = 3;

    private void Awake()
    {
        instance = this;
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
        HostileDataMonsGOs.Clear();
        DataMon_StartAI = StartAI;

    }

    private void OnValidate()
    {
        if (Player == null)
            return;
        ReferencePlayerRenderDistTrigger();
        if (RenderDistanceTrigger == null)
            return;
        RenderDistanceTrigger.transform.localScale = Player.transform.InverseTransformVector(Vector3.one * RenderDistance);
    }

    private void ReferencePlayerRenderDistTrigger()
    {
        if (RenderDistanceTrigger == null || RenderDistanceTrigger.transform.parent != Player.transform)
        {
            RenderDistanceTrigger = Player.transform.FindChildByTag("PlayerRenderDist").gameObject;

        }
    }

    // Update is called once per frame
    void Update()
    {
        NumberOfDataMonsInTeam = Mathf.Clamp(NumberOfDataMonsInTeam, 1, NumberOfDataMonsInTeam+1);
        if (Entity_Updates != null)
            Entity_Updates();
    }
    private void FixedUpdate()
    {
        if (Entity_FixedUpdates != null)
            Entity_FixedUpdates();
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Player.transform.position, PlayerDataMonPatrolMinDist);
        Gizmos.DrawWireSphere(Player.transform.position, PlayerDataMonPatrolMaxDist);
        Gizmos.DrawWireSphere(Player.transform.position, MaxDistForCompanionDataMon);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Player.transform.position, DataMonSpawnRadiusFromPlayer);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Player.transform.position, DataMonEnableRbInRadius);
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

    void StartAI(DataMonAI dataMonAI)
    {
        DataMon_UpdateAI += dataMonAI.UpdateDatamonAI;
        StartCoroutine(StartPathing());
    }
    IEnumerator StartPathing()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            DataMon_UpdateAI();
            yield return new WaitForSeconds(1f);
        }
    }
    Collider2D[] Glitches;
    public bool CheckForGlitchesInProximity(out Collider2D Glitch)
    {
        Glitch = null;
        Glitches = Physics2D.OverlapCircleAll(instance.Player.transform.position, instance.PlayerDataMonPatrolMaxDist, instance.GlitchLayerMask);
        if (Glitches.Length >0)
        {
            Glitch = Glitches[0];
            return true;
        }

        return false;
    }
    public static IEnumerator GlitchDestroyed(GameObject Glitch, ParticleSystem particle)
    {
        particle.transform.SetParent(null);
        particle.gameObject.SetActive(true);
        particle.Play();
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(instance.GlitchRespawnTime);
    }
    public void RespawnGlitch(GameObject glitch, float delay, GameObject DataBytes, float radius)
    {
        StartCoroutine(Respawnglitch(glitch, delay, DataBytes, radius));
    }
    IEnumerator Respawnglitch(GameObject glitch, float delay, GameObject DataBytes, float radius)
    {
        Databytes databyte;
        databyte = Instantiate(DataBytes, transform.position, Quaternion.identity).GetComponent<Databytes>();
        yield return new WaitForEndOfFrame();
        //databyte.randomPos = (Random.insideUnitCircle * radius) + (Vector2)transform.position;

        yield return new WaitForSeconds(delay);
        glitch.SetActive(true);
    }

}
[System.Serializable]
public class PlayerProgress
{
    [Header("-----Melee-------")]
    public Item Melee =  new Item();

    [Header("-----DataBall-------")]
    public Item DataBall = new Item();
    [Header("-----Command-------")]
    public Item Command = new Item();
    [Header("-----HuntingRifle-------")]
    public Item HuntingRifle = new Item();
    [Header("-----Shotgun-------")]
    public Item Shotgun = new Item();
    [Header("-----AssaultRifle-------")]
    public Item AssaultRifle = new Item();
    public PlayerProgress() { }
}
[System.Serializable]
public class Item
{
    public GameObject ItemPrefab;
    [HideInInspector]public GameObject ItemInstance;
    public bool isUnlocked;
    public WeaponUpgradeModifiers WeaponModifiers;
    public Item() { }
    public void InstantiatePrefab(Transform Hotbar)
    {
        ItemInstance = Object.Instantiate(ItemPrefab, Hotbar);
    }
}
[System.Serializable]
public class WeaponUpgradeModifiers
{
    public int CurrentTier = 1;
    public float fire_Rate = 1;
    public float Damage = 1;
    public float ReloadSpeed = 1;
    public float ClipAmountUpgradeModifier = 1;
    public float WeaponUpgradeCostModifier = 1;
}