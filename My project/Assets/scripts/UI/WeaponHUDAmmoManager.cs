using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class WeaponHUDAmmoManager : MonoBehaviour
{
    public enum WeaponState
    {
        Unarmed,
        Pistol,
        Rifle
    }

    [Header("Input Manager")]
    public InputManager inputManager;

    [Header("Current Weapon")]
    public WeaponState currentWeapon = WeaponState.Unarmed;

    [Header("Animator")]
    public AnimatorManager animatorManager;
    public Animator playerAnimator;
    public string pistolReloadTriggerName = "Reload";
    public string rifleReloadTriggerName = "rifleReload";

    [Header("Reload State")]
    public bool isReloading = false;
    private WeaponState reloadingWeapon = WeaponState.Unarmed;

    [Header("Original Weapon Icon Images")]
    public Image unarmedIcon;
    public Image pistolIcon;
    public Image rifleIcon;

    [Header("Weapon Number Texts")]
    public TextMeshProUGUI unarmedNumberText;
    public TextMeshProUGUI pistolNumberText;
    public TextMeshProUGUI rifleNumberText;

    [Header("Ammo UI")]
    public GameObject ammoHUD;
    public TextMeshProUGUI magazineText;
    public TextMeshProUGUI bulletText;
    public GameObject separatorLine;

    [Header("Ammo Icon Objects")]
    public GameObject pistolAmmoIcon;
    public GameObject rifleAmmoIcon;

    [Header("Colors")]
    public Color selectedColor = Color.black;
    public Color unselectedColor = new Color(0.45f, 0.45f, 0.45f, 1f);

    [Header("Pistol Ammo")]
    public int pistolMagSize = 9;
    public int pistolCurrentBullets = 9;
    public int pistolRemainingMagazines = 5;

    [Header("Rifle Ammo")]
    public int rifleMagSize = 2;
    public int rifleCurrentBullets = 2;
    public int rifleRemainingMagazines = 5;

    [Header("Reload Input Cooldown")]
    public float reloadInputCooldown = 2f;
    private float nextReloadAllowedTime = 0f;

    void Start()
    {
        pistolCurrentBullets = pistolMagSize;
        rifleCurrentBullets = rifleMagSize;

        if (inputManager == null)
        {
            inputManager = GetComponent<InputManager>();
        }

        if (animatorManager == null)
        {
            animatorManager = GetComponent<AnimatorManager>();
        }

        if (playerAnimator == null)
        {
            playerAnimator = GetComponent<Animator>();
        }

        UpdateHUD();
    }

    void Update()
    {
        if (!isReloading)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                SetWeaponUnarmed();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                SetWeaponPistol();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                SetWeaponRifle();
            }
        }

        HandleReloadInput();
    }

    void HandleReloadInput()
    {
        if (inputManager == null)
        {
            return;
        }

        if (inputManager.IsReloadPressed())
        {
            StartReloadCurrentWeapon();
            inputManager.ResetReloadInput();
        }
    }

    public void SetWeaponUnarmed()
    {
        if (isReloading) return;

        currentWeapon = WeaponState.Unarmed;
        UpdateHUD();

        Debug.Log("Current Weapon: Unarmed");
    }

    public void SetWeaponPistol()
    {
        if (isReloading) return;

        currentWeapon = WeaponState.Pistol;
        UpdateHUD();

        Debug.Log("Current Weapon: Pistol");
    }

    public void SetWeaponRifle()
    {
        if (isReloading) return;

        currentWeapon = WeaponState.Rifle;
        UpdateHUD();

        Debug.Log("Current Weapon: Rifle");
    }

    public bool TryShootCurrentWeapon()
    {
        if (isReloading)
        {
            Debug.Log("Cannot shoot while reloading.");
            return false;
        }

        if (currentWeapon == WeaponState.Pistol)
        {
            return TryShootPistol();
        }

        if (currentWeapon == WeaponState.Rifle)
        {
            return TryShootRifle();
        }

        return false;
    }

    public bool TryShootPistol()
    {
        if (isReloading)
        {
            Debug.Log("Cannot shoot pistol while reloading.");
            return false;
        }

        if (currentWeapon != WeaponState.Pistol)
        {
            Debug.Log("Cannot shoot pistol because current weapon is not Pistol.");
            return false;
        }

        if (pistolCurrentBullets <= 0)
        {
            Debug.Log("Pistol is empty. Press R to reload.");
            UpdateHUD();
            return false;
        }

        pistolCurrentBullets--;

        Debug.Log("Pistol shot. Bullets left: " + pistolCurrentBullets);

        UpdateHUD();
        return true;
    }

    public bool TryShootRifle()
    {
        if (isReloading)
        {
            Debug.Log("Cannot shoot rifle while reloading.");
            return false;
        }

        if (currentWeapon != WeaponState.Rifle)
        {
            Debug.Log("Cannot shoot rifle because current weapon is not Rifle.");
            return false;
        }

        if (rifleCurrentBullets <= 0)
        {
            Debug.Log("Rifle is empty. Press R to reload.");
            UpdateHUD();
            return false;
        }

        rifleCurrentBullets--;

        Debug.Log("Rifle shot. Bullets left: " + rifleCurrentBullets);

        UpdateHUD();
        return true;
    }

    public bool StartReloadCurrentWeapon()
    {
        if (Time.time < nextReloadAllowedTime)
        {
            Debug.Log("Reload input ignored. Need to wait 2 seconds.");
            return false;
        }

        if (isReloading)
        {
            Debug.Log("Already reloading.");
            return false;
        }

        if (currentWeapon == WeaponState.Unarmed)
        {
            Debug.Log("Cannot reload while unarmed.");
            return false;
        }

        if (currentWeapon == WeaponState.Pistol)
        {
            if (pistolCurrentBullets >= pistolMagSize)
            {
                Debug.Log("Pistol magazine is already full.");
                return false;
            }

            if (pistolRemainingMagazines <= 0)
            {
                Debug.Log("No pistol magazines left.");
                return false;
            }
        }

        if (currentWeapon == WeaponState.Rifle)
        {
            if (rifleCurrentBullets >= rifleMagSize)
            {
                Debug.Log("Rifle magazine is already full.");
                return false;
            }

            if (rifleRemainingMagazines <= 0)
            {
                Debug.Log("No rifle magazines left.");
                return false;
            }
        }

        // 关键：只是限制下一次 R 输入，不是延迟补子弹
        nextReloadAllowedTime = Time.time + reloadInputCooldown;

        isReloading = true;
        reloadingWeapon = currentWeapon;

        PlayReloadAnimation();

        Debug.Log("Reload animation started for: " + reloadingWeapon);

        return true;
    }

    void PlayReloadAnimation()
    {
        if (animatorManager != null)
        {
            if (reloadingWeapon == WeaponState.Pistol)
            {
                animatorManager.TriggerReload();
            }

            if (reloadingWeapon == WeaponState.Rifle)
            {
                animatorManager.TriggerRifleReload();
            }

            return;
        }

        if (playerAnimator != null)
        {
            if (reloadingWeapon == WeaponState.Pistol)
            {
                playerAnimator.ResetTrigger(pistolReloadTriggerName);
                playerAnimator.SetTrigger(pistolReloadTriggerName);
            }

            if (reloadingWeapon == WeaponState.Rifle)
            {
                playerAnimator.ResetTrigger(rifleReloadTriggerName);
                playerAnimator.SetTrigger(rifleReloadTriggerName);
            }

            return;
        }

        Debug.LogWarning("WeaponHUDAmmoManager: No AnimatorManager or Animator found.");
    }

    // Animation Event 调用这个函数
    // 动画快结束时才真正补子弹
    public void FinishReloadAnimationEvent()
    {
        if (!isReloading)
        {
            return;
        }

        if (reloadingWeapon == WeaponState.Pistol)
        {
            pistolRemainingMagazines--;
            pistolCurrentBullets = pistolMagSize;

            Debug.Log("Pistol reload finished. Bullets: " + pistolCurrentBullets + ", Magazines left: " + pistolRemainingMagazines);
        }

        if (reloadingWeapon == WeaponState.Rifle)
        {
            rifleRemainingMagazines--;
            rifleCurrentBullets = rifleMagSize;

            Debug.Log("Rifle reload finished. Bullets: " + rifleCurrentBullets + ", Magazines left: " + rifleRemainingMagazines);
        }

        isReloading = false;
        reloadingWeapon = WeaponState.Unarmed;

        UpdateHUD();
    }

    public void CancelReload()
    {
        isReloading = false;
        reloadingWeapon = WeaponState.Unarmed;

        UpdateHUD();

        Debug.Log("Reload cancelled.");
    }

    void UpdateHUD()
    {
        UpdateWeaponIcons();
        UpdateWeaponNumbers();
        UpdateAmmoHUD();
    }

    void UpdateWeaponIcons()
    {
        SetImageColor(unarmedIcon, currentWeapon == WeaponState.Unarmed);
        SetImageColor(pistolIcon, currentWeapon == WeaponState.Pistol);
        SetImageColor(rifleIcon, currentWeapon == WeaponState.Rifle);
    }

    void UpdateWeaponNumbers()
    {
        SetTextColor(unarmedNumberText, currentWeapon == WeaponState.Unarmed);
        SetTextColor(pistolNumberText, currentWeapon == WeaponState.Pistol);
        SetTextColor(rifleNumberText, currentWeapon == WeaponState.Rifle);
    }

    void UpdateAmmoHUD()
    {
        if (currentWeapon == WeaponState.Unarmed)
        {
            SetActiveSafe(ammoHUD, false);
            SetActiveSafe(separatorLine, false);
            SetActiveSafe(pistolAmmoIcon, false);
            SetActiveSafe(rifleAmmoIcon, false);
            return;
        }

        SetActiveSafe(ammoHUD, true);
        SetActiveSafe(separatorLine, true);

        if (currentWeapon == WeaponState.Pistol)
        {
            if (magazineText != null)
            {
                magazineText.text = pistolRemainingMagazines.ToString();
            }

            if (bulletText != null)
            {
                bulletText.text = pistolCurrentBullets.ToString();
            }

            SetActiveSafe(pistolAmmoIcon, true);
            SetActiveSafe(rifleAmmoIcon, false);
            return;
        }

        if (currentWeapon == WeaponState.Rifle)
        {
            if (magazineText != null)
            {
                magazineText.text = rifleRemainingMagazines.ToString();
            }

            if (bulletText != null)
            {
                bulletText.text = rifleCurrentBullets.ToString();
            }

            SetActiveSafe(pistolAmmoIcon, false);
            SetActiveSafe(rifleAmmoIcon, true);
            return;
        }
    }

    void SetImageColor(Image image, bool selected)
    {
        if (image == null)
        {
            return;
        }

        image.color = selected ? selectedColor : unselectedColor;
    }

    void SetTextColor(TextMeshProUGUI text, bool selected)
    {
        if (text == null)
        {
            return;
        }

        text.color = selected ? selectedColor : unselectedColor;
    }

    void SetActiveSafe(GameObject obj, bool active)
    {
        if (obj != null)
        {
            obj.SetActive(active);
        }
    }
}