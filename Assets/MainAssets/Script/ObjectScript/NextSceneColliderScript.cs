using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NextSceneColliderScript : MonoBehaviour
{
    [SerializeField] GameObject NextSceneNamePanel;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NextSceneNamePanel.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //PanelÇîÒï\é¶Ç…Ç∑ÇÈÅB
            NextSceneNamePanel.SetActive(false);
        }
    }
}
