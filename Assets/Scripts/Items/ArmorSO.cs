using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "Scriptable objects/Armor")]
public class ArmorSO : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string description;

    public Sprite sprite_inventory;
    public Sprite sprite_equipFront;
    public Sprite sprite_equipBack;

    public Slot.SlotType slotType;

    public PlayerBase.BaseUpgrade craftedIn;
    public int requieredCraftingLevel;
    public List<ScriptableObject> recipeMaterials;
    public List<int> recipeMaterialsAmount;

    public bool hideHairWhenEquiped;
    public bool hideBeardWhenEquiped;
    public bool hideBodyWhenEquiped;

    [Header("Stats")]
    [SerializeField] private float armor;
    [SerializeField] private float magicResistance;
    [SerializeField] private float weight;
    [SerializeField] private float price;

    [Header("Full-set bonus")]
    public string armorSetName;
    [SerializeField] private float hpMax;
    [SerializeField] private float staminaMax;
    [SerializeField] private float manaMax;

    public Dictionary <string, float> Stats()
    {
        return new()
        {
            {"armor", armor },
            {"magicResistance", magicResistance },
            {"weight", weight },
            {"price", price},
        };
    }

    public Dictionary<string, float> FullsetBonus()
    {
        return new()
        {
            {"hpMax", hpMax },
            {"staminaMax", staminaMax },
            {"manaMax", manaMax },
        };
    }

    [Header("Upgrade")]
    public ArmorSO upgradedVersionsOfArmor;
}
