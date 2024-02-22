using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerShoot : MonoBehaviour
{

    public Image ReloadingImage;

    public GameObject gunPoint;
    public GameObject DataBall;

    
    public Animation Fists;
    
    public BulletInstance SGBullet;
    public BulletInstance HRBullet;
    public BulletInstance ARBullet;
    public float _DestroyDataBallAtDelay =1;
    public static float DestroyBallAtDelay;

     WeaponType Fists_weapon = new WeaponType();
     WeaponType DataBallLauncher = new WeaponType();
     WeaponType huntingRifle = new WeaponType();
     WeaponType shotgun = new WeaponType();
     WeaponType assaultRifle = new WeaponType();

    public TMP_Text AmmoText;

    public float _FistsDamage;
    public BulletInstance fistsCollision;

    float timer, ReloadTime;
    bool isReloading;
    FistAttack fistsAttack;
    float timeToShootAnother;

    float prevPlayerMoveSpeed;
    private void Start()
    {
        fistsAttack = Fists.GetComponent<FistAttack>();
        fistsCollision = Fists.transform.GetComponentInChildren<BulletInstance>();
        Fists_weapon = GameManager.instance.Fists_weapon;
        DataBallLauncher = GameManager.instance.DataBallLauncher;
        huntingRifle = GameManager.instance.huntingRifle;
        shotgun = GameManager.instance.shotgun;
        assaultRifle = GameManager.instance.assaultRifle;
        fistsCollision.gameObject.SetActive(false);
        isReloading = false; prevPlayerMoveSpeed = GameManager.instance.MovementSpeed;
    }
    void Update()
    {

        Vector3 mousePosition = Input.mousePosition;

        RectTransform ammoTextRectTransform = AmmoText.GetComponent<RectTransform>();
        ammoTextRectTransform.position = mousePosition + new Vector3(-90, -20, 0);
        timeToShootAnother += Time.deltaTime;

        UpdateAmmoUI();
        ClampAmmoClip(shotgun);
        ClampAmmoClip(huntingRifle);
        ClampAmmoClip(assaultRifle);

        if (Input.GetKeyDown(KeyCode.R))
        {
            switch (HotBarController.ItemsInHotbar[HotBarController.selectedItem].item)
            {
                case ItemHolding.AssaultRifle:
                    Reload(ref assaultRifle);

                    break;
                case ItemHolding.Shotgun:
                    Reload(ref shotgun);

                    break;
                case ItemHolding.HuntingRifle:

                    Reload(ref huntingRifle);
                    break;
            }
        }
        if (isReloading)
        {
            ReloadingImage.fillAmount = 1 - timer / ReloadTime;
            timer += Time.deltaTime;
        }
        else
        {
            ReloadingImage.fillAmount = 0;
        }
    }

    private void ClampAmmoClip(WeaponType wepAmmo)
    {
        wepAmmo.CurrentClipAmount = Mathf.Clamp(wepAmmo.CurrentClipAmount, 0, wepAmmo.ClipAmount);
    }

    void UpdateAmmoUI()
    {
        if (HotBarController.ItemsInHotbar[HotBarController.selectedItem] == null)
            return;

        switch (HotBarController.ItemsInHotbar[HotBarController.selectedItem].item)
        {
            case ItemHolding.DataBall:
                AmmoText.text = "DataBalls: " + DataBallLauncher.AmmoAmount;
                break;
            case ItemHolding.AssaultRifle:
                AmmoText.text = "Ammo: " + assaultRifle.CurrentClipAmount+ " / " + assaultRifle.AmmoAmount;
                break;
            case ItemHolding.HuntingRifle:
                AmmoText.text = "Ammo: " + huntingRifle.CurrentClipAmount + " / " + huntingRifle.AmmoAmount;
                break;
            case ItemHolding.Shotgun:
                AmmoText.text = "Ammo: " + shotgun.CurrentClipAmount + " / " + shotgun.AmmoAmount;
                break;
            case ItemHolding.Melee:

                AmmoText.text = "";
                break;
        }
    }
    public void ShowWeaponModel(ItemHolding itemType)
    {
        switch (itemType)
        {
            case ItemHolding.Melee:
                SetWeaponModelActive(Fists_weapon, true);
                SetWeaponModelActive(DataBallLauncher, false);
                SetWeaponModelActive(huntingRifle, false);
                SetWeaponModelActive(shotgun, false);
                SetWeaponModelActive(assaultRifle, false);
                //gunPoint = Fists_weapon.ModelInstance.transform.Find("GunPoint").gameObject;
                break;
            case ItemHolding.DataBall:

                SetWeaponModelActive(Fists_weapon, false);
                SetWeaponModelActive(huntingRifle, false);
                SetWeaponModelActive(shotgun, false);
                SetWeaponModelActive(assaultRifle, false);
                SetWeaponModelActive(DataBallLauncher, true);
                gunPoint = DataBallLauncher.ModelInstance.transform.Find("GunPoint").gameObject;
                break;
            case ItemHolding.HuntingRifle:
                SetWeaponModelActive(Fists_weapon, false);

                SetWeaponModelActive(DataBallLauncher, false);
                SetWeaponModelActive(shotgun, false);
                SetWeaponModelActive(assaultRifle, false);
                SetWeaponModelActive(huntingRifle, true);
                gunPoint = huntingRifle.ModelInstance.transform.Find("GunPoint").gameObject;
                break;
            case ItemHolding.Shotgun:
                SetWeaponModelActive(Fists_weapon, false);

                SetWeaponModelActive(DataBallLauncher, false);
                SetWeaponModelActive(huntingRifle, false);
                SetWeaponModelActive(assaultRifle, false);
                SetWeaponModelActive(shotgun, true);
                gunPoint = shotgun.ModelInstance.transform.Find("GunPoint").gameObject;
                break;
            case ItemHolding.AssaultRifle:
                SetWeaponModelActive(Fists_weapon, false);

                SetWeaponModelActive(DataBallLauncher, false);
                SetWeaponModelActive(huntingRifle, false);
                SetWeaponModelActive(shotgun, false);
                SetWeaponModelActive(assaultRifle, true);
                gunPoint = assaultRifle.ModelInstance.transform.Find("GunPoint").gameObject;
                break;
            case ItemHolding.None:

                SetWeaponModelActive(Fists_weapon, false);

                SetWeaponModelActive(DataBallLauncher, false);
                SetWeaponModelActive(huntingRifle, false);
                SetWeaponModelActive(shotgun, false);
                SetWeaponModelActive(assaultRifle, false);
                break;
        }
    }
    void SetWeaponModelActive(WeaponType weapon, bool isActive)
    {
        weapon.ModelInstance.SetActive(isActive);
    }
    void Reload(ref WeaponType CurrentWepAmmo)
    {
        StartCoroutine(ReloadCoroutine(CurrentWepAmmo));
    }
    IEnumerator ReloadCoroutine(WeaponType CurrentWepAmmo)
    {

        if (CurrentWepAmmo.CurrentClipAmount == CurrentWepAmmo.ClipAmount || CurrentWepAmmo.AmmoAmount == 0)
            yield break;
        isReloading = true;
        timer = 0;
        ReloadTime = CurrentWepAmmo.reloadTime;
        GameManager.instance.MovementSpeed = 2;
        yield return new WaitForSeconds(CurrentWepAmmo.reloadTime);
        GameManager.instance.MovementSpeed = prevPlayerMoveSpeed;
        isReloading = false;
        if (CurrentWepAmmo.CurrentClipAmount != 0)
        {
            int diff = CurrentWepAmmo.ClipAmount - CurrentWepAmmo.CurrentClipAmount;
            if (diff >= CurrentWepAmmo.AmmoAmount)
            {
                CurrentWepAmmo.CurrentClipAmount += CurrentWepAmmo.AmmoAmount;
                CurrentWepAmmo.AmmoAmount -= CurrentWepAmmo.AmmoAmount;
                yield break;
            }
            CurrentWepAmmo.AmmoAmount -= diff;
            CurrentWepAmmo.CurrentClipAmount = CurrentWepAmmo.ClipAmount;
            yield break;


        }

        if ((CurrentWepAmmo.AmmoAmount - CurrentWepAmmo.ClipAmount) <= 0)
        {
            CurrentWepAmmo.CurrentClipAmount = CurrentWepAmmo.AmmoAmount;
        }
        else
        {
            CurrentWepAmmo.CurrentClipAmount = CurrentWepAmmo.ClipAmount;
        }
        CurrentWepAmmo.AmmoAmount -= CurrentWepAmmo.CurrentClipAmount;

    }
    public void Shoot_Melee()
    {
        fistsCollision.SetDamageAndSpeed(_FistsDamage * GameManager.instance.player_progress.Melee.WeaponModifiers.Damage, 0);

        if (!Fists.isPlaying)
        {
            Fists.Play();
        }
    }
    public void StopFistsAnimation()
    {
        Fists.Stop();
        SetFistPositionAndRotation(fistsAttack.LeftFist.transform,
            fistsAttack.LeftFistDefaultPos, Vector2.zero);
        SetFistPositionAndRotation(fistsAttack.RightFist.transform,
            fistsAttack.RightFistDefaultPos, Vector2.zero);
    }
    void SetFistPositionAndRotation(Transform fist, Vector2 pos, Vector2 rot)
    {
        fist.localPosition = pos;
        fist.localRotation = Quaternion.Euler(rot);
    }
    public void Shoot_Databall()
    {
        if (DataBallLauncher.AmmoAmount > 0)
        {
            DataBallLauncher.AmmoAmount--;
            DestroyBallAtDelay = _DestroyDataBallAtDelay;
            Instantiate(DataBall, gunPoint.transform.position, gunPoint.transform.rotation);
        }
    }
    public void StopShooting()
    {
        GameManager.instance.MovementSpeed = prevPlayerMoveSpeed;
    }
    BulletInstance bullet;
    int RandomChance;
    public void Shoot_Shotgun()
    {
        RandomChance = 0;
        if (shotgun.CurrentClipAmount > 0 && 
            timeToShootAnother >= GameManager.instance.ShotgunDelay * GameManager.instance.player_progress.Shotgun.WeaponModifiers.fire_Rate)
        {
            if (GameManager.instance.isHamsterPassive)
                RandomChance = Random.Range(1, 4);
            if (RandomChance == 1 || RandomChance == 2)
                shotgun.CurrentClipAmount--;
            timeToShootAnother = 0;

            for (int i = 0; i < GameManager.instance.ShotgunPelletPerRnd; i++)
            {
                bullet = GameManager.instance.GetAvailableBullet();
                bullet.transform.SetPositionAndRotation(gunPoint.transform.position, gunPoint.transform.rotation);
                bullet.transform.Rotate(Vector3.forward * Random.Range(-25, 26));
                bullet.SetDamageAndSpeed(SGBullet.Damage * GameManager.instance.player_progress.Shotgun.WeaponModifiers.Damage, SGBullet.speed);
            }
            GameManager.instance.MovementSpeed = 2;
        }
    }
    public void Shoot_HuntingRifle()
    {
        RandomChance = 0;
        if (huntingRifle.CurrentClipAmount > 0 && 
            timeToShootAnother >= GameManager.instance.HuntingRifleDelay * GameManager.instance.player_progress.HuntingRifle.WeaponModifiers.fire_Rate)
        {
            if (GameManager.instance.isHamsterPassive)
                RandomChance = Random.Range(1, 4);
            if (RandomChance == 1 || RandomChance == 2)
                huntingRifle.CurrentClipAmount--;
            timeToShootAnother = 0;

            bullet = GameManager.instance.GetAvailableBullet();
            bullet.transform.SetPositionAndRotation(gunPoint.transform.position, gunPoint.transform.rotation);
            bullet.SetDamageAndSpeed(HRBullet.Damage * GameManager.instance.player_progress.HuntingRifle.WeaponModifiers.Damage, HRBullet.speed);
            bullet.HRBulletCheckPath();
            bullet.isHR = true;
            GameManager.instance.MovementSpeed = 2;
        }
    }
    public void Shoot_AssaultRifle()
    {
        RandomChance = 0;
        if (assaultRifle.CurrentClipAmount > 0 
            && timeToShootAnother >= GameManager.instance.AssaultRifleDelay * GameManager.instance.player_progress.AssaultRifle.WeaponModifiers.fire_Rate)
        {
            if (GameManager.instance.isHamsterPassive)
                RandomChance = Random.Range(1, 4);
            if (RandomChance == 1 || RandomChance == 2)
                assaultRifle.CurrentClipAmount--;
            timeToShootAnother = 0;

            bullet = GameManager.instance.GetAvailableBullet();
            bullet.transform.SetPositionAndRotation(gunPoint.transform.position, gunPoint.transform.rotation);
            bullet.SetDamageAndSpeed(ARBullet.Damage * GameManager.instance.player_progress.Shotgun.WeaponModifiers.Damage, ARBullet.speed);
        }
    }
}
[System.Serializable]
public class WeaponType
{
    public enum Type
    {
        AssaultRifle, Shotgun, HuntingRifle, Databall, Fists
    }
    public Type type;
    public GameObject Model;
    public string ModelName;
    public GameObject ModelInstance;
    public int AmmoAmount;
    public int ClipAmount = 2;
    public float reloadTime;
    public int CurrentClipAmount = 0;
}

