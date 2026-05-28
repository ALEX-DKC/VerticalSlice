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

    [Header("Debug")]
    public bool debugIsUnarmed;
    public bool debugFoundGuard;
    public bool debugIsBehind;
    public bool debugCanAssassinate;
    public string debugTargetName;

    private Guard currentGuard;
    private Guard123 currentGuard123;

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

        debugFoundGuard = false;
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
                debugFoundGuard = true;
                debugTargetName = guard.gameObject.name;

                bool behind = IsBehindGuard(guard.transform);
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
                debugFoundGuard = true;
                debugTargetName = guard123.gameObject.name;

                bool behind = IsBehindGuard(guard123.transform);
                debugIsBehind = behind;

                if (!guard123.isDead && behind)
                {
                    currentGuard123 = guard123;
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

        return false;
    }

    bool IsBehindGuard(Transform guardTransform)
    {
        Vector3 guardToPlayer = transform.position - guardTransform.position;
        guardToPlayer.y = 0f;

        if (guardToPlayer.magnitude < 0.01f)
        {
            return false;
        }

        guardToPlayer.Normalize();

        float dot = Vector3.Dot(guardTransform.forward, guardToPlayer);

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

        yield return new WaitForSeconds(killDelay);

        if (currentGuard != null && !currentGuard.isDead)
        {
            currentGuard.characterHitDamage(assassinateDamage);
        }

        if (currentGuard123 != null && !currentGuard123.isDead)
        {
            currentGuard123.characterHitDamage(assassinateDamage);
        }

        currentGuard = null;
        currentGuard123 = null;

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