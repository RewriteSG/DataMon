using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerShoot : MonoBehaviour
{
    public GameObject gunPoint;
    public GameObject DataBall;

    public int ClipAmount = 2;
    public int CurrentClipAmount = 0;
    public int dataBallAmmo = 3; 
    public int shotgunAmmo = 3; 
    public int huntingRifleAmmo = 3; 
    public int assaultRifleAmmo = 3;
    public BulletInstance SGBullet;
    public BulletInstance HRBullet;
    public BulletInstance ARBullet;
    public float _DestroyDataBallAtDelay =1;
    public static float DestroyBallAtDelay;
    float timeToShootAnother;
    public TMP_Text dataBallAmmoText;
    public TMP_Text shotgunAmmoText;
    public TMP_Text huntingRifleAmmoText;
    public TMP_Text assaultRifleAmmoText;

    void Update()
    {
        timeToShootAnother += Time.deltaTime;
        UpdateAmmoUI();
        
        if(Input.GetKeyDown(KeyCode.R))
        {
            Reload(ref shotgunAmmo);
        }
    }
    void UpdateAmmoUI()
    {
        dataBallAmmoText.text = "DataBall Ammo: " + dataBallAmmo.ToString();
        shotgunAmmoText.text = "Shotgun Ammo: " + shotgunAmmo.ToString();
        huntingRifleAmmoText.text = "Hunting Rifle Ammo: " + huntingRifleAmmo.ToString();
        assaultRifleAmmoText.text = "Assault Rifle Ammo: " + assaultRifleAmmo.ToString();
    }

    void Reload(ref int CurrentAmmo)
    {
        if(CurrentClipAmount != 0)
        {
            int diff = ClipAmount - CurrentClipAmount;
            CurrentAmmo -= diff;
            CurrentClipAmount = ClipAmount;
            return;
        }

        if((CurrentAmmo-ClipAmount) <= 0)
        {
            CurrentClipAmount = CurrentAmmo;
        }
        else
        {
            CurrentClipAmount = ClipAmount;
        }
        CurrentAmmo -= CurrentClipAmount;

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
        if (CurrentClipAmount > 0 && timeToShootAnother >= GameManager.instance.ShotgunDelay)
        {
            CurrentClipAmount--;
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
        if (huntingRifleAmmo > 0 && timeToShootAnother >= GameManager.instance.HuntingRifleDelay)
        {
            huntingRifleAmmo--;
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
        if (assaultRifleAmmo > 0 && timeToShootAnother >= GameManager.instance.AssaultRifleDelay)
        {
            assaultRifleAmmo--;
            timeToShootAnother = 0;

            bullet = GameManager.instance.GetAvailableBullet();
            bullet.transform.SetPositionAndRotation(gunPoint.transform.position, gunPoint.transform.rotation);
            bullet.SetDamageAndSpeed(ARBullet.Damage, ARBullet.speed);
        }
    }
}

