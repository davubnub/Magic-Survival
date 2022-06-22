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
    public PlayerMovement playerMovement;
    public int xpToLevelUp;
    public float xpIncr;

    int xp;
    int level;
    int coins;
    int score;
    float health;
    float fireRateTimer;

    bool paused = true;

    [System.Serializable]
    public struct UpgradableStats
    {
        [Header("Player stats")]
        public float playerSpeed; 
        public float magnetStrength; 

        [Header("Health stats")]
        public float maxHealth;
        public int healingAmount;

        [Header("Projectile stats")]
        public int projectileSpeed;
        public int projectilePierce;
        public int criticalChance;
        public float projectileDamage;
        public float projectileSize;
        public float projectileRange;
        public float projectileKnockback;

        public float fireRate;
        public float accuracy;

        [Header("Projectile special stats")]
        public float homingStrength;
        public float spinStrength;
        public float explosionSize;
        public float stunAmount;
        public float trailLength;
    }

    public UpgradableStats upgradableStats;

    public enum UPGRADES
    {
        playerSpeed,
        maxHealth,
        projectileSpeed,
        fireRate,
        piercing,
        spread,
        magnet,
        knockback,
        glassCannon,
        homing,
        critical,
    };

    private void Start()
    {
        Time.timeScale = 1;
        health = upgradableStats.maxHealth;
        xp = 0;
        level = 0;
        score = 0;
        coins = PlayerPrefs.GetInt("Coins", 0);
        fireRateTimer = upgradableStats.fireRate;

        //update UI
        inGameUI.UpdateHealthBar(health, upgradableStats.maxHealth);
        inGameUI.UpdateXPBar(xp, xpToLevelUp);
        inGameUI.UpdateLevelText(level);
        inGameUI.UpdateCoinText(coins);
        inGameUI.UpdateScoreText(score);

        playerMovement.UpdateMovemnentSpeed(upgradableStats.playerSpeed);

        SetPaused(true);
    }

    private void Update()
    {
        if (!paused)
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("xp"))
        {
            IncreaseXP(1);
            Destroy(other.gameObject);
        }
        if(other.CompareTag("Coin"))
        {
            IncreaseCoins(1);
            Destroy(other.gameObject);
        }
    }

    public void SetPaused(bool _pause)
    {
        paused = _pause;

        playerMovement.SetPaused(_pause);

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<EnemyScript>().SetPaused(_pause);
        }
    }

    public void StartPressed()
    {
        SetPaused(false);
        uiManager.ShowPlayScreen(false);
        uiManager.ShowInGameUI(true);
    }

    public void UpdateHealth(float _damage)
    {
        health += _damage;
        inGameUI.UpdateHealthBar(health, upgradableStats.maxHealth);

        if(health <= 0)
        {
            PlayerDied();
        }
    }

    void PlayerDied()
    {
        if(score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }        

        uiManager.ShowGameOverScreen(true);
        uiManager.ShowInGameUI(true);
        menuUI.UpdateScoreText(score);
        menuUI.UpdateHighScoreText(PlayerPrefs.GetInt("HighScore"));
        Time.timeScale = 0;
    }

    void IncreaseXP(int _xp)
    {
        xp += _xp;

        if(xp >= xpToLevelUp)
        {
            LeveledUp();
        }
        inGameUI.UpdateXPBar(xp, xpToLevelUp);
        inGameUI.UpdateLevelText(level);
    }
    public void IncreaseCoins(int _coin)
    {
        coins += _coin;

        PlayerPrefs.SetInt("Coins", coins);

        inGameUI.UpdateCoinText(coins);
    }

    public void IncreaseScore(int _score)
    {
        score += _score;
        inGameUI.UpdateScoreText(score);
    }

    void LeveledUp()
    {
        xp = 0;
        level++;
        xpToLevelUp = (int)(xpToLevelUp * xpIncr);
        UpdateHealth(upgradableStats.healingAmount);
        uiManager.ShowUpgradeUI(true);
        upgradeManager.SelectOptions();
        SetPaused(true);
        Time.timeScale = 0;
    }

    void FireProjectile(Quaternion _direction)
    {
        GameObject projectileObj = Instantiate(projectile, transform.position, _direction);
        projectileObj.transform.localEulerAngles = _direction.eulerAngles;
        projectileObj.GetComponent<ProjectileScript>().FireProjectile(upgradableStats.projectileSpeed, upgradableStats.projectileRange, upgradableStats.projectileDamage, upgradableStats.projectilePierce, upgradableStats.accuracy, upgradableStats.homingStrength);
    }

    public UpgradableStats GetUpgradableStats()
    {
        return upgradableStats;
    }

    void UpdateMaxHealth()
    {
        if(health > upgradableStats.maxHealth)
        {
            health = upgradableStats.maxHealth;
        }
    }

    public void Upgrade(UPGRADES _upgrade, float _positiveUpgrade, float _negativeUpgrade)
    {
        SetPaused(false);
        uiManager.ShowUpgradeUI(false);
        Time.timeScale = 1;

        switch (_upgrade)
        {
            case UPGRADES.playerSpeed:
                upgradableStats.playerSpeed += _positiveUpgrade;
                playerMovement.UpdateMovemnentSpeed(upgradableStats.playerSpeed);
                break;

            case UPGRADES.maxHealth:
                upgradableStats.maxHealth += (int)_positiveUpgrade;
                inGameUI.UpdateHealthBar(health, upgradableStats.maxHealth);
                UpdateMaxHealth();
                break;

            case UPGRADES.projectileSpeed:
                upgradableStats.projectileSpeed += (int)_positiveUpgrade;
                break;

            case UPGRADES.fireRate:
                upgradableStats.fireRate += _positiveUpgrade;
                break;

            case UPGRADES.piercing:
                upgradableStats.projectilePierce += (int)_positiveUpgrade;
                break;

            case UPGRADES.spread:
                upgradableStats.accuracy += _positiveUpgrade;
                upgradableStats.projectileDamage *= _negativeUpgrade;
                break;

            case UPGRADES.magnet:
                upgradableStats.magnetStrength += _positiveUpgrade;
                break;

            case UPGRADES.knockback:
                upgradableStats.projectileKnockback += _positiveUpgrade;
                break;

            case UPGRADES.glassCannon:
                upgradableStats.projectileDamage *= _positiveUpgrade;
                upgradableStats.maxHealth /= _negativeUpgrade;
                inGameUI.UpdateHealthBar(health, upgradableStats.maxHealth);
                UpdateMaxHealth();
                break;

            case UPGRADES.homing:
                upgradableStats.homingStrength += _positiveUpgrade;
                break;

            case UPGRADES.critical:
                upgradableStats.criticalChance += (int)_positiveUpgrade;
                break;
        }
    }
}
