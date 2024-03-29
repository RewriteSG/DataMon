﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistAttack : MonoBehaviour
{
    public bool isFistProjectile = false;
    public GameObject LeftFist,RightFist;
    public PlayerShoot playerShoot;
    List<Animation> LeftFistAnimations = new List<Animation>(), RightFistAnimations = new List<Animation>();
    Animation thisAnimation;
    public Vector2 LeftFistDefaultPos, RightFistDefaultPos, LeftFistDefaultRot, RightFistDefaultRot;
    // Start is called before the first frame update
    void Start()
    {
        if (isFistProjectile)
        {
            thisAnimation = GetComponent<Animation>();
            return;
        }
        LeftFistAnimations = LeftFist.transform.GetComponentsInChildren<Animation>().ToList();
        RightFistAnimations = RightFist.transform.GetComponentsInChildren<Animation>().ToList();

        LeftFistDefaultPos = LeftFist.transform.localPosition;
        LeftFistDefaultRot = LeftFist.transform.localRotation.eulerAngles;
        RightFistDefaultPos = RightFist.transform.localPosition;
        RightFistDefaultRot = RightFist.transform.localRotation.eulerAngles;


    }

    // Update is called once per frame
    void Update()
    {

    }
    Animation availableAnimation;
    public void PlayLeftFistAttack()
    {
        StartCoroutine(SetFistsCollision());
        availableAnimation = GetAvailableAnimation(LeftFistAnimations);
        if (availableAnimation == null)
            return;
        availableAnimation.Play();

    }
    public void PlayRightFistAttack()
    {
        StartCoroutine(SetFistsCollision());
        availableAnimation = GetAvailableAnimation(RightFistAnimations);
        if (availableAnimation == null)
            return;
        availableAnimation.Play();
    }
    IEnumerator SetFistsCollision()
    {

        playerShoot.fistsCollision.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerShoot.fistsCollision.gameObject.SetActive(false);

    }
    public void FistProjectileFinished()
    {
        thisAnimation.Stop();
    }
    Animation GetAvailableAnimation(List<Animation> animations)
    {
        for (int i = 0; i < animations.Count; i++)
        {
            if (!animations[i].isPlaying)
                return animations[i];
        }
        return null;
    }
}
