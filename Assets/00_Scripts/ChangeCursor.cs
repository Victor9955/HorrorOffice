using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeCursor : MonoBehaviour
{
    [SerializeField] CameraMovement cameraMovement;
    [SerializeField] Texture2D hoverUI;
    [SerializeField] Texture2D normalUI;
    [SerializeField] Texture2D outsideUI;
    private int UILayer;

    private void Start()
    {
        UILayer = LayerMask.NameToLayer("UI");
    }

    private void Update()
    {
        if(cameraMovement.focusState == FocusState.PC)
        {
            IsPointerOverUIElement(GetEventSystemRaycastResults());
        }
        else
        {
            Cursor.SetCursor(outsideUI, Vector2.zero, CursorMode.Auto);
        }
    }

    private void IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        if(eventSystemRaysastResults.Count == 0)
        {
            Cursor.SetCursor(outsideUI, Vector2.zero, CursorMode.Auto);
            return;
        }
        int uiAmount = 0;
        int notUIAmount = 0;
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
            {
                if(curRaysastResult.gameObject.TryGetComponent(out Button button) || curRaysastResult.gameObject.TryGetComponent(out InputField inputField) || curRaysastResult.gameObject.name == "Handle")
                {
                    uiAmount++;
                }
            }
            else
            {
                notUIAmount++;
            }
        }
        if(notUIAmount > 0) return;
        if(uiAmount == 0)
        {
            Cursor.SetCursor(normalUI, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(hoverUI, Vector2.zero, CursorMode.Auto);
        }
    }

    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
