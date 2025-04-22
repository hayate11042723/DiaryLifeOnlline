using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    [SerializeField] private GameObject itemKanriUI; // アイテム管理UI
    [SerializeField] private PlayerStatus playerStatus; // プレイヤーのステータスデータ
    [SerializeField] private Image weaponEquipMark; // 武器装備中を示すImage UI
    [SerializeField] private Image armorEquipMark;  // 防具装備中を示すImage UI

    private ItemData currentWeapon; // 現在装備している武器
    private ItemData currentArmor;  // 現在装備している防具

    void Start()
    {
        // 初期装備を設定
        SetInitialEquipment();
    }

    private void SetInitialEquipment()
    {
        // アイテム管理スクリプトを取得
        ItemKanri itemKanri = itemKanriUI.GetComponent<ItemKanri>();
        if (itemKanri == null)
        {
            Debug.LogError("ItemKanri component is not found on itemKanriUI.");
            return;
        }

        // 初期装備を取得
        var woodenSword = itemKanri.GetItemByName("Wooden Sword");
        var clothArmor = itemKanri.GetItemByName("DefaultClothes");

        // 武器を装備
        if (woodenSword != null)
        {
            EquipWeapon(woodenSword);
        }
        else
        {
            Debug.LogWarning("Initial weapon (Wooden Sword) not found.");
        }

        // 防具を装備
        if (clothArmor != null)
        {
            EquipArmor(clothArmor);
        }
        else
        {
            Debug.LogWarning("Initial armor (DefaultClothes) not found.");
        }
    }

    public void EquipSelectedItem()
    {
        // アイテム管理スクリプトを取得
        ItemKanri itemKanri = itemKanriUI.GetComponent<ItemKanri>();
        if (itemKanri == null)
        {
            Debug.LogError("ItemKanri component is not found on itemKanriUI.");
            return;
        }

        // 選択されているアイテムを取得
        var selectedItem = GetSelectedItem(itemKanri);
        if (selectedItem == null)
        {
            Debug.LogWarning("No item is selected or item is not valid.");
            return;
        }

        // 装備可能かどうかを判定
        if (selectedItem.GetItemType() == ItemData.itemtype.Sword)
        {
            EquipWeapon(selectedItem);
        }
        else if (selectedItem.GetItemType() == ItemData.itemtype.Armor)
        {
            EquipArmor(selectedItem);
        }
        else
        {
            Debug.LogWarning("Selected item is not equippable.");
        }
    }

    private ItemData GetSelectedItem(ItemKanri itemKanri)
    {
        // トグルグループから選択されているアイテムを取得
        var toggleGroup = itemKanri.GetToggleGroup();
        if (toggleGroup == null)
        {
            Debug.LogWarning("ToggleGroup is null.");
            return null;
        }

        var activeToggle = toggleGroup.ActiveToggles().FirstOrDefault();
        if (activeToggle == null)
        {
            Debug.LogWarning("No active toggle found.");
            return null;
        }

        if (int.TryParse(activeToggle.name, out int index))
        {
            var motimonoList = itemKanri.GetMotimonoList();
            if (index > 0 && index <= motimonoList.Count)
            {
                return motimonoList[index - 1];
            }
        }

        return null;
    }

    private void EquipWeapon(ItemData newWeapon)
    {
        // 既存の武器を解除
        if (currentWeapon != null)
        {
            Debug.Log($"Unequipping weapon: {currentWeapon.GetItemName()}");
            RemoveItemFromEquipment(currentWeapon);
        }

        // 新しい武器を装備
        currentWeapon = newWeapon;
        Debug.Log($"Equipped weapon: {newWeapon.GetItemName()}");

        // 装備中マークを更新
        UpdateEquipMark(weaponEquipMark, newWeapon);

        // プレイヤーのステータスを更新
        UpdatePlayerStatus();
    }

    private void EquipArmor(ItemData newArmor)
    {
        // 既存の防具を解除
        if (currentArmor != null)
        {
            Debug.Log($"Unequipping armor: {currentArmor.GetItemName()}");
            RemoveItemFromEquipment(currentArmor);
        }

        // 新しい防具を装備
        currentArmor = newArmor;
        Debug.Log($"Equipped armor: {newArmor.GetItemName()}");

        // 装備中マークを更新
        UpdateEquipMark(armorEquipMark, newArmor);

        // プレイヤーのステータスを更新
        UpdatePlayerStatus();
    }

    private void RemoveItemFromEquipment(ItemData item)
    {
        // 装備を解除する処理（必要に応じてインベントリに戻すなど）
        Debug.Log($"Removed item from equipment: {item.GetItemName()}");
    }

    private void UpdateEquipMark(Image equipMark, ItemData item)
    {
        // 装備中マークをアイテムのアイコンに重ねる
        if (equipMark != null)
        {
            equipMark.sprite = item.GetItemIcon(); // アイテムのアイコンを装備中マークに設定
            equipMark.gameObject.SetActive(true); // 装備中マークを表示
        }
    }

    private void UpdatePlayerStatus()
    {
        // プレイヤーのステータスを更新
        playerStatus.ATK = currentWeapon != null ? playerStatus.ATK * currentWeapon.GetATK() : playerStatus.ATK;
        playerStatus.DEF = currentArmor != null ? playerStatus.DEF * currentArmor.GetDFE() : playerStatus.DEF;

        Debug.Log($"Player status updated: ATK={playerStatus.ATK}, DEF={playerStatus.DEF}");
    }
}
