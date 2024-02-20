using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchObject : MonoBehaviour
{
    public SpriteRenderer GlitchPNG;
    public ParticleSystem GlitchParticleSytem;
    public float GlitchHealth, MaxHealth;
    bool isShakingGlitch;
    public GameObject DataBytes;
    public float minRandomRespawn, maxRandomRespawn, radius;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnEnable()
    {
        isShakingGlitch = false;
        GlitchParticleSytem.transform.parent = transform;

        GlitchHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (GlitchHealth <= 0)
        {
            StartCoroutine(GameManager.GlitchDestroyed(gameObject,GlitchParticleSytem));
            gameObject.SetActive(false);
            GlitchHealth = MaxHealth;
            GameManager.instance.RespawnGlitch(gameObject, Random.Range(minRandomRespawn, maxRandomRespawn), DataBytes, radius);

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    public void DamageGlitch(float Damage)
    {
        GlitchHealth-= Damage;

        StartCoroutine(ShakeObject());
    }
    public void ShakeGlitch()
    {
        if (!isShakingGlitch)
            StartCoroutine(ShakeObject());
    }
    float random;
    Databytes databyte;
    IEnumerator ShakeObject()
    {
        isShakingGlitch = true;
        random = Random.Range(0, 1);
        yield return new WaitForEndOfFrame();
        GlitchParticleSytem.gameObject.SetActive(true);
        GlitchParticleSytem.Play();
        GlitchPNG.transform.localPosition = new Vector3(random == 1 ? 0.05f : -0.05f, 0, 0);
        yield return new WaitForSeconds(0.1f);
        GlitchPNG.transform.localPosition = new Vector3(random == 1 ? -0.05f : 0.05f, 0, 0);
        yield return new WaitForSeconds(0.1f);
        GlitchPNG.transform.localPosition = Vector3.zero;

        databyte = Instantiate(DataBytes, transform.position, Quaternion.identity).GetComponent<Databytes>();
        yield return new WaitForEndOfFrame();
        print(databyte.transform.position);
        databyte.randomPos = (Random.insideUnitCircle * radius) + (Vector2)transform.position;
        yield return new WaitForSeconds(1f);
        isShakingGlitch = false;
    }
}
