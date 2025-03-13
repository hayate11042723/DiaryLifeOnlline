using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentChange : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus; // プレイヤーのステータスデータ
    [SerializeField] private ItemDataBase itemDataBase; // アイテムデータベース

    // 現在の装備
    private ItemData currentWeapon;
    private ItemData currentArmor;

    // Start is called before the first frame update
    void Start()
    {
        // 初期装備を設定
        currentWeapon = itemDataBase.GetItemByName("Wooden Sword");
        currentArmor = itemDataBase.GetItemByName("Cloth Armor");

        // プレイヤーのステータスを更新
        UpdatePlayerStatus();
    }

    // 装備を変更するメソッド
    public void ChangeEquipment(string equipmentName, EquipmentType type)
    {
        // 新しい装備をデータベースから取得
        ItemData newEquipment = itemDataBase.GetItemByName(equipmentName);

        if (newEquipment != null)
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
            Debug.LogError($"Equipment {equipmentName} not found in the database.");
        }
    }

    // プレイヤーのステータスを更新するメソッド
    private void UpdatePlayerStatus()
    {
        // 現在の装備に基づいてステータスを更新
        playerStatus.ATK = currentWeapon != null ? currentWeapon.GetATK() : 0;
        playerStatus.DEF = currentArmor != null ? currentArmor.GetDFE() : 0;
        // 他のステータスも必要に応じて更新
    }
}

// 装備の種類を表す列挙型
public enum EquipmentType
{
    Weapon,
    Armor
}
