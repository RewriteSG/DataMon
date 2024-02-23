using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjects : MonoBehaviour
{
    public float DmgBasedOfStat = 0.1f;
    public float Damage;
    public float AttackRange;
    public float StartAttackDelay = 1;
    public float EndAttackAt = 3;
    public float Speed, RotationSpeed, MaxDistance, DashDamp;
    public IndividualDataMon.DataMon AttacksByEntity;
    public GameObject AttacksByEntityGameObject;
    public Animation ExplosionAnimation;
    public GameObject target;
    public SpriteRenderer SpritePNG;
    SpriteRenderer[] allSpritePNG;
    public ParticleSystem UnityParticleSystem;
    public delegate void UpdateAttackObject();
    public UpdateAttackObject updateAttack;
    public bool isMoving, isDashAttack, isMoveAtDistance, isSpinningSprite, isSpinningProjectile, isCollisionEffect, onEndAtSeconds, isScaledUp,
        isDotAttack, isFireBreath, isExplosion;
    Vector2 StartDestination,EndDestination;
    Vector3 smoothVelocity = Vector3.zero;

    ParticleSystem ParticleBeforeAttack;
    public Collider2D[] ALLcollider;

    // divide Model world Scale 0.1732548 /  with constant 0.7054937403162723f
    public const float ScaledOfDataMon = 0.7054937403162723f;
    //float tLerp;
    float timer = 0;
    public float timerToEnd;
    private void OnEnable()
    {

        if (isMoveAtDistance)
            isDashAttack = true;
        if (isMoving)
            updateAttack += MoveBySpeed;
        else
        if (isDashAttack)
        {
            updateAttack = DashAttack;
            EndDestination = (transform.up * MaxDistance) + transform.position;
            //tLerp = 0;
        }

        if (isSpinningSprite)
            updateAttack += SpinByRotatingSpeed;

        if (isCollisionEffect)
        {
            updateAttack += CollisionEffectPlay;
            StartDestination = transform.position;
        }
        if (UnityParticleSystem != null && isCollisionEffect)
        {
            UnityParticleSystem.transform.SetParent(null);

            UnityParticleSystem.gameObject.SetActive(false);
        }
        if (UnityParticleSystem != null && isFireBreath)
        {
            UnityParticleSystem.transform.parent.SetParent(null);
            UnityParticleSystem.gameObject.SetActive(true);
            UnityParticleSystem.loop = true;

        }
        if (SpritePNG != null)
            SpritePNG.gameObject.SetActive(true);

        if (SpritePNG != null)
            SpritePNG.gameObject.SetActive(true);

        if (isScaledUp)
        {
            
            updateAttack += IncreaseScale;
        }
        if (isExplosion)
        {
            ExplosionAnimation.gameObject.SetActive(false);
        }
        //delayAfterAttack = EndAttackAt;
        if (AttacksByEntity == null)
            return;

        timerToEnd = EndAttackAt;

        ALLcollider = transform.GetComponentsInChildren<Collider2D>();
        timer = StartAttackDelay;

        if(GameManager.instance.GetParticleFromPool(StartAttackDelay,out ParticleSystem particleSystem))
        {
            ParticleBeforeAttack = particleSystem;
            ParticleBeforeAttack.gameObject.SetActive(true);
            ParticleBeforeAttack.Play();
            
            //ParticleBeforeAttack.loop = true;
        }

        for (int i = 0; i < ALLcollider.Length; i++)
        {
            ALLcollider[i].enabled = false;
        }



        gameObject.GetComponent<Collider2D>().enabled = false;
        AttacksByEntity.dataMonAI.AttackLaunched = false;
        allSpritePNG = transform.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < allSpritePNG.Length; i++)
        {
            allSpritePNG[i].color = new Color(allSpritePNG[i].color.r, allSpritePNG[i].color.b, allSpritePNG[i].color.g, 0);
        }
    }

    bool once;
    //float delayAfterAttack;
    private void Update()
    {
        if(timer >= 0 && AttacksByEntityGameObject != GameManager.instance.Player)
        {
            once = false;
            timer -= Time.deltaTime;
            if(ParticleBeforeAttack != null)
            ParticleBeforeAttack.transform.position = transform.position;
            transform.parent = AttacksByEntity == null ? AttacksByEntityGameObject.transform : AttacksByEntity.transform;



            allSpritePNG = transform.GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < allSpritePNG.Length; i++)
            {
                allSpritePNG[i].color = new Color(allSpritePNG[i].color.r, allSpritePNG[i].color.b, allSpritePNG[i].color.g,
                    Mathf.Lerp(1, 0, timer/ StartAttackDelay));
            }

            EndDestination = (transform.up * MaxDistance) + transform.position;
            return;
        }
        if(AttacksByEntityGameObject != GameManager.instance.Player && AttacksByEntity != null && timer<0)
        {
            //UnityParticleSystem.loop = false;
            transform.parent = isFireBreath ? transform.parent : null;
            //print(AttacksByEntity.dataMonAI == null);
            if (!once)
            {
                for (int i = 0; i < ALLcollider.Length; i++)
                {
                    ALLcollider[i].enabled = true;
                }
                gameObject.GetComponent<Collider2D>().enabled = true;
                print("uh Wut");
                once = true;
            }


            AttacksByEntity.dataMonAI.AttackLaunched = true;
            ParticleBeforeAttack.transform.position = Vector3.up * 500;
        }

        //if (AttacksByEntity != null)
        //{
        //}
        if (AttacksByEntityGameObject != null && isDashAttack && !isMoveAtDistance)
            GameManager.instance.PlayerisDashing = AttacksByEntityGameObject == GameManager.instance.Player;
        if(isFireBreath && !UnityParticleSystem.isNull())
        {
            UnityParticleSystem.transform.parent.position = transform.position;
            UnityParticleSystem.transform.parent.rotation = transform.rotation;
        }
        //if (AttacksByEntityGameObject == GameManager.instance.Player)
        //    UnityParticleSystem. &= ~(1 << GameManager.instance.PlayerLayer);
        //if (isFireBreath)
        //{
        //    UnityParticleSystem.transform.position = transform.position;
        //    UnityParticleSystem.transform.rotation = transform.rotation;

        //}

        if ((isMoving || isDashAttack || isScaledUp))
            updateAttack();
        else if((isMoving || isDashAttack || isScaledUp))
        {
            updateAttack();
        }
        if (isExplosion)
        {

            if (timerToEnd <= 0 && onEndAtSeconds)
            {
                ExplosionPlay();
            }
            else
                timerToEnd -= Time.deltaTime;

            return;
        }

        if (timerToEnd <= 0 && onEndAtSeconds)
        {
            AttackFinished();
        }
        else
            timerToEnd -= Time.deltaTime;

    }
    private void OnDisable()
    {
        updateAttack -= MoveBySpeed;
    }
    //public void OwnAttack(GameObject byDataMon)
    //{
    //    AttacksByEntity = byDataMon;
    //}
    public void OnCollided()
    {
        CollisionEffectPlay(true);
    }
    public void AttackFinished()
    {
        gameObject.SetActive(false);
        if (AttacksByEntity == null)
        {
            print("why udestroy this?");
            Destroy(gameObject);
        }
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
        
        if (AttacksByEntity != null && !isMoveAtDistance)
            AttacksByEntity.transform.position = transform.position;
        else
        if(AttacksByEntityGameObject != null && !isMoveAtDistance)
        {
            AttacksByEntityGameObject.transform.position = transform.position;
        }

        if (Vector2.Distance(transform.position, EndDestination) < 0.1f && !onEndAtSeconds )
        {

            if (AttacksByEntityGameObject != null && isDashAttack && !isMoveAtDistance)
                GameManager.instance.PlayerisDashing = false;
            if (AttacksByEntity == null)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);
        }

    }
    void SpinByRotatingSpeed()
    {
        SpritePNG.transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime);
    }
    void ExplosionPlay()
    {
        ExplosionAnimation.gameObject.SetActive(true);
        StartCoroutine(DestroyGameObjectAtDelay(ExplosionAnimation.clip.length));
        if (!ExplosionAnimation.isPlaying)
        {
            transform.position = Vector3.up * 500;
            ExplosionAnimation.transform.SetParent(null);
            ExplosionAnimation.Play();
        }
    }
    IEnumerator DestroyGameObjectAtDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        if (isExplosion)
        Destroy(ExplosionAnimation);
    }
    void CollisionEffectPlay()
    {
        
        if (UnityParticleSystem != null)
        {
            if (isSpinningSprite)
                SpritePNG.gameObject.SetActive(true);
            UnityParticleSystem.gameObject.SetActive(true);
            UnityParticleSystem.transform.position = transform.position;
            //UnityParticleSystem.Play();
            //gameObject.SetActive(false);
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
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
    private void OnDestroy()
    {
        if (isFireBreath && UnityParticleSystem != null)
        {
            Transform parent = UnityParticleSystem.transform.parent;
            UnityParticleSystem.transform.parent = null;
            UnityParticleSystem.loop = false;
            if (!parent.isNull())
                Destroy(parent.gameObject);
            //UnityParticleSystem.Stop();

        }else
        if (UnityParticleSystem != null)
            Destroy(UnityParticleSystem.gameObject);
    }
    private void IncreaseScale()
    {
        
        transform.localScale += Vector3.one * Speed * Time.deltaTime;
    }
}
