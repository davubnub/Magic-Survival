using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStats : MonoBehaviour
{
    [System.Serializable]
    public class upgradeTiers
    {
        public string upgradeName;
        public PlayerScript.UPGRADES upgrade;
        public string upgradeDescription;
        public float[] tiers;
        [System.NonSerialized]
        public int tierLevel;

        public void SetUpgradeTier(int _tier)
        {
            tierLevel = _tier;
        }
    }

    public List<upgradeTiers> upgradeStats = new List<upgradeTiers>();

    public List<upgradeTiers> GetUpgradeStats()
    {
        return upgradeStats;
    }
}
