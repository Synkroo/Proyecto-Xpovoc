using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusUISwitcher : MonoBehaviour
{
    public static StatusUISwitcher Instance;

    [Serializable]
    public class Entry
    {
        public string characterName;
        public GameObject panel;
    }

    public GameObject defaultPanel;
    public List<Entry> panels = new();

    private readonly Dictionary<string, GameObject> map = new();
    private GameObject current;

    private void Awake()
    {
        Instance = this;

        map.Clear();
        foreach (var e in panels)
        {
            if (e != null && !string.IsNullOrEmpty(e.characterName) && e.panel != null)
                map[e.characterName] = e.panel;
        }
    }

    private void OnEnable()
    {
        ShowDefault();
    }

    public void Show(string characterName)
    {
        if (current != null) current.SetActive(false);

        if (!string.IsNullOrEmpty(characterName) && map.TryGetValue(characterName, out var next) && next != null)
        {
            current = next;
            current.SetActive(true);
        }
        else
        {
            ShowDefault();
        }
    }

    public void ShowDefault()
    {
        if (defaultPanel == null) return;

        if (current != null) current.SetActive(false);
        current = defaultPanel;
        current.SetActive(true);
    }
}