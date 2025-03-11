using UnityEngine;
using UnityEngine.EventSystems;

public class PointerClickTest : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Pointer clicked: " + eventData.pointerPress.name);
    }
}
