using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerShoot : MonoBehaviour
{
    public GameObject gunPoint;
    public GameObject DataBall;

    public int dataBallAmmo = 3; 
    
    public WeaponAmmo shotgunAmmo = new WeaponAmmo(); 
    public WeaponAmmo huntingRifleAmmo = new WeaponAmmo(); 
    public WeaponAmmo assaultRifleAmmo = new WeaponAmmo();
    public BulletInstance SGBullet;
    public BulletInstance HRBullet;
    public BulletInstance ARBullet;
    public float _DestroyDataBallAtDelay =1;
    public static float DestroyBallAtDelay;
    float timeToShootAnother;
    public TMP_Text AmmoText;

    void Update()
    {
        timeToShootAnother += Time.deltaTime;
        UpdateAmmoUI();
        ClampAmmoClip(shotgunAmmo);
        ClampAmmoClip(huntingRifleAmmo);
        ClampAmmoClip(assaultRifleAmmo);

        if (Input.GetKeyDown(KeyCode.R))
        {
            switch (HotBarController.holdingItem)
            {
                case ItemHolding.AssaultRifle:
                    Reload(ref assaultRifleAmmo);

                    break;
                case ItemHolding.Shotgun:
                    Reload(ref shotgunAmmo);

                    break;
                case ItemHolding.HuntingRifle:

                    Reload(ref huntingRifleAmmo);
                    break;
            }
        }
    }

    private void ClampAmmoClip(WeaponAmmo wepAmmo)
    {
        wepAmmo.CurrentClipAmount = Mathf.Clamp(wepAmmo.CurrentClipAmount, 0, wepAmmo.ClipAmount);
    }

    void UpdateAmmoUI()
    {
        switch (HotBarController.holdingItem)
        {
            case ItemHolding.DataBall:
                AmmoText.text = "DataBalls: " + dataBallAmmo;
                break;
            case ItemHolding.AssaultRifle:
                AmmoText.text = "Ammo: " + assaultRifleAmmo.CurrentClipAmount+ " / " + assaultRifleAmmo.ClipAmount;
                break;
            case ItemHolding.HuntingRifle:
                AmmoText.text = "Ammo: " + huntingRifleAmmo.CurrentClipAmount + " / " + huntingRifleAmmo.ClipAmount;
                break;
            case ItemHolding.Shotgun:
                AmmoText.text = "Ammo: " + shotgunAmmo.CurrentClipAmount + " / " + shotgunAmmo.ClipAmount;
                break;
        }
    }

    void Reload(ref WeaponAmmo CurrentWepAmmo)
    {
        if (CurrentWepAmmo.CurrentClipAmount == CurrentWepAmmo.ClipAmount || CurrentWepAmmo.AmmoAmount==0)
            return;
        if(CurrentWepAmmo.CurrentClipAmount != 0)
        {
            int diff = CurrentWepAmmo.ClipAmount - CurrentWepAmmo.CurrentClipAmount;
            if(diff>= CurrentWepAmmo.AmmoAmount)
            {
                CurrentWepAmmo.CurrentClipAmount += CurrentWepAmmo.AmmoAmount;
                CurrentWepAmmo.AmmoAmount -= CurrentWepAmmo.AmmoAmount;
                return;
            }
            CurrentWepAmmo.AmmoAmount -= diff;
            CurrentWepAmmo.CurrentClipAmount = CurrentWepAmmo.ClipAmount;
            return;


        }

        if((CurrentWepAmmo.AmmoAmount - CurrentWepAmmo.ClipAmount) <= 0)
        {
            CurrentWepAmmo.CurrentClipAmount = CurrentWepAmmo.AmmoAmount;
            print("is it doing this?");
        }
        else
        {
            CurrentWepAmmo.CurrentClipAmount = CurrentWepAmmo.ClipAmount;
            print("Or doing this?");

        }
        CurrentWepAmmo.AmmoAmount -= CurrentWepAmmo.CurrentClipAmount;

    }
    public void Shoot_Databall()
    {
        if (dataBallAmmo > 0)
        {
            dataBallAmmo--;
            DestroyBallAtDelay = _DestroyDataBallAtDelay;
            Instantiate(DataBall, gunPoint.transform.position, gunPoint.transform.rotation);
        }
    }
    BulletInstance bullet;
    public void Shoot_Shotgun()
    {
        if (shotgunAmmo.CurrentClipAmount > 0 && timeToShootAnother >= GameManager.instance.ShotgunDelay)
        {
            shotgunAmmo.CurrentClipAmount--;
            timeToShootAnother = 0;

            for (int i = 0; i < GameManager.instance.ShotgunPelletPerRnd; i++)
            {
                bullet = GameManager.instance.GetAvailableBullet();
                bullet.transform.SetPositionAndRotation(gunPoint.transform.position, gunPoint.transform.rotation);
                bullet.transform.Rotate(Vector3.forward * Random.Range(-25, 26));
                bullet.SetDamageAndSpeed(SGBullet.Damage, SGBullet.speed);
            }
        }
    }
    public void Shoot_HuntingRifle()
    {
        if (huntingRifleAmmo.CurrentClipAmount > 0 && timeToShootAnother >= GameManager.instance.HuntingRifleDelay)
        {
            huntingRifleAmmo.CurrentClipAmount--;
            timeToShootAnother = 0;

            bullet = GameManager.instance.GetAvailableBullet();
            bullet.transform.SetPositionAndRotation(gunPoint.transform.position, gunPoint.transform.rotation);
            bullet.SetDamageAndSpeed(HRBullet.Damage, HRBullet.speed);
            bullet.HRBulletCheckPath();
            bullet.isHR = true;
        }
    }
    public void Shoot_AssaultRifle()
    {
        if (assaultRifleAmmo.CurrentClipAmount > 0 && timeToShootAnother >= GameManager.instance.AssaultRifleDelay)
        {
            assaultRifleAmmo.CurrentClipAmount--;
            timeToShootAnother = 0;

            bullet = GameManager.instance.GetAvailableBullet();
            bullet.transform.SetPositionAndRotation(gunPoint.transform.position, gunPoint.transform.rotation);
            bullet.SetDamageAndSpeed(ARBullet.Damage, ARBullet.speed);
        }
    }
}
[System.Serializable]
public class WeaponAmmo
{
    public int AmmoAmount;
    public int ClipAmount = 2;
    public int CurrentClipAmount = 0;
}

