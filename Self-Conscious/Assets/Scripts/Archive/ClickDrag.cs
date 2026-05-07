using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ClickDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Start drag");
        Cursor.visible = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        transform.position = Mouse.current.position.ReadValue();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End drag");
        Cursor.visible = true;
    }
}
