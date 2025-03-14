using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentChange : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus; // プレイヤーのステータスデータ
    [SerializeField] private GameObject itemKanriUI; // アイテム管理UI
    [SerializeField] private Canvas parentCanvas; // 親要素のCanvas

    // 現在の装備
    private ItemData currentWeapon;
    private ItemData currentArmor;

    // Start is called before the first frame update
    void Start()
    {
        // itemKanriUIが設定されているか確認
        if (itemKanriUI == null)
        {
            Debug.LogError("itemKanriUI is not assigned.");
            return;
        }

        // ItemKanriコンポーネントを取得
        ItemKanri itemKanri = itemKanriUI.GetComponent<ItemKanri>();
        if (itemKanri == null)
        {
            Debug.LogError("ItemKanri component is not found on itemKanriUI.");
            return;
        }

        // 初期装備を設定
        currentWeapon = itemKanri.GetItemByName("Wooden Sword");
        currentArmor = itemKanri.GetItemByName("DefaultClothes");

        // プレイヤーのステータスを更新
        UpdatePlayerStatus();

        // 親要素のCanvasの状態を監視
        StartCoroutine(MonitorCanvasState());
    }

    // 装備を変更するメソッド
    public void ChangeEquipment(string equipmentName, EquipmentType type)
    {
        // ItemKanriコンポーネントを取得
        ItemKanri itemKanri = itemKanriUI.GetComponent<ItemKanri>();
        if (itemKanri == null)
        {
            Debug.LogError("ItemKanri component is not found on itemKanriUI.");
            return;
        }

        // 新しい装備をインベントリから取得
        ItemData newEquipment = itemKanri.GetItemByName(equipmentName);

        if (newEquipment != null && itemKanri.HasItem(newEquipment))
        {
            // 装備の種類に応じて現在の装備を更新
            if (type == EquipmentType.Weapon)
            {
                currentWeapon = newEquipment;
            }
            else if (type == EquipmentType.Armor)
            {
                currentArmor = newEquipment;
            }

            // プレイヤーのステータスを更新
            UpdatePlayerStatus();
        }
        else
        {
            Debug.LogError($"Equipment {equipmentName} not found in the inventory.");
        }
    }

    // プレイヤーのステータスを更新するメソッド
    private void UpdatePlayerStatus()
    {
        // 現在の装備に基づいてステータスを更新
        playerStatus.ATK = currentWeapon != null ? playerStatus.ATK * currentWeapon.GetATK() : playerStatus.ATK;
        playerStatus.DEF = currentArmor != null ? playerStatus.DEF * currentArmor.GetDFE() : playerStatus.DEF;
        // 他のステータスも必要に応じて更新
    }

    // 親要素のCanvasの状態を監視するコルーチン
    private IEnumerator MonitorCanvasState()
    {
        while (true)
        {
            if (parentCanvas.gameObject.activeSelf)
            {
                // Canvasがアクティブな間、インベントリUIを子要素にする
                itemKanriUI.transform.SetParent(parentCanvas.transform, false);
            }
            else
            {
                // Canvasが非アクティブになったら、インベントリUIを元の親要素に戻す
                itemKanriUI.transform.SetParent(null);
            }
            yield return new WaitForSeconds(0.1f); // 0.1秒ごとにチェック
        }
    }
}

// 装備の種類を表す列挙型
public enum EquipmentType
{
    Weapon,
    Armor
}