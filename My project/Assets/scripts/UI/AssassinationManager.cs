using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinationManager : MonoBehaviour
{
    [Header("References")]
    public InputManager inputManager;
    public AnimatorManager animatorManager;
    public GameObject promptUI;

    [Header("Assassination Settings")]
    public float assassinateRange = 3f;
    public float behindDotThreshold = -0.3f;
    public float killDelay = 0.8f;
    public float assassinateDamage = 999f;

    [Header("Assassination Sound")]
    public float assassinateSoundDelay = 0.15f;

    [Header("Debug")]
    public bool debugIsUnarmed;
    public bool debugFoundTarget;
    public bool debugIsBehind;
    public bool debugCanAssassinate;
    public string debugTargetName;

    private Guard currentGuard;
    private Guard123 currentGuard123;
    private Boss currentBoss;
    private MeleeGuard currentMeleeGuard;

    private bool isAssassinating = false;

    void Awake()
    {
        if (inputManager == null)
        {
            inputManager = GetComponent<InputManager>();
        }

        if (animatorManager == null)
        {
            animatorManager = GetComponent<AnimatorManager>();
        }

        if (promptUI != null)
        {
            promptUI.SetActive(false);
        }
    }

    void Update()
    {
        if (inputManager == null)
        {
            return;
        }

        if (isAssassinating)
        {
            if (promptUI != null)
            {
                promptUI.SetActive(false);
            }

            return;
        }

        FindAssassinationTarget();

        debugIsUnarmed = inputManager.IsUnarmedState();

        bool canAssassinate =
            inputManager.IsUnarmedState() &&
            HasLivingTarget();

        debugCanAssassinate = canAssassinate;

        if (promptUI != null)
        {
            promptUI.SetActive(canAssassinate);
        }

        if (inputManager.IsAssassinatePressed())
        {
            inputManager.ResetAssassinateInput();

            if (canAssassinate)
            {
                StartCoroutine(DoAssassination());
            }
        }
    }

    void FindAssassinationTarget()
    {
        currentGuard = null;
        currentGuard123 = null;
        currentBoss = null;
        currentMeleeGuard = null;

        debugFoundTarget = false;
        debugIsBehind = false;
        debugTargetName = "None";

        if (!inputManager.IsUnarmedState())
        {
            return;
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, assassinateRange);

        foreach (Collider hit in hits)
        {
            Guard guard = hit.GetComponentInParent<Guard>();

            if (guard != null)
            {
                debugFoundTarget = true;
                debugTargetName = guard.gameObject.name;

                bool behind = IsBehindTarget(guard.transform);
                debugIsBehind = behind;

                if (!guard.isDead && behind)
                {
                    currentGuard = guard;
                    return;
                }
            }

            Guard123 guard123 = hit.GetComponentInParent<Guard123>();

            if (guard123 != null)
            {
                debugFoundTarget = true;
                debugTargetName = guard123.gameObject.name;

                bool behind = IsBehindTarget(guard123.transform);
                debugIsBehind = behind;

                if (!guard123.isDead && behind)
                {
                    currentGuard123 = guard123;
                    return;
                }
            }

            Boss boss = hit.GetComponentInParent<Boss>();

            if (boss != null)
            {
                debugFoundTarget = true;
                debugTargetName = boss.gameObject.name;

                bool behind = IsBehindTarget(boss.transform);
                debugIsBehind = behind;

                if (!boss.isDead && behind)
                {
                    currentBoss = boss;
                    return;
                }
            }

            MeleeGuard meleeGuard = hit.GetComponentInParent<MeleeGuard>();

            if (meleeGuard != null)
            {
                debugFoundTarget = true;
                debugTargetName = meleeGuard.gameObject.name;

                bool behind = IsBehindTarget(meleeGuard.transform);
                debugIsBehind = behind;

                if (!meleeGuard.isDead && behind)
                {
                    currentMeleeGuard = meleeGuard;
                    return;
                }
            }
        }
    }

    bool HasLivingTarget()
    {
        if (currentGuard != null && !currentGuard.isDead)
        {
            return true;
        }

        if (currentGuard123 != null && !currentGuard123.isDead)
        {
            return true;
        }

        if (currentBoss != null && !currentBoss.isDead)
        {
            return true;
        }

        if (currentMeleeGuard != null && !currentMeleeGuard.isDead)
        {
            return true;
        }

        return false;
    }

    bool IsBehindTarget(Transform targetTransform)
    {
        Vector3 targetToPlayer = transform.position - targetTransform.position;
        targetToPlayer.y = 0f;

        if (targetToPlayer.magnitude < 0.01f)
        {
            return false;
        }

        targetToPlayer.Normalize();

        float dot = Vector3.Dot(targetTransform.forward, targetToPlayer);

        return dot < behindDotThreshold;
    }

    IEnumerator DoAssassination()
    {
        if (!HasLivingTarget())
        {
            yield break;
        }

        isAssassinating = true;

        if (promptUI != null)
        {
            promptUI.SetActive(false);
        }

        if (inputManager != null)
        {
            inputManager.SetCanMove(false);
        }

        if (animatorManager != null)
        {
            animatorManager.TriggerAssassinate();
        }

        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayAssassinateDelayed(assassinateSoundDelay);
        }

        yield return new WaitForSeconds(killDelay);

        if (currentGuard != null && !currentGuard.isDead)
        {
            currentGuard.characterHitDamage(assassinateDamage);
        }

        if (currentGuard123 != null && !currentGuard123.isDead)
        {
            currentGuard123.characterHitDamage(assassinateDamage);
        }

        if (currentBoss != null && !currentBoss.isDead)
        {
            currentBoss.characterHitDamage(assassinateDamage);
        }

        if (currentMeleeGuard != null && !currentMeleeGuard.isDead)
        {
            currentMeleeGuard.characterHitDamage(assassinateDamage);
        }

        currentGuard = null;
        currentGuard123 = null;
        currentBoss = null;
        currentMeleeGuard = null;

        yield return new WaitForSeconds(0.3f);

        if (inputManager != null)
        {
            inputManager.SetCanMove(true);
        }

        isAssassinating = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, assassinateRange);
    }
}