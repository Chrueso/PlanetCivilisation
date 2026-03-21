using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TabGroup : MonoBehaviour
{
    [SerializeField] private int pageIndex = 0;

    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private List<Toggle> tabs = new List<Toggle>();
    [SerializeField] private List<CanvasGroup> pages = new List<CanvasGroup>();

    public event Action<int> OnPageIndexChanged;

    private void Init()
    {
        toggleGroup = GetComponentInChildren<ToggleGroup>();    

        tabs.Clear();
        pages.Clear();

        tabs.AddRange(GetComponentsInChildren<Toggle>());
        pages.AddRange(GetComponentsInChildren<CanvasGroup>());
    }

    private void Reset()
    {
        Init();
    }

    private void OnValidate()
    {
        Init();
        OpenPage(pageIndex);
        tabs[pageIndex].SetIsOnWithoutNotify(true);
    }

    private void Awake()
    {
        foreach (var toggle in tabs)
        {
            toggle.onValueChanged.AddListener(CheckForTab);
            toggle.group = toggleGroup;
        }
    }

    private void OnDestroy()
    {
        foreach (var toggle in tabs)
        {
            toggle.onValueChanged.RemoveListener(CheckForTab);
        }
    }

    private void CheckForTab(bool value)
    {
        for (int i = 0; i < tabs.Count; i++)
        {
            if (!tabs[i].isOn) continue;
            pageIndex = i;
        }

        OpenPage(pageIndex);
    }

    private void OpenPage(int index)
    {
        EnsureIndexIsRange(index);

        for (int i = 0; i < pages.Count; i ++)
        {
            bool isActivePage = (i == pageIndex);

            pages[i].alpha = isActivePage ? 1 : 0;
            pages[i].interactable = isActivePage;
            pages[i].blocksRaycasts = isActivePage;
        }

        if (Application.isPlaying) OnPageIndexChanged?.Invoke(pageIndex);
    }

    private void EnsureIndexIsRange(int index)
    {
        if (tabs.Count == 0 || pages.Count == 0)
        {
            Debug.Log("No tabs or pages!");
            return;
        }

        pageIndex = Mathf.Clamp(index, 0, pages.Count - 1);
    }

    public void JumpToPage(int page)
    {
        EnsureIndexIsRange(page);

        tabs[pageIndex].isOn = true;
    }

}
