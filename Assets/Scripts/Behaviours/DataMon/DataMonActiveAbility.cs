using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMonActiveAbility : MonoBehaviour
{

    //public UnityEngine.UI.RawImage HotbarItem;

    //public int selectedItem;
    //public int prevSelectedItem;

    public KeyCode AbilityKeyInput;
    public bool GetKeyDown = true;

    public AbilitiesScriptableObjects ability;
    public AbilityState abilityState;
    public int EvolutionTier;
    public DataMonIndividualData individualData;
    public DataMonsData dataMonsData;
    public float Damage;
    [SerializeField]float activeCooldown, activeTimer, activeFuel;
    public Transform GunPoint;

    MountAbility mountAbility;
    GameObject spawnedObject;
    bool isMountAbility, usingAbility;
    float prevPlayerSpeed;
    public enum AbilityState
    {
        Activated, Ready, Cooldown
    }
    // Start is called before the first frame update
    void Start()
    {
        isMountAbility = ability is MountAbility;
        if (isMountAbility)
        {
            mountAbility = (MountAbility)ability;
        }
        activeFuel = ability.AbilityCooldown[EvolutionTier];
        abilityState = AbilityState.Ready;
    }
    private void OnEnable()
    {
        usingAbility = false;
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(AbilityKeyInput) && abilityState == AbilityState.Ready && GetKeyDown && !isMountAbility)
        {
            ability.UseAbility(dataMonsData, individualData, GameManager.instance, false, GunPoint);
            abilityState = AbilityState.Activated;
        }
        if (!isMountAbility)
            goto ContinueOn;

        if (Input.GetKeyDown(AbilityKeyInput) && GetKeyDown && mountAbility.typeOfAbility == MountAbility.TypeOfAbility.Active)
        {
            ability.UseAbility(dataMonsData, individualData, GameManager.instance, false, GunPoint);
            abilityState = AbilityState.Activated;
        }
        if (mountAbility.typeOfAbility == MountAbility.TypeOfAbility.Active)
            goto ContinueOn;

        FuelAttack();
        return;
        ContinueOn:
        CheckState();
    }

    private void FuelAttack()
    {
        if (Input.GetKeyDown(AbilityKeyInput) && GetKeyDown && activeFuel > mountAbility.FuelUseThreshold && mountAbility.typeOfAbility == MountAbility.TypeOfAbility.Fuel)
        {
            if (spawnedObject == null)
            {
                spawnedObject = ability.UseAbility(dataMonsData, individualData, GameManager.instance, false, GunPoint);

                
            }
            //else
            //    spawnedObject.SetActive(true);
            spawnedObject.transform.parent = GunPoint;
            activeCooldown = mountAbility.ChargeTime / ability.AbilityCooldown[0];
            usingAbility = true;
            prevPlayerSpeed = GameManager.instance.MovementSpeed;
            GameManager.instance.MovementSpeed = 2;
        }
        if (Input.GetKeyUp(AbilityKeyInput) && GetKeyDown && activeFuel > 0 && mountAbility.typeOfAbility == MountAbility.TypeOfAbility.Fuel)
        {
            usingAbility = false;
        }
        if (activeFuel <= 0)
            usingAbility = false;
        if(!spawnedObject.isNull() && !usingAbility)
        {
            GameManager.instance.MovementSpeed = prevPlayerSpeed;
            Destroy(spawnedObject);
        }
        if (!usingAbility)
        {
            activeFuel += activeCooldown * Time.deltaTime;
        }
        else
        {
            spawnedObject.transform.position = transform.position;
            spawnedObject.transform.rotation = transform.rotation;
            activeFuel -= activeCooldown * Time.deltaTime;
        }
    }

    private void CheckState()
    {
        switch (abilityState)
        {
            case AbilityState.Activated:

                activeCooldown = ability.AbilityCooldown[EvolutionTier];

                abilityState = AbilityState.Cooldown;
                break;
            case AbilityState.Ready:
                break;
            case AbilityState.Cooldown:

                if (activeCooldown <= 0)
                {
                    abilityState = AbilityState.Ready;
                }
                else
                {
                    activeCooldown -= Time.deltaTime;
                }
                break;
        }
    }
}
