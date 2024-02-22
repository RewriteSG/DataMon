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

    [Header("Important: Set this true if its in DataHub")]
    public bool InHub = false;

    [Header("Player Variables")]

    public GameObject Player;
    public Canvas canvas;
    public HotBarController hotBarController;
    public int NumberOfDataMonsInTeam = 1;
    [HideInInspector] public PlayerShoot playerShootScript;
    [HideInInspector] public Rigidbody2D playerRb;
    public List<GameObject> HostileDataMonsGOs = new List<GameObject>();
    [SerializeField] public float MovementSpeed = 6;
    public float PlayerDataMonPatrolMinDist;
    public float PlayerDataMonPatrolMaxDist;
    public float DataMonSpawnRadiusFromPlayer, DataMonEnableRbInRadius;
    public float MaxDistForCompanionDataMon;
    public float CaptureDelay = 1;
    public bool PlayerisRiding, PlayerisDashing;
    public GameObject ridingDataMonAttackPoint, DataMonMount, DataMonHotBar;

    public DataMonHolder[] DataTeam = new DataMonHolder[] { };
    public List<AbilitiesScriptableObjects> DataMonAbilities = new List<AbilitiesScriptableObjects>();

    [Header("====================")]

    public float DataMonsTargetingRotationSpeed = 40;
    public float DataMonsRotationSpeed;
    public float DataMonInDataDexHPRegen = 2;
    public int MaxNumberOfWildDataMons = 15;
    [Header("WorldBorders")]
    public int DataWorldBorderLeftX,DataWorldBorderRightX, DataWorldBorderDownY, DataWorldBorderUpY;
    public float GlitchRespawnTime = 45;

    public int Databytes = 0;
    public ParticleSystem ParticlesBeforeEveryAttack;
    public List<ParticleSystem> ParticlesBeforeEveryAttackPool = new List<ParticleSystem>();
    public GameObject RenderDistanceTrigger;
    public float RenderDistance = 10f;
    public Color NeutralColor, HostileColor, CompanionColor;
    public LayerMask GlitchLayerMask, PlayerLayer, DataMonLayer;
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

    public delegate void PassivesAbilitiesEffects(float modifier);

    public PassivesAbilitiesEffects onCollisionPassive;
    public PassivesAbilitiesEffects onUpdatePassive;
    public PassivesAbilitiesEffects onAttackEffectPassive;
    public PassivesAbilitiesEffects onStartPassive;

    public bool isInteractingNPC;

    private void Awake()
    {
        instance = this;
        TotalDataMonIDs = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        if (SaveLoadManager.instance.DoLoadWorld)
        {
            Databytes = SaveLoadManager.LoadDataBytes();
            player_progress = SaveLoadManager.LoadPlayerProgress();
            WeaponType[] weaponTypes = SaveLoadManager.LoadWeaponTypes();
            for (int i = 0; i < weaponTypes.Length; i++)
            {
                print(weaponTypes[i].type);
                if (weaponTypes[i].Model.isNull())
                    continue;
                if (InHub)
                    weaponTypes[i].ModelInstance = Instantiate(weaponTypes[i].Model, Vector3.zero, Quaternion.identity);
                else
                    weaponTypes[i].ModelInstance = Instantiate(weaponTypes[i].Model, Player.transform);
                switch (weaponTypes[i].type)
                {
                    case WeaponType.Type.AssaultRifle:
                        assaultRifle = weaponTypes[i];
                        break;
                    case WeaponType.Type.Shotgun:
                        shotgun = weaponTypes[i];
                        break;
                    case WeaponType.Type.HuntingRifle:
                        huntingRifle = weaponTypes[i];
                        break;
                    case WeaponType.Type.Databall:
                        DataBallLauncher = weaponTypes[i];
                        break;
                }
            }
        }
        else if(!InHub)
        {
            assaultRifle.ModelInstance = Instantiate(assaultRifle.Model, Player.transform);
            shotgun.ModelInstance = Instantiate(shotgun.Model, Player.transform);
            huntingRifle.ModelInstance = Instantiate(huntingRifle.Model, Player.transform);
            print(DataBallLauncher.Model.name);
            DataBallLauncher.ModelInstance = Instantiate(DataBallLauncher.Model, Player.transform);
        }

        if (Player == null || InHub)
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
#if UNITY_EDITOR
        ReferencePlayerRenderDistTrigger();
#endif
        HostileDataMonsGOs.Clear();
        DataMon_StartAI = StartAI;

        DataTeam = SaveLoadManager.LoadDataTeamFromSave();

        for (int i = 0; i < DataTeam.Length; i++)
        {
            DataMonAbilities.Add(DataTeam[i].dataMonData.Ability);
        }

        //try
        //{
        
        //}
        //catch (System.NullReferenceException)
        //{
        //    
        Fists_weapon.ModelInstance = Fists_weapon.Model;
        //}
        for (int i = 0; i < 20; i++)
        {
            ParticlesBeforeEveryAttackPool.Add(Instantiate(ParticlesBeforeEveryAttack.gameObject, Vector3.up * 500, Quaternion.identity).GetComponent<ParticleSystem>());
        }
        isInteractingNPC = false;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Player == null || InHub)
            return;
        ReferencePlayerRenderDistTrigger();
        if (RenderDistanceTrigger == null)
            return;
        RenderDistanceTrigger.transform.localScale = Player.transform.InverseTransformVector(Vector3.one * RenderDistance);
    }
#endif

#if UNITY_EDITOR
    private void ReferencePlayerRenderDistTrigger()
    {
        if (InHub)
            return;
        if (RenderDistanceTrigger == null || RenderDistanceTrigger.transform.parent != Player.transform)
        {
            RenderDistanceTrigger = Player.transform.FindChildByTag("PlayerRenderDist").gameObject;

        }
    }

#endif
    // Update is called once per frame
    void Update()
    {
        if (InHub)
            return;
        NumberOfDataMonsInTeam = Mathf.Clamp(NumberOfDataMonsInTeam, 1, NumberOfDataMonsInTeam+1);
       

        if (Entity_Updates != null)
            Entity_Updates();
    }
    private void FixedUpdate()
    {
        if (InHub)
            return;
            if (Entity_FixedUpdates != null)
            Entity_FixedUpdates();
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (InHub)
            return;
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
#endif
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
    public bool GetParticleFromPool(float delay, out ParticleSystem ps)
    {
        ps = null;
        if (ParticlesBeforeEveryAttackPool.Count == 0)
            return false;
        ps = ParticlesBeforeEveryAttackPool[0];
        StartCoroutine(SetParticleToPool(ps, delay));
        ParticlesBeforeEveryAttackPool.RemoveAt(0);
        return true;
    }
    IEnumerator SetParticleToPool(ParticleSystem ps, float delay)
    {
        yield return new WaitForSeconds(delay);
        ps.gameObject.transform.position = Vector3.up * 500;
        ParticlesBeforeEveryAttackPool.Add(ps);
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
    //[Header("-----Command-------")]
    //public Item Command = new Item();
    [Header("-----HuntingRifle-------")]
    public Item HuntingRifle = new Item();
    [Header("-----Shotgun-------")]
    public Item Shotgun = new Item();
    [Header("-----AssaultRifle-------")]
    public Item AssaultRifle = new Item();
    public PlayerProgress() 
    {
        Melee = new Item(); DataBall = new Item(); HuntingRifle = new Item(); Shotgun = new Item(); AssaultRifle = new Item();
    }
    public PlayerProgress(Item _Melee, Item _DataBall, Item _HuntingRifle, Item _Shotgun,Item _AssaultRifle)
    {
        Melee = _Melee;
        DataBall = _DataBall;
        HuntingRifle = _HuntingRifle;
        Shotgun = _Shotgun;
        AssaultRifle = _AssaultRifle;
    }
}
[System.Serializable]
public class Item
{
    public GameObject ItemPrefab;
    public string prefabName;
    //[HideInInspector]public GameObject ItemInstance;
    public bool isUnlocked;
    public WeaponUpgradeModifiers WeaponModifiers;
    public Item() 
    {
        isUnlocked = false;
        WeaponModifiers = new WeaponUpgradeModifiers();
    }
    //public void InstantiatePrefab(Transform Hotbar)
    //{
    //    ItemInstance = Object.Instantiate(ItemPrefab, Hotbar);
    //}
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
    public WeaponUpgradeModifiers()
    {
        CurrentTier = 1; fire_Rate = 1; Damage = 1; ReloadSpeed = 1; ClipAmountUpgradeModifier = 1; WeaponUpgradeCostModifier = 1;
    }
}