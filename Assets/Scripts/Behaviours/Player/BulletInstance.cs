using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInstance : MonoBehaviour
{
    public float speed;
    public float Damage;
    public float DestroyBulletAfterSecs = 3;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        SetName();
    }
    void SetName()
    {
        gameObject.name = Damage.ToString();

    }
    // Update is called once per frame
    void Update()
    {
        transform.position += (transform.up * speed*Time.deltaTime);
        timer += Time.deltaTime;
        if (timer >= DestroyBulletAfterSecs)
        {
            timer = 0;
            GameManager.instance.BulletsPool.TryGetValue(false, out List<BulletInstance> b);
            b.Add(this);
            gameObject.SetActive(false);
        }
    }
    public void SetDamageAndSpeed(float Dmg, float Spd)
    {
        Damage = Dmg;
        speed = Spd;
        SetName();
    }
}
