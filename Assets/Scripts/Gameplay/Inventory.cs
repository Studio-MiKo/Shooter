using System.Collections.Generic;

public class Inventory 
{
    public List<Item> items = new List<Item>();

    public void AddItem(Item item) 
    {
        bool found = false;

        foreach (var invItem in items) 
        {
            if (invItem.name == item.name && invItem.type == item.type) 
            {
                invItem.quantity += item.quantity;
                found = true;
                break;
            }
        }
        if (!found) 
        {
            items.Add(item);
        }
    }

    public void RemoveItem(string itemName, int quantity) 
    {
        foreach (var item in items) 
        {
            if (item.name == itemName) 
            {
                item.quantity -= quantity;

                if (item.quantity <= 0) 
                {
                    items.Remove(item);
                }
                
                return;
            }
        }
    }
}