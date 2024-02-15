﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInstance : MonoBehaviour
{
    public float speed;
    public float Damage;
    public float DestroyBulletAfterSecs = 3;
    float timer;
    public LayerMask datamonLayer;
    bool HRB_Hits;
    public bool isHR;
    // Start is called before the first frame update
    void Start()
    {
        SetName();
    }
    private void OnEnable()
    {
        HRB_Hits = false;

    }
    public void HRBulletCheckPath()
    {
        hit = Physics2D.Raycast(transform.position, transform.up, Vector2.Distance(transform.position,transform.position + transform.up * speed * Time.deltaTime),datamonLayer);
        if (hit.collider == null)
            return;
        if (hit.collider.gameObject.CompareTag("DataMon"))
        {
            transform.position = hit.point;
            HRB_Hits = true;
        }
    }
    void SetName()
    {
        gameObject.name = Damage.ToString();

    }
    RaycastHit2D hit;
    // Update is called once per frame
    void Update()
    {
        if (isHR)
            HRBulletCheckPath();
        if(!HRB_Hits)
        transform.position += transform.up * speed*Time.deltaTime;
        timer += Time.deltaTime;
        
        if (timer >= DestroyBulletAfterSecs)
        {
            gameObject.SetActive(false);
            
        }
    }
    private void OnDisable()
    {
        timer = 0;
        isHR = false;
        if(GameManager.instance.BulletsPool.TryGetValue(false, out List<BulletInstance> b))
        {
            b.Add(this);
        }
        
        
    }
    public void SetDamageAndSpeed(float Dmg, float Spd)
    {
        Damage = Dmg;
        speed = Spd;
        SetName();
    }
}
