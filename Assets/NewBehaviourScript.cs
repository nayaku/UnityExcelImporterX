using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public MsItems MsItems;

    void Start()
    {
        foreach (var item in MsItems.itemDict)
        {
            Debug.Log($"Item Key: {item.Key}, Item Name: {item.Value.name}, Item Category: {item.Value.category}");
        }
    }
}
