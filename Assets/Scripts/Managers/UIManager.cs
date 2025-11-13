using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    private Dictionary<string, GameObject> _uiElements = new Dictionary<string, GameObject>();

    public void RegisterElement(string name, GameObject element)
    {
        if (!_uiElements.ContainsKey(name))
            _uiElements[name] = element;
    }

        public void SetUIElement(string name, bool isActive)
    {
        if (_uiElements.TryGetValue(name, out var element))
            element.SetActive(isActive);
        else
            Debug.LogWarning($"UIManager: Elemento '{name}' não registrado.");
    }
}
