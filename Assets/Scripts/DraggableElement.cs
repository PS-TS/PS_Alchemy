using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ElementData elementData;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvas = FindFirstObjectByType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        DraggableElement[] allElements = Object.FindObjectsByType<DraggableElement>(FindObjectsSortMode.None);

        foreach (DraggableElement other in allElements)
        {
            if (other == this)
                continue;

            if (AreRectsOverlapping(rectTransform, other.rectTransform))
            {
                AlchemyManager manager = FindFirstObjectByType<AlchemyManager>();
                if (manager != null)
                {
                    manager.TryCreateCombination(this, other);
                    return;
                }
            }
        }
    }

    private bool AreRectsOverlapping(RectTransform rect1, RectTransform rect2)
    {
        Rect r1 = GetScreenRect(rect1);
        Rect r2 = GetScreenRect(rect2);
        return r1.Overlaps(r2);
    }

    private Rect GetScreenRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];
        return new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
    }

}

