using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject gunPoint;
    public GameObject DataBall;

    public BulletInstance SGBullet;
    public BulletInstance HRBullet;
    public BulletInstance ARBullet;
    public float _DestroyDataBallAtDelay =1;
    public static float DestroyBallAtDelay;
    float timeToShootAnother;
    // Update is called once per frame
    //GameObject spawnedDataBall;
    
    void Update()
    {
        timeToShootAnother += Time.deltaTime;
    }
    public void Shoot_Databall()
    {
        DestroyBallAtDelay = _DestroyDataBallAtDelay;
        Instantiate(DataBall, gunPoint.transform.position, gunPoint.transform.rotation);
    }
    BulletInstance bullet;
    public void Shoot_Shotgun()
    {
        if (timeToShootAnother >= GameManager.instance.ShotgunDelay)
        {
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
        if (timeToShootAnother >= GameManager.instance.HuntingRifleDelay)
        {
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
        if (timeToShootAnother >= GameManager.instance.AssaultRifleDelay)
        {
            timeToShootAnother = 0;
            bullet = GameManager.instance.GetAvailableBullet();
            bullet.transform.SetPositionAndRotation(gunPoint.transform.position, gunPoint.transform.rotation);
            bullet.SetDamageAndSpeed(ARBullet.Damage, ARBullet.speed);

        }
    }
}
