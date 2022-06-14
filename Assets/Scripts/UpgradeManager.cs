using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public PlayerScript player;

    public GameObject[] optionButtons;
    public TextMeshProUGUI[] upgradeNameText;
    public TextMeshProUGUI[] upgradeDescriptionText;
    public Image[] upgradeIcons;

    const int amountOfOptions = 3;

    string[] upgradeNames = { "Bigger penis", "Chonky", "Thicc ass", "Become gay" };
    string[] upgradeDescriptions = { "Penis becomes very long yes", "chonk", "super thicc", "gay" };

    private void Start()
    {
        ShowUpgradeUI(false);
    }

    public void ShowUpgradeUI(bool _active)
    {
        gameObject.SetActive(_active);
    }

    public void SelectOptions()
    {
        int[] options = { 0, 0, 0 };

        int[] listOfUpgrades = {0, 1, 2, 3};

        for(int i = 0; i < amountOfOptions; i++)
        {
            //pick random upgrade
            options[i] = Random.Range(0, listOfUpgrades.Length);
            //remove that upgrade from the list of upgrades to choose from
            listOfUpgrades[System.Array.IndexOf(listOfUpgrades, options[i])] = listOfUpgrades[listOfUpgrades.Length - 1];
            System.Array.Resize(ref listOfUpgrades, listOfUpgrades.Length - 1);

            upgradeNameText[i].text = upgradeNames[options[i]];
            upgradeDescriptionText[i].text = upgradeDescriptions[options[i]];
        }
    }

    public void OptionButtonPressed(int _number)
    {
        player.Upgrade();
    }
}
