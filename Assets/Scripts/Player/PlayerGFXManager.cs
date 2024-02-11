using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGFXManager : MonoBehaviour
{
    [SerializeField]
    private GameObject hairObj;
    [SerializeField]
    private GameObject beardObj;
    [SerializeField]
    private GameObject torsoObj;
    [SerializeField]
    private SpriteRenderer headArmor;
    [SerializeField]
    private SpriteRenderer headEquipment;
    [SerializeField]
    private SpriteRenderer bodyArmor;
    [SerializeField]
    private SpriteRenderer bodyEquipment;

    private GameObject headSlot;
    private GameObject headEquipmentSlot;
    private GameObject torsoSlot;
    private GameObject torsoEquipmentSlot;


    void Start()
    {
        headSlot = GetComponent<PlayerInventory>().armorSlots.transform.Find("Head").gameObject;
        headEquipmentSlot = GetComponent<PlayerInventory>().equipmentSlots.transform.Find("HeadEquipment").gameObject;
        torsoSlot = GetComponent<PlayerInventory>().armorSlots.transform.Find("Torso").gameObject;
        torsoEquipmentSlot = GetComponent<PlayerInventory>().equipmentSlots.transform.Find("TorsoEquipment").gameObject;
    }

    public void UpdateGFX()
    {
        Item item;

        // HeadArmor
        if (headSlot.transform.childCount > 0 && headSlot.transform.GetChild(0).TryGetComponent(out item))
        {
            hairObj.SetActive(false);
            headArmor.sprite = item.sprite_equip;
        }
        else
        {
            hairObj.SetActive(true);
            headArmor.sprite = null;
        }
        // HeadEquipment
        if (headEquipmentSlot.transform.childCount > 0 && headEquipmentSlot.transform.GetChild(0).TryGetComponent(out item))
        {
            headEquipment.sprite = item.sprite_equip;
        }
        else
        {
            headEquipment.sprite = null;
        }
        // BodyArmor
        if (torsoSlot.transform.childCount > 0 && torsoSlot.transform.GetChild(0).TryGetComponent(out item))
        {
            bodyArmor.sprite = item.sprite_equip;
        }
        else
        {
            bodyArmor.sprite = null;
        }
        // BodyEquipment
        if (torsoEquipmentSlot.transform.childCount > 0 && torsoEquipmentSlot.transform.GetChild(0).TryGetComponent(out item))
        {
            bodyEquipment.sprite = item.sprite_equip;
        }
        else
        {
            bodyEquipment.sprite = null;
        }
    }
}
