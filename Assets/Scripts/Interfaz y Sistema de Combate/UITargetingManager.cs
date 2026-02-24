using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UITargetingManager : MonoBehaviour
{
    public static UITargetingManager Instance;

    public bool isTargeting;
    public ITargetable currentHover;

    private Action<ITargetable> onTargetSelected;
    private List<ITargetable> validTargetsUI;

    private GraphicRaycaster[] uiRaycasters;
    private EventSystem eventSystem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
        SetupReferences();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupReferences();
        StopTargeting();
    }

    private void SetupReferences()
    {
        eventSystem = EventSystem.current;
        uiRaycasters = FindObjectsByType<GraphicRaycaster>(FindObjectsSortMode.None);
    }

    private void Update()
    {
        if (!isTargeting) return;

        HandleHover();
        HandleClick();
    }

    public void StartTargeting(Action<ITargetable> onTargetSelected, List<ITargetable> uiTargets = null)
    {
        this.onTargetSelected = onTargetSelected;
        this.validTargetsUI = uiTargets;
        isTargeting = true;
    }

    private void HandleHover()
    {
        if (eventSystem == null || uiRaycasters == null)
        {
            return;
        }

        ITargetable nextHover = null;

        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        foreach (var raycaster in uiRaycasters)
        {
            if (raycaster == null || !raycaster.isActiveAndEnabled)
            {
                continue;
            }

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);


            foreach (var result in results)
            {
                var target = result.gameObject.GetComponent<ITargetable>();
                if (target != null)
                {
                    if (validTargetsUI == null || validTargetsUI.Contains(target))
                    {
                        nextHover = target;
                        break;
                    }
                }
                else
                {
                }
            }

            if (nextHover != null)
                break;
        }

        if (nextHover != currentHover)
        {
            currentHover?.OnHoverExit();
            currentHover = nextHover;
            currentHover?.OnHoverEnter();
        }
    }

    private void HandleClick()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        if (currentHover != null)
        {
            onTargetSelected?.Invoke(currentHover);
        }

        StopTargeting();
    }

    public void StopTargeting()
    {
        currentHover?.OnHoverExit();
        currentHover = null;
        isTargeting = false;
        onTargetSelected = null;
        validTargetsUI = null;
    }
}