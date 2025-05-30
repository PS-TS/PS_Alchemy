using UnityEngine;

[CreateAssetMenu(fileName = "NewElement", menuName = "Alchemy/Element")]
public class ElementData : ScriptableObject
{
    public string elementName;
    public Sprite icon;
}
