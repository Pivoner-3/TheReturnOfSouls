using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [Header("Ресурсы для крафта")]
    public string rustySword1Name = "Ржавый меч (вид 1)";
    public int rustySword1Amount = 50;

    public string rustySword2Name = "Ржавый меч (вид 2)";
    public int rustySword2Amount = 30;

    public string rustySword3Name = "Ржавый меч (вид 3)";
    public int rustySword3Amount = 20;

    [Header("Результат крафта")]
    public ItemData resultSword;

    public bool CanCraft()
    {
        int count1 = InventoryManager.Instance.GetItemCount(rustySword1Name);
        int count2 = InventoryManager.Instance.GetItemCount(rustySword2Name);
        int count3 = InventoryManager.Instance.GetItemCount(rustySword3Name);

        return count1 >= rustySword1Amount &&
               count2 >= rustySword2Amount &&
               count3 >= rustySword3Amount;
    }

    public void Craft()
    {
        if (!CanCraft())
        {
            Debug.Log("Недостаточно ресурсов для крафта!");
            return;
        }

        InventoryManager.Instance.RemoveMultipleItems(rustySword1Name, rustySword1Amount);
        InventoryManager.Instance.RemoveMultipleItems(rustySword2Name, rustySword2Amount);
        InventoryManager.Instance.RemoveMultipleItems(rustySword3Name, rustySword3Amount);

        InventoryManager.Instance.AddItem(resultSword);

        Debug.Log($"Скрафчен: {resultSword.itemName}!");
    }
}