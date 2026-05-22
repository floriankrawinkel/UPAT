using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "VisualDelegate/Collection", fileName = "New Visual Delegate Collection")]
public class VisualDelegateCollection : ScriptableObject
{
    [Header("Collection Info")]
    public string collectionName;

    [TextArea]
    public string description;

    [Header("Visual Delegates")]
    public List<VisualDelegate> visualDelegates = new();
}
