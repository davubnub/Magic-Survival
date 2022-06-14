using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject projectile;

    public GameObject playerModel;

    public InGameUIManager inGameUI;
    public MenuUIManager menuUI;
    public UpgradeManager upgradeManager;

    public int maxHealth;
    public int maxXp;
    public int healingAmount;
    public float fireRate;
    int health;
    int xp;
    int level;
    float fireRateTimer;

    private void Start()
    {
        Time.timeScale = 1;
        health = maxHealth;
        xp = 0;
        level = 0;
        fireRateTimer = fireRate;

        inGameUI.UpdateHealthBar(health, maxHealth);
        inGameUI.UpdateXPBar(xp, maxXp);
        inGameUI.UpdateLevelText(level);

        menuUI.ShowGameOverScreen(false);
        inGameUI.ShowInGameUI(true);
        upgradeManager.ShowUpgradeUI(false);
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
                fireRateTimer = fireRate;
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
        inGameUI.UpdateHealthBar(health, maxHealth);

        if(health <= 0)
        {
            menuUI.ShowGameOverScreen(true);
            inGameUI.ShowInGameUI(true);
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
        UpdateHealth(healingAmount);
        upgradeManager.ShowUpgradeUI(true);
        upgradeManager.SelectOptions();
        Time.timeScale = 0; //pause instead
    }

    public void Upgrade()
    {
        Time.timeScale = 1;
        upgradeManager.ShowUpgradeUI(false);
    }

    void FireProjectile(Quaternion _direction)
    {
        GameObject projectileObj = Instantiate(projectile, transform.position, _direction);
        projectileObj.transform.localEulerAngles = _direction.eulerAngles;
        projectileObj.GetComponent<ProjectileScript>().FireProjectile();
    }
}
