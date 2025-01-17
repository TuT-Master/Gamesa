using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ItemCardStat : MonoBehaviour
{
    [SerializeField] private Image statImage;
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI statValue;
    [SerializeField] private Transform statEffects;
    [SerializeField] private GameObject bar;
    [SerializeField] private GameObject fillBar_main;
    [SerializeField] private GameObject fillBar_bonus;

    [HideInInspector] public int age;

    [Header("Fillbar gfx")]
    [SerializeField] private Sprite fillBarMain;
    [SerializeField] private Sprite fillBarBonus_plus;
    [SerializeField] private Sprite fillBarBonus_minus;
    [SerializeField] private Sprite fillBarBackground;


    [Header("Stat images")]
    [SerializeField] private Sprite damage;
    [SerializeField] private Sprite penetration;
    [SerializeField] private Sprite armorIgnore;
    [SerializeField] private Sprite critChance;
    [SerializeField] private Sprite critDamage;
    [SerializeField] private Sprite magazineSize;
    [SerializeField] private Sprite attackSpeed;
    [SerializeField] private Sprite reloadTime;
    [SerializeField] private Sprite defense;
    [SerializeField] private Sprite additionalSlots;
    [SerializeField] private Sprite armor;
    [SerializeField] private Sprite magicResistance;

    [Header("Stat effects")]
    [SerializeField] private GameObject statEffectPrefab;
    [SerializeField] List<ItemCard.StatEffect> _statEffect;
    [SerializeField] List<Sprite> _statEffectSprites;
    private Dictionary<ItemCard.StatEffect, Sprite> statEffect_Sprite_pairs;


    // Each stat has its own fillbar max values per age
    private readonly Dictionary<string, List<float>> fillBarMaxValues = new()
    {
        {"damage", new List<float>(){ 20, 40, 60, 80, 100 } },
        {"penetration", new List<float>(){ 10, 20, 30, 40, 50 } },
        {"armorIgnore", new List<float>(){ 1, 1, 1, 1, 1 } },
        {"critChance", new List<float>(){ 1, 1, 1, 1, 1 } },
        {"critDamage", new List<float>(){ 3, 3, 3, 3, 3 } },
        {"magazineSize", new List<float>(){ 15, 20, 40, 100, 500 } },
        {"attackSpeed", new List<float>(){ 5, 10, 15, 20, 40 } },
        {"reloadTime", new List<float>(){ 5, 5, 5, 5, 5 } },
        {"defense", new List<float>(){ 100, 100, 100, 100, 100 } },
        {"backpackSize", new List<float>(){ 20, 20, 20, 20, 20 } },
        {"armor", new List<float>(){ 15, 30, 45, 60, 75 } },
        {"magicResistance", new List<float>(){ 15, 30, 45, 60, 75 } },
    };
    /*
    ammo
    effect (equipables)
    */


    public void SetUp(string stat, float defaultValue, float bonusValue)
    {
        statEffect_Sprite_pairs = new();
        for(int i = 0; i < _statEffect.Count; i++)
            statEffect_Sprite_pairs.Add(_statEffect[i], _statEffectSprites[i]);
        // Set up name of stat
        switch(stat)
        {
            case "damage":
                statName.text = "Damage";
                statImage.sprite = damage;
                break;
            case "penetration":
                statName.text = "Penetration";
                statImage.sprite = penetration;
                break;
            case "armorIgnore":
                statName.text = "Armor ignore";
                statImage.sprite = armorIgnore;
                break;
            case "critChance":
                statName.text = "Critical chance";
                statImage.sprite = critChance;
                break;
            case "critDamage":
                statName.text = "Critical damage";
                statImage.sprite = critDamage;
                break;
            case "magazineSize":
                statName.text = "Magazine size";
                statImage.sprite = magazineSize;
                break;
            case "attackSpeed":
                statName.text = "Attack speed";
                statImage.sprite = attackSpeed;
                break;
            case "reloadTime":
                statName.text = "Reload time";
                statImage.sprite = reloadTime;
                break;
            case "defense":
                statName.text = "Defense";
                statImage.sprite = defense;
                break;
            case "backpackSize":
                statName.text = "Additional slots";
                statImage.sprite = additionalSlots;
                break;
            case "armor":
                statName.text = "Armor";
                statImage.sprite = armor;
                break;
            case "magicResistance":
                statName.text = "Magic resistance";
                statImage.sprite = magicResistance;
                break;
            default:
                Debug.Log("Stat not found! Given parameter: " + stat);
                break;
        }

        // Set up values of stat
        if (stat == "armorIgnore")
            statValue.text = Math.Round((defaultValue + bonusValue) * 100, 2).ToString() + "%";
        else if (stat == "attackSpeed")
            statValue.text = Math.Round(defaultValue + bonusValue, 2).ToString() + " / s";
        else if (stat == "reloadTime")
            statValue.text = Math.Round(defaultValue + bonusValue, 2).ToString() + " s";
        else if (stat == "backpackSize")
            statValue.text = "+ " + Math.Round(defaultValue + bonusValue, 2).ToString();
        else
            statValue.text = Math.Round(defaultValue + bonusValue, 2).ToString();

        // Update fillBar
        float _mainValue = defaultValue / fillBarMaxValues[stat][age];
        float _bonusValue = (defaultValue + bonusValue) / fillBarMaxValues[stat][age];
        
        fillBar_main.GetComponent<Image>().sprite = fillBarMain;
        fillBar_main.GetComponent<Image>().type = Image.Type.Filled;
        fillBar_main.GetComponent<Image>().fillMethod = Image.FillMethod.Horizontal;
        fillBar_main.GetComponent<Image>().fillAmount = _mainValue;
        
        fillBar_bonus.GetComponent<Image>().sprite = fillBarBonus_plus;
        fillBar_bonus.GetComponent<Image>().type = Image.Type.Filled;
        fillBar_bonus.GetComponent<Image>().fillMethod = Image.FillMethod.Horizontal;

        fillBar_bonus.SetActive(true);
        if (bonusValue == 0)
            fillBar_bonus.SetActive(false);
        else if (bonusValue > 0)
            fillBar_bonus.GetComponent<Image>().fillAmount = _bonusValue;
        else if (bonusValue < 0)
        {
            fillBar_bonus.GetComponent<Image>().fillAmount = _mainValue;
            fillBar_bonus.GetComponent<Image>().sprite = fillBarBonus_minus;
            fillBar_main.GetComponent<Image>().fillAmount = _bonusValue;
        }
    }

    public void AddStatEffect(ItemCard.StatEffect statEffect, float value)
    {
        GameObject newStatEffect = Instantiate(statEffectPrefab, statEffects);
        newStatEffect.GetComponent<ItemCardStatEffect>().SetUp(statEffect, value, statEffect_Sprite_pairs[statEffect]);
    }
    public void AddStatEffect_FullSetBonus(ItemCard.StatEffect statEffect, Dictionary<string, float> values)
    {
        GameObject newStatEffect = Instantiate(statEffectPrefab, statEffects);
        newStatEffect.GetComponent<ItemCardStatEffect>().SetUp(values, statEffect_Sprite_pairs[statEffect]);
    }
    public void RemoveStatEffects()
    {
        for (int i = 0; i < statEffects.childCount; i++)
            Destroy(statEffects.GetChild(i).gameObject);
    }
}
