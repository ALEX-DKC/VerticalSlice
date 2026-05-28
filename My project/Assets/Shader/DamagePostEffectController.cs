using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamagePostEffectController : MonoBehaviour
{
    [Header("Post Effect Material")]
    public Material damagePostEffectMaterial;

    [Header("Effect Settings")]
    public float flashIntensity = 0.8f;
    public float fadeSpeed = 2.5f;

    private float currentIntensity = 0f;

    void Start()
    {
        if (damagePostEffectMaterial != null)
        {
            damagePostEffectMaterial.SetFloat("_Intensity", 0f);
        }
    }

    void Update()
    {
        if (damagePostEffectMaterial == null)
        {
            return;
        }

        currentIntensity = Mathf.MoveTowards(
            currentIntensity,
            0f,
            fadeSpeed * Time.deltaTime
        );

        damagePostEffectMaterial.SetFloat("_Intensity", currentIntensity);
    }

    public void TriggerDamageEffect()
    {
        currentIntensity = flashIntensity;

        if (damagePostEffectMaterial != null)
        {
            damagePostEffectMaterial.SetFloat("_Intensity", currentIntensity);
        }
    }
}
