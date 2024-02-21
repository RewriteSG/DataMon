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
        isReloading = false;
    }
    void Update()
    {
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
        switch (HotBarController.ItemsInHotbar[HotBarController.selectedItem].item)
        {
            case ItemHolding.DataBall:
                AmmoText.text = "DataBalls: " + DataBallLauncher.AmmoAmount;
                break;
            case ItemHolding.AssaultRifle:
                AmmoText.text = "Ammo: " + assaultRifle.CurrentClipAmount+ " / " + assaultRifle.ClipAmount;
                break;
            case ItemHolding.HuntingRifle:
                AmmoText.text = "Ammo: " + huntingRifle.CurrentClipAmount + " / " + huntingRifle.ClipAmount;
                break;
            case ItemHolding.Shotgun:
                AmmoText.text = "Ammo: " + shotgun.CurrentClipAmount + " / " + shotgun.ClipAmount;
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
                break;
            case ItemHolding.DataBall:

                SetWeaponModelActive(Fists_weapon, false);
                SetWeaponModelActive(DataBallLauncher, true);
                //SetWeaponModelActive(huntingRifle, false);
                //SetWeaponModelActive(shotgun, false);
                //SetWeaponModelActive(assaultRifle, false);
                break;
            case ItemHolding.HuntingRifle:
                SetWeaponModelActive(Fists_weapon, false);

                SetWeaponModelActive(DataBallLauncher, false);
                SetWeaponModelActive(huntingRifle, true);
                //SetWeaponModelActive(shotgun, false);
                //SetWeaponModelActive(assaultRifle, false);
                break;
            case ItemHolding.Shotgun:
                SetWeaponModelActive(Fists_weapon, false);

                SetWeaponModelActive(DataBallLauncher, false);
                SetWeaponModelActive(huntingRifle, false);
                SetWeaponModelActive(shotgun, true);
                //SetWeaponModelActive(assaultRifle, false);
                break;
            case ItemHolding.AssaultRifle:
                SetWeaponModelActive(Fists_weapon, false);

                SetWeaponModelActive(DataBallLauncher, false);
                SetWeaponModelActive(huntingRifle, false);
                SetWeaponModelActive(shotgun, false);
                SetWeaponModelActive(assaultRifle, true);
                break;
        }
    }
    void SetWeaponModelActive(WeaponType weapon, bool isActive)
    {
        weapon.Model.SetActive(isActive);
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
        yield return new WaitForSeconds(CurrentWepAmmo.reloadTime);
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
    BulletInstance bullet;
    public void Shoot_Shotgun()
    {
        if (shotgun.CurrentClipAmount > 0 && 
            timeToShootAnother >= GameManager.instance.ShotgunDelay * GameManager.instance.player_progress.Shotgun.WeaponModifiers.fire_Rate)
        {
            shotgun.CurrentClipAmount--;
            timeToShootAnother = 0;

            for (int i = 0; i < GameManager.instance.ShotgunPelletPerRnd; i++)
            {
                bullet = GameManager.instance.GetAvailableBullet();
                bullet.transform.SetPositionAndRotation(gunPoint.transform.position, gunPoint.transform.rotation);
                bullet.transform.Rotate(Vector3.forward * Random.Range(-25, 26));
                bullet.SetDamageAndSpeed(SGBullet.Damage * GameManager.instance.player_progress.Shotgun.WeaponModifiers.Damage, SGBullet.speed);
            }
        }
    }
    public void Shoot_HuntingRifle()
    {
        if (huntingRifle.CurrentClipAmount > 0 && 
            timeToShootAnother >= GameManager.instance.HuntingRifleDelay * GameManager.instance.player_progress.HuntingRifle.WeaponModifiers.fire_Rate)
        {
            huntingRifle.CurrentClipAmount--;
            timeToShootAnother = 0;

            bullet = GameManager.instance.GetAvailableBullet();
            bullet.transform.SetPositionAndRotation(gunPoint.transform.position, gunPoint.transform.rotation);
            bullet.SetDamageAndSpeed(HRBullet.Damage * GameManager.instance.player_progress.HuntingRifle.WeaponModifiers.Damage, HRBullet.speed);
            bullet.HRBulletCheckPath();
            bullet.isHR = true;
        }
    }
    public void Shoot_AssaultRifle()
    {
        if (assaultRifle.CurrentClipAmount > 0 
            && timeToShootAnother >= GameManager.instance.AssaultRifleDelay * GameManager.instance.player_progress.AssaultRifle.WeaponModifiers.fire_Rate)
        {
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
    public GameObject Model;
    public int AmmoAmount;
    public int ClipAmount = 2;
    public float reloadTime;
    public int CurrentClipAmount = 0;
}

