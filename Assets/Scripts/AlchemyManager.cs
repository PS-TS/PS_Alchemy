using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class AlchemyManager : MonoBehaviour
{
    public ElementCombiner combiner;
    public Transform buttonPanel;
    public GameObject buttonPrefab;
    public GameObject elementPrefab;
    public GameObject winTab;
    public GameObject startMenu;
    public Transform contentTransform;
    public TMP_Text playerProgressText;

    public List<ElementData> allElements;
    public List<ElementData> startingElements;

    private HashSet<ElementData> discoveredElements = new HashSet<ElementData>();
    private HashSet<ElementData> createdButtons = new HashSet<ElementData>();

    public int discoveredCount => discoveredElements.Count-4;
    public int totalCount => allElements.Count-4;

    void Start()
    {
        foreach (var element in startingElements)
        {
            UnlockElement(element);
        }

        UpdateAvailableButtons();
        UpdateProgressUI();
    }

    public void UnlockElement(ElementData element)
    {
        if (discoveredElements.Contains(element))
            return;

        discoveredElements.Add(element);
        CreateButtonIfNeeded(element);
        UpdateProgressUI();

        if (discoveredCount >= totalCount)
        {
            TriggerWin();
        }
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
        GameObject el = Instantiate(elementPrefab, contentTransform);
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

            GameObject newElement = Instantiate(elementPrefab, contentTransform);
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
        foreach (var element in discoveredElements)
        {
            CreateButtonIfNeeded(element);
        }
    }

    public void UpdateProgressUI()
    {
        if (playerProgressText != null)
        {
            playerProgressText.text = $"Odkryte: {discoveredCount} / {totalCount}";
        }
    }

    public void TriggerWin()
    {
        if (winTab != null)
        {
            winTab.SetActive(true);
        }
    }
    public void StartGame()
    {
        if (startMenu != null)
        {
            startMenu.SetActive(false);
        }
    }
    public void PauseGame()
    {
        if (startMenu != null)
        {
            startMenu.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}
