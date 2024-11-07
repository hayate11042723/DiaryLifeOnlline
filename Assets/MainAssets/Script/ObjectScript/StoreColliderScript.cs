using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StoreColliderScript : MonoBehaviour
{
    [SerializeField] GameObject storeName;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            storeName.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //PanelÇîÒï\é¶Ç…Ç∑ÇÈÅB
            storeName.SetActive(false);
        }
    }
}
