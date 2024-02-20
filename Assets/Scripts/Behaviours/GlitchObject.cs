using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchObject : MonoBehaviour
{
    public SpriteRenderer GlitchPNG;
    public ParticleSystem GlitchParticleSytem;
    public float GlitchHealth, MaxHealth;
    bool isShakingGlitch;
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
        }
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
        yield return new WaitForSeconds(1f);
        isShakingGlitch = false;
    }
}
