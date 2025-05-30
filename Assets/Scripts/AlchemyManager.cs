using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlchemyManager : MonoBehaviour
{
    public ElementCombiner combiner;
    public Transform buttonPanel;
    public GameObject buttonPrefab;
    public GameObject elementPrefab;
    public Transform canvasTransform;

    private HashSet<ElementData> discoveredElements = new HashSet<ElementData>();
    private HashSet<ElementData> createdButtons = new HashSet<ElementData>();


    public List<ElementData> startingElements;

    void Start()
    {
        foreach (var element in startingElements)
        {
            UnlockElement(element);
        }
        UpdateAvailableButtons();
    }

    public void UnlockElement(ElementData element)
    {
        if (discoveredElements.Contains(element))
            return;

        discoveredElements.Add(element);
        CreateButtonIfNeeded(element);
    }

    public void CreateButtonIfNeeded(ElementData element)
    {
        if (createdButtons.Contains(element))
            return;

        GameObject btn = Instantiate(buttonPrefab, buttonPanel);

        btn.GetComponentInChildren<TMP_Text>().text = element.elementName;

        Transform iconTransform = btn.transform.Find("Icon");
        if (iconTransform != null)
        {
            Image iconImage = iconTransform.GetComponent<Image>();
            if (iconImage != null)
            {
                iconImage.sprite = element.icon;
                iconImage.enabled = element.icon != null;
            }
        }

        btn.GetComponent<Button>().onClick.AddListener(() => SpawnElement(element));
        createdButtons.Add(element);
    }


    public void SpawnElement(ElementData element)
    {
        GameObject el = Instantiate(elementPrefab, canvasTransform);
        DraggableElement draggable = el.GetComponent<DraggableElement>();
        draggable.elementData = element;

        Image image = el.GetComponent<Image>();
        if (image != null && element.icon != null)
        {
            image.sprite = element.icon;
            image.enabled = true;
        }
    }



    public void TryCreateCombination(DraggableElement a, DraggableElement b)
    {
        ElementData result = combiner.TryCombine(a.elementData, b.elementData);
        if (result != null)
        {
            UnlockElement(result);
            UpdateAvailableButtons();

            Vector3 spawnPos = (a.transform.position + b.transform.position) / 2;

            Destroy(a.gameObject);
            Destroy(b.gameObject);

            GameObject newElement = Instantiate(elementPrefab, canvasTransform);
            DraggableElement draggable = newElement.GetComponent<DraggableElement>();
            draggable.elementData = result;
            newElement.transform.position = spawnPos;

            Image image = newElement.GetComponent<Image>();
            if (image != null && result.icon != null)
            {
                image.sprite = result.icon;
                image.enabled = true;
            }
        }
    }

    public void UpdateAvailableButtons()
    {
        foreach (var combo in combiner.combinations)
        {
            var result = combo.result;
            bool bothKnown = discoveredElements.Contains(combo.element1) && discoveredElements.Contains(combo.element2);

            if (bothKnown)
            {
                CreateButtonIfNeeded(result);
            }
        }
    }
}
