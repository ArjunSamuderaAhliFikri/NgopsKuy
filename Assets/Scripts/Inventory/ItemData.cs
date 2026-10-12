using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "NgopsKuy/Inventory Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    public bool stackable = true;
    public int maxStack = 99;
}
