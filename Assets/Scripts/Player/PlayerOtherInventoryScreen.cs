using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;
using static UnityEditor.Progress;

public class PlayerOtherInventoryScreen : MonoBehaviour
{
    public Dictionary<int, Item> otherInventory = new();
    private GameObject otherInventoryObj;

    public bool isOpened;


    [SerializeField]
    private GameObject otherInventoryScreen;
    [SerializeField]
    private List<GameObject> playerInventorySlots;
    [SerializeField]
    private List<GameObject> otherInventorySlots;

    private PlayerInventory playerInventory;

    [SerializeField]
    private ItemDatabase itemDatabase;

    [SerializeField]
    private GameObject itemPrefab;



    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
        isOpened = false;
        otherInventoryScreen.SetActive(false);
    }

    public void UpdateOtherInventory(GameObject inventory)
    {
        otherInventoryObj = inventory;
        otherInventory = inventory.GetComponent<OtherInventory>().inventory;

        for (int i = 0; i < otherInventorySlots.Count; i++)
        {
            if (otherInventorySlots[i].transform.childCount > 0)
                Destroy(otherInventorySlots[i].transform.GetChild(0).gameObject);

            if (i < otherInventory.Count)
            {
                otherInventorySlots[i].transform.GetComponent<Slot>().isActive = true;
                otherInventorySlots[i].SetActive(true);

                if (otherInventory[i] != null)
                {
                    var newItem = Instantiate(itemPrefab, otherInventorySlots[i].transform);
                    newItem.GetComponent<Item>().SetUpByItem(otherInventory[i]);
                }
            }
            else
            {
                otherInventorySlots[i].transform.GetComponent<Slot>().isActive = false;
                otherInventorySlots[i].SetActive(false);
            }
        }
    }

    private void UpdatePlayerInventory()
    {
        for (int i = 0; i < playerInventory.backpackInventory.transform.childCount; i++)
        {
            playerInventorySlots[i].GetComponent<Slot>().isActive = playerInventory.backpackInventory.transform.GetChild(i).GetComponent<Slot>().isActive;
            playerInventorySlots[i].SetActive(playerInventorySlots[i].GetComponent<Slot>().isActive);

            if (playerInventorySlots[i].transform.childCount > 0)
                Destroy(playerInventorySlots[i].transform.GetChild(0).gameObject);

            if (playerInventory.backpackInventory.transform.GetChild(i).childCount > 0 && playerInventory.backpackInventory.transform.GetChild(i).GetChild(0).TryGetComponent(out Item itemInSlot))
            {
                var newItem = Instantiate(itemPrefab, playerInventorySlots[i].transform);
                newItem.GetComponent<Item>().SetUpByItem(itemInSlot);
            }
        }
    }

    public void SaveInventory()
    {
        // Player inventory
        for(int i = 0; i < playerInventorySlots.Count; i++)
        {
            if (playerInventory.backpackInventory.transform.GetChild(i).childCount > 0)
                Destroy(playerInventory.backpackInventory.transform.GetChild(i).GetChild(0).gameObject);

            if (playerInventorySlots[i].transform.childCount > 0)
                playerInventorySlots[i].transform.GetChild(0).SetParent(playerInventory.backpackInventory.transform.GetChild(i));
        }

        // Other inventory
        for (int i = 0; i < otherInventorySlots.Count; i++)
        {
            if (otherInventorySlots[i].GetComponent<Slot>().isActive)
            {
                if (otherInventorySlots[i].transform.childCount > 0 && otherInventorySlots[i].transform.GetChild(0).TryGetComponent(out Item item))
                    otherInventory[i] = item;
                else
                    otherInventory[i] = null;
            }
        }
        otherInventoryObj.GetComponent<OtherInventory>().SetUpInventory(otherInventory, otherInventoryObj.GetComponent<OtherInventory>().isLocked);

        otherInventory = null;
    }

    public void ToggleOtherInventoryScreen(bool toggle)
    {
        if (toggle)
        {
            UpdatePlayerInventory();
            Time.timeScale = 0f;
        }
        if (!toggle && otherInventory != null && otherInventoryObj.GetComponent<OtherInventory>().isOpened)
        {
            SaveInventory();
            otherInventoryObj.GetComponent<OtherInventory>().isOpened = false;
            Time.timeScale = 1f;
        }

        otherInventoryScreen.SetActive(toggle);
        isOpened = toggle;
    }
}
