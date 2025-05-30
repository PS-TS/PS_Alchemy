using UnityEngine;

public class ElementCollision : MonoBehaviour
{
    private DraggableElement draggable;

    private void Awake()
    {
        draggable = GetComponent<DraggableElement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var otherDraggable = other.GetComponent<DraggableElement>();
        if (otherDraggable != null && draggable != otherDraggable)
        {
            FindFirstObjectByType<AlchemyManager>().TryCreateCombination(draggable, otherDraggable);
        }
    }
}

