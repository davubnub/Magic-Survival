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
    public Joystick aimingJoystick;
    public int xpToLevelUp;
    public float xpIncr;
    public float maxHealthLevelUp;
    public float speedLevelUp;

    int xp;
    int level;
    int coins;
    int score;
    float health;
    float fireRateTimer;
    float regenerationTimer;

    bool paused = true;

    bool playingOnComputer = false;
    bool playingOnPhone = false;

    [System.Serializable]
    public struct UpgradableStats
    {
        [Header("Player stats")]
        public float playerSpeed; 
        public float magnetStrength; 

        [Header("Health stats")]
        public float maxHealth;
        public int regeneration;

        [Header("Projectile stats")]
        public int projectileSpeed;
        public int projectilePierce;
        public int criticalChance;
        public int projectiles;
        public float bulletDamage;
        public float bulletsSize;
        public float bulletRange;
        public float bulletKnockback;

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
        none,
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
        sniper,
        extraProjectile,
        submachineGun,
        regeneration,
    };

    private void Start()
    {
        Time.timeScale = 1;
        health = upgradableStats.maxHealth;
        regenerationTimer = upgradableStats.regeneration;
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

        #if UNITY_STANDALONE_WIN
            playingOnComputer = true;
        #endif
        #if UNITY_IOS || UNITY_ANDROID || UNITY_IPHONE
            playingOnPhone = true;
        #endif
    }

    private void Update()
    {
        if (!paused)
        {
            if (playingOnComputer)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    playerModel.transform.LookAt(hit.point);
                    playerModel.transform.localEulerAngles = new Vector3(0, playerModel.transform.localEulerAngles.y, 0);
                }
            }
            if (playingOnPhone && aimingJoystick.Direction.magnitude != 0)
            {
                playerModel.transform.localEulerAngles = new Vector3(0, Angle(aimingJoystick.Direction) - 45, 0);
            }

            fireRateTimer -= Time.deltaTime;

            if ((aimingJoystick.Direction.magnitude != 0 && playingOnPhone) || (Input.GetMouseButton(0) && playingOnComputer))
            {
                if (fireRateTimer <= 0)
                {
                    /*Vector3 angle = playerModel.transform.rotation.eulerAngles;
                    for (int i = 0; i < upgradableStats.projectiles; i++)
                    {
                        FireProjectile(angle);
                    }*/

                    int amount = upgradableStats.projectiles;
                    int angleRange = 5 + ((amount - 1) * 20);
                    int newAngle = (amount == 1) ? 0 : angleRange / (amount - 1);
                    for(int i = 0; i < amount; i++)
                    {
                        Vector3 angle = playerModel.transform.rotation.eulerAngles;
                        angle += new Vector3(0, (i * newAngle) + (-angleRange / 2), 0);
                        FireProjectile(angle);
                    }

                    fireRateTimer = upgradableStats.fireRate;
                }
            }
            if (upgradableStats.regeneration > 0)
            {
                regenerationTimer -= Time.deltaTime;

                if(regenerationTimer <= 0)
                {
                    regenerationTimer = 1; //every 5 seconds
                    UpdateHealth(upgradableStats.regeneration);
                }
            }

            //DEBUG
            if (Input.GetKey(KeyCode.Q))
            {
                IncreaseXP(xpToLevelUp - xp);
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
        menuUI.GameOver(coins);
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

        upgradableStats.maxHealth   += maxHealthLevelUp;
        upgradableStats.playerSpeed += speedLevelUp;
        UpdateStats();

        xpToLevelUp = (int)(xpToLevelUp * xpIncr);
        uiManager.ShowUpgradeUI(true);
        upgradeManager.SelectOptions();
        SetPaused(true);
        //Time.timeScale = 0;
    }

    float Angle(Vector2 vector2)
    {
        if (vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg;
        }
    }

    void FireProjectile(Vector3 _direction)
    {
        GameObject projectileObj = Instantiate(projectile, transform.position, Quaternion.identity);
        projectileObj.transform.localEulerAngles = _direction;
        projectileObj.GetComponent<ProjectileScript>().FireProjectile(upgradableStats.projectileSpeed, upgradableStats.bulletRange, upgradableStats.bulletDamage, upgradableStats.projectilePierce, upgradableStats.accuracy, upgradableStats.homingStrength);
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
                upgradableStats.playerSpeed         += _positiveUpgrade;
                break;

            case UPGRADES.maxHealth:
                upgradableStats.maxHealth       += (int)_positiveUpgrade;
                upgradableStats.playerSpeed     /= _negativeUpgrade;
                break;

            case UPGRADES.projectileSpeed:
                upgradableStats.projectileSpeed += (int)_positiveUpgrade;
                break;

            case UPGRADES.fireRate:
                upgradableStats.fireRate        += _positiveUpgrade;
                break;

            case UPGRADES.piercing:
                upgradableStats.projectilePierce += (int)_positiveUpgrade;
                break;

            case UPGRADES.spread:
                upgradableStats.accuracy        += _positiveUpgrade;
                upgradableStats.bulletDamage    *= _negativeUpgrade;
                break;

            case UPGRADES.magnet:
                upgradableStats.magnetStrength  += _positiveUpgrade;
                break;

            case UPGRADES.knockback:
                upgradableStats.bulletKnockback += _positiveUpgrade;
                break;

            case UPGRADES.glassCannon:
                upgradableStats.bulletDamage    *= _positiveUpgrade;
                upgradableStats.maxHealth       /= _negativeUpgrade;
                break;

            case UPGRADES.homing:
                upgradableStats.homingStrength  += _positiveUpgrade;
                break;

            case UPGRADES.critical:
                upgradableStats.criticalChance  += (int)_positiveUpgrade;
                break;

            case UPGRADES.sniper:
                upgradableStats.bulletRange     += _positiveUpgrade;
                upgradableStats.fireRate        += _negativeUpgrade;
                break;

            case UPGRADES.extraProjectile:
                upgradableStats.projectiles     += (int)_positiveUpgrade;
                upgradableStats.bulletDamage    /= _negativeUpgrade;
                break;

            case UPGRADES.submachineGun:
                upgradableStats.fireRate        /= _positiveUpgrade;
                upgradableStats.bulletRange     *= _negativeUpgrade;
                break;

            case UPGRADES.regeneration:
                upgradableStats.regeneration    += (int)_positiveUpgrade;
                break;
        }

        UpdateStats();
    }

    void UpdateStats()
    {

        UpdateMaxHealth();
        inGameUI.UpdateHealthBar(health, upgradableStats.maxHealth);
        playerMovement.UpdateMovemnentSpeed(upgradableStats.playerSpeed);
    }
}
