
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Serializable]
    public class InventoryEntry
    {
        public ItemData item;
        public int quantity;
    }

    [Header("UI")]
    public GameObject inventoryPanel;
    public InventorySlotUI[] slots;

    [Header("Starting Items")]
    public List<InventoryEntry> startingItems =
        new List<InventoryEntry>();

    private List<InventoryEntry> items =
        new List<InventoryEntry>();

    private void Start()
    {
        inventoryPanel.SetActive(false);

        foreach (InventoryEntry entry in startingItems)
        {
            AddItem(entry.item, entry.quantity);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(
                !inventoryPanel.activeSelf
            );
        }
    }

    public bool AddItem(ItemData item, int amount = 1)
    {
        if (item == null || amount <= 0)
            return false;

        // Tambahkan ke tumpukan item yang sudah ada.
        if (item.stackable)
        {
            foreach (InventoryEntry entry in items)
            {
                if (entry.item == item &&
                    entry.quantity < item.maxStack)
                {
                    int available =
                        item.maxStack - entry.quantity;

                    int added = Mathf.Min(amount, available);

                    entry.quantity += added;
                    amount -= added;

                    if (amount == 0)
                    {
                        RefreshUI();
                        return true;
                    }
                }
            }
        }

        // Masukkan sisa item ke slot kosong.
        while (amount > 0)
        {
            if (items.Count >= slots.Length)
            {
                RefreshUI();
                return false;
            }

            int added = item.stackable
                ? Mathf.Min(amount, item.maxStack)
                : 1;

            items.Add(new InventoryEntry
            {
                item = item,
                quantity = added
            });

            amount -= added;
        }

        RefreshUI();
        return true;
    }

    public bool RemoveItem(ItemData item, int amount = 1)
    {
        if (item == null || amount <= 0)
            return false;

        int total = 0;

        foreach (InventoryEntry entry in items)
        {
            if (entry.item == item)
                total += entry.quantity;
        }

        if (total < amount)
            return false;

        int remaining = amount;

        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i].item != item)
                continue;

            int removed = Mathf.Min(
                remaining,
                items[i].quantity
            );

            items[i].quantity -= removed;
            remaining -= removed;

            if (items[i].quantity <= 0)
                items.RemoveAt(i);

            if (remaining == 0)
                break;
        }

        RefreshUI();
        return true;
    }

    public int GetQuantity(ItemData item)
    {
        int total = 0;

        foreach (InventoryEntry entry in items)
        {
            if (entry.item == item)
                total += entry.quantity;
        }

        return total;
    }

    private void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count)
            {
                slots[i].UpdateSlot(
                    items[i].item,
                    items[i].quantity
                );
            }
            else
            {
                slots[i].UpdateSlot(null, 0);
            }
        }
    }
}
