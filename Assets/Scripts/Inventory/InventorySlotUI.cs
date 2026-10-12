using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InventorySlotUI : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text quantityText;

    public void UpdateSlot(ItemData data, int quantity)
    {
        Debug.Log("Item Data : " + data + " Quantity is : " + quantity);

        if (data == null || quantity <= 0)
        {
            iconImage.enabled = false;
            quantityText.text = "0";

            return;
        }

        iconImage.enabled = true;
        iconImage.sprite = data.icon;
        quantityText.text = quantity.ToString();
    }
}
