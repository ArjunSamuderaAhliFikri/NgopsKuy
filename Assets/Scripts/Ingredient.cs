using System;

[Serializable]

public class Ingredient
{
    public string ingredientName;
    public int quantity;

    public Ingredient(string name, int quantity)
    {
        this.ingredientName = name;
        this.quantity = quantity;
    }
}