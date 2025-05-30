using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Combination
{
    public ElementData element1;
    public ElementData element2;
    public ElementData result;
}

public class ElementCombiner : MonoBehaviour
{
    public List<Combination> combinations;

    public ElementData TryCombine(ElementData a, ElementData b)
    {
        foreach (var combo in combinations)
        {
            Debug.Log($"Trying: {combo.element1.name} + {combo.element2.name}");

            if ((combo.element1 == a && combo.element2 == b) ||
                (combo.element1 == b && combo.element2 == a))
            {
                Debug.Log($"Match found: {combo.result.name}");
                return combo.result;
            }
        }
        Debug.Log("No combination matched.");
        return null;
    }

}

