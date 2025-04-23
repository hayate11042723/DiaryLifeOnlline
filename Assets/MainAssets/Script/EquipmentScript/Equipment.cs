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
    [SerializeField] private StatusWindow statusWindow; // ステータスウィンドウ

    private ItemData currentWeapon; // 現在装備している武器
    private ItemData currentArmor;  // 現在装備している防具

    void Start()
    {
        // 初期装備を設定
        SetInitialEquipment();

        // 基本ステータスを初期化
        playerStatus.InitializeBaseStats();
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
            UnequipWeapon(); // 現在の武器を外す
        }

        // 新しい武器を装備
        currentWeapon = newWeapon;
        Debug.Log($"Equipped weapon: {newWeapon.GetItemName()}");

        // 装備中マークを更新
        UpdateEquipMark(weaponEquipMark, newWeapon);

        // プレイヤーのステータスをリセット
        ResetPlayerStatus();
    }

    private void EquipArmor(ItemData newArmor)
    {
        // 既存の防具を解除
        if (currentArmor != null)
        {
            UnequipArmor(); // 現在の防具を外す
        }

        // 新しい防具を装備
        currentArmor = newArmor;
        Debug.Log($"Equipped armor: {newArmor.GetItemName()}");

        // 装備中マークを更新
        UpdateEquipMark(armorEquipMark, newArmor);

        // プレイヤーのステータスをリセット
        ResetPlayerStatus();
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
            // アイコンの位置を取得
            var itemIcon = FindItemIcon(item);
            if (itemIcon != null)
            {
                // 装備中マークをアイコンの位置に移動
                RectTransform equipMarkRect = equipMark.GetComponent<RectTransform>();
                RectTransform itemIconRect = itemIcon.GetComponent<RectTransform>();

                if (equipMarkRect != null && itemIconRect != null)
                {
                    equipMarkRect.SetParent(itemIconRect, false); // アイコンの子要素に設定
                    equipMarkRect.anchoredPosition = Vector2.zero; // アイコンの中央に配置
                    equipMarkRect.sizeDelta = itemIconRect.sizeDelta; // アイコンと同じサイズに設定
                }

                equipMark.gameObject.SetActive(true); // 装備中マークを表示
            }
        }
    }

    private GameObject FindItemIcon(ItemData item)
    {
        // アイテムのアイコンを検索
        ItemKanri itemKanri = itemKanriUI.GetComponent<ItemKanri>();
        if (itemKanri == null)
        {
            Debug.LogError("ItemKanri component is not found on itemKanriUI.");
            return null;
        }

        var icons = itemKanri.GetIcons(); // アイコンのリストを取得
        for (int i = 0; i < icons.Length; i++)
        {
            if (icons[i] != null && icons[i].sprite == item.GetItemIcon())
            {
                return icons[i].gameObject; // 該当するアイコンを返す
            }
        }

        return null; // 該当するアイコンが見つからない場合
    }


    private void UpdatePlayerStatus()
    {
        // プレイヤーのステータスを更新
        playerStatus.ATK = currentWeapon != null ? playerStatus.BaseATK * currentWeapon.GetATK() : playerStatus.BaseATK;
        playerStatus.DEF = currentArmor != null ? playerStatus.BaseDEF * currentArmor.GetDFE() : playerStatus.BaseDEF;

        Debug.Log($"Player status updated: ATK={playerStatus.ATK}, DEF={playerStatus.DEF}");

        // ステータスウィンドウを更新
        if (statusWindow != null)
        {
            statusWindow.UpdateUI();
        }
    }

    private void UnequipWeapon()
    {
        if (currentWeapon != null)
        {
            Debug.Log($"Unequipping weapon: {currentWeapon.GetItemName()}");
            RemoveItemFromEquipment(currentWeapon);
            currentWeapon = null;

            // ステータスをリセット
            ResetPlayerStatus();

            weaponEquipMark.gameObject.SetActive(false); // 装備中マークを非表示
        }
    }

    private void UnequipArmor()
    {
        if (currentArmor != null)
        {
            Debug.Log($"Unequipping armor: {currentArmor.GetItemName()}");
            RemoveItemFromEquipment(currentArmor);
            currentArmor = null;

            // ステータスをリセット
            ResetPlayerStatus();

            armorEquipMark.gameObject.SetActive(false); // 装備中マークを非表示
        }
    }

    private void ResetWeaponStats()
    {
        // 武器によるステータス上昇値をリセット
        playerStatus.ATK = playerStatus.BaseATK; // BaseATKは元のATK値
    }

    private void ResetArmorStats()
    {
        // 防具によるステータス上昇値をリセット
        playerStatus.DEF = playerStatus.BaseDEF; // BaseDEFは元のDEF値
    }

    private void ResetPlayerStatus()
    {
        // 武器が装備されていない場合、基本値にリセット
        playerStatus.ATK = currentWeapon != null ? playerStatus.BaseATK + currentWeapon.GetATK() : playerStatus.BaseATK;

        // 防具が装備されていない場合、基本値にリセット
        playerStatus.DEF = currentArmor != null ? playerStatus.BaseDEF + currentArmor.GetDFE() : playerStatus.BaseDEF;

        Debug.Log($"Player status reset: ATK={playerStatus.ATK}, DEF={playerStatus.DEF}");

        // ステータスウィンドウを更新
        if (statusWindow != null)
        {
            statusWindow.UpdateUI();
        }
    }

}
