using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjects : MonoBehaviour
{
    public float DmgBasedOfStat = 0.1f;
    public GameObject AttacksByEntity;
    public SpriteRenderer SpritePNG;
    public ParticleSystem UnityParticleSystem;
    public delegate void UpdateAttackObject();
    public UpdateAttackObject updateAttack;
    public bool isMoving, isDashAttack, isSpinningSprite, isSpinningProjectile, isCollisionEffect;
    public float Speed, RotationSpeed, MaxDistance, DashDamp;
    Vector2 StartDestination,EndDestination;
    Vector3 smoothVelocity = Vector3.zero;

    public const float localScaledOfDataMon = 2.748926111503607f;
    float tLerp;
    private void OnEnable()
    {
        if (isMoving)
            updateAttack += MoveBySpeed;
        else
        if (isDashAttack)
        {
            updateAttack = DashAttack;
            EndDestination = (transform.up * MaxDistance) + transform.position;
            tLerp = 0;
        }
        if (isSpinningSprite)
            updateAttack += SpinByRotatingSpeed;
        if (isCollisionEffect)
        {
            updateAttack += CollisionEffectPlay;
            StartDestination = transform.position;
        }
        if(UnityParticleSystem != null)
        {
            if (UnityParticleSystem.transform.parent != null)
            {

                UnityParticleSystem.transform.SetParent(null);
            }
            UnityParticleSystem.gameObject.SetActive(false);
        }
        if (SpritePNG != null)
            SpritePNG.gameObject.SetActive(true);
    }
    private void Update()
    {

        if (isMoving || isDashAttack)
            updateAttack();
    }
    private void OnDisable()
    {
        updateAttack -= MoveBySpeed;
    }
    public void OwnAttack(GameObject byDataMon)
    {
        AttacksByEntity = byDataMon;
    }
    public void OnCollided()
    {
        CollisionEffectPlay(true);
    }
    public void AttackFinished()
    {
        gameObject.SetActive(false);
        if (AttacksByEntity == null)
            Destroy(gameObject);
    }
    void MoveBySpeed()
    {
        transform.position += transform.up * Speed * Time.deltaTime;
    }
    void DashAttack()
    {
        
        //transform.position = Vector2.Lerp(StartDestination,EndDestination,tLerp);
        transform.position = Vector3.SmoothDamp(transform.position, EndDestination, ref smoothVelocity, DashDamp);

        //tLerp += Speed * Time.deltaTime;
        AttacksByEntity.transform.position = transform.position;
    }
    void SpinByRotatingSpeed()
    {
        SpritePNG.transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime);
    }
    void CollisionEffectPlay()
    {
        
        if (UnityParticleSystem != null && Vector3.Distance(transform.position, StartDestination) > MaxDistance)
        {
            if (isSpinningSprite)
                SpritePNG.gameObject.SetActive(false);
            UnityParticleSystem.gameObject.SetActive(true);
            UnityParticleSystem.transform.position = transform.position;
            //UnityParticleSystem.Play();
                gameObject.SetActive(false);
        }
    }
    void CollisionEffectPlay(bool ignore)
    {
        if(UnityParticleSystem != null)
        {
            if (isSpinningSprite)
                SpritePNG.gameObject.SetActive(false);
            else
                gameObject.SetActive(false);
            UnityParticleSystem.transform.position = transform.position;
            UnityParticleSystem.Play();

        }
    }
}
