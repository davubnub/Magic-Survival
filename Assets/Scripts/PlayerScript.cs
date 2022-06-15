using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerScript : MonoBehaviour
{
    public GameObject projectile;

    public GameObject playerModel;

    public InGameUIManager inGameUI;
    public MenuUIManager menuUI;
    public UpgradeManager upgradeManager;
    public UIManager uiManager;
    public UpgradeStats upgradeStats;
    public int maxXp;
    public float xpIncr;

    int xp;
    int level;
    int health;
    float fireRateTimer;

    [System.Serializable]
    public struct UpgradableStats
    {
        [Header("Player stats")]
        public int playerSpeed; 

        [Header("Health stats")]
        public int maxHealth;
        public int healingAmount;

        [Header("Projectile stats")]
        public int projectileDamage;
        public int projectileSpeed;
        public float projectileSize;
        public float projectileRange;
        public int projectilePhaseThrough;

        public float fireRate;
        public float accuracy;

        [Header("Projectile special stats")]
        public float homingStrength;
        public float spinStrength;
        public float explosionSize;
        public float webStrength;
        public float trailLength;
    }

    public UpgradableStats upgradableStats;

    public enum UPGRADES
    {
        playerSpeed,
        maxHealth,
        projectileSpeed,
        fireRate,
    };

private void Start()
    {
        Time.timeScale = 1;
        health = upgradableStats.maxHealth;
        xp = 0;
        level = 0;
        fireRateTimer = upgradableStats.fireRate;

        inGameUI.UpdateHealthBar(health, upgradableStats.maxHealth);
        inGameUI.UpdateXPBar(xp, maxXp);
        inGameUI.UpdateLevelText(level);

        uiManager.ShowGameOverScreen(false);
        uiManager.ShowInGameUI(true);
        uiManager.ShowUpgradeUI(false);
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            playerModel.transform.LookAt(hit.point);
            playerModel.transform.localEulerAngles = new Vector3(0, playerModel.transform.localEulerAngles.y, 0);
        }

        fireRateTimer -= Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            if (fireRateTimer <= 0)
            {
                FireProjectile(playerModel.transform.rotation);
                fireRateTimer = upgradableStats.fireRate;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("xp"))
        {
            IncreaseXP(1);
            Destroy(other.gameObject);
        }
    }

    public void UpdateHealth(int _damage)
    {
        health += _damage;
        inGameUI.UpdateHealthBar(health, upgradableStats.maxHealth);

        if(health <= 0)
        {
            uiManager.ShowGameOverScreen(true);
            uiManager.ShowInGameUI(true);
            Time.timeScale = 0;
        }
    }

    void IncreaseXP(int _xp)
    {
        xp += _xp;

        if(xp >= maxXp)
        {
            LeveledUp();
        }
        inGameUI.UpdateXPBar(xp, maxXp);
        inGameUI.UpdateLevelText(level);
    }

    void LeveledUp()
    {
        xp = 0;
        level++;
        maxXp = (int)(maxXp * xpIncr);
        UpdateHealth(upgradableStats.healingAmount);
        uiManager.ShowUpgradeUI(true);
        upgradeManager.SelectOptions();
        Time.timeScale = 0; //pause instead
    }

    void FireProjectile(Quaternion _direction)
    {
        GameObject projectileObj = Instantiate(projectile, transform.position, _direction);
        projectileObj.transform.localEulerAngles = _direction.eulerAngles;
        projectileObj.GetComponent<ProjectileScript>().FireProjectile(upgradableStats.projectileSpeed, upgradableStats.projectileRange, upgradableStats.projectileDamage);
    }

    public void Upgrade(UPGRADES _upgrade, float _upgradeValue)
    {
        Time.timeScale = 1;
        uiManager.ShowUpgradeUI(false);

        switch (_upgrade)
        {
            case UPGRADES.playerSpeed:
                upgradableStats.playerSpeed = (int)_upgradeValue;
                break;

            case UPGRADES.maxHealth:
                upgradableStats.maxHealth = (int)_upgradeValue;
                inGameUI.UpdateHealthBar(health, upgradableStats.maxHealth);
                break;

            case UPGRADES.projectileSpeed:
                upgradableStats.projectileSpeed = (int)_upgradeValue;
                break;

            case UPGRADES.fireRate:
                upgradableStats.fireRate = _upgradeValue;
                break;

        }
    }
}
