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
    public TargetableEntity currentHover;

    private Action<TargetableEntity> onTargetSelected;
    private List<TargetableEntity> validTargetsUI;

    private GraphicRaycaster[] uiRaycasters;
    private EventSystem eventSystem;

    // Excepción controlada (solo Status)
    private bool keepTargetingAfterClick;

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

    // MODO NORMAL (inventario, etc.) -> se para al click
    public void StartTargeting(Action<TargetableEntity> onTargetSelected, List<TargetableEntity> uiTargets = null)
    {
        SetupReferences();

        this.onTargetSelected = onTargetSelected;
        this.validTargetsUI = uiTargets;

        keepTargetingAfterClick = false;
        isTargeting = true;
    }

    // MODO STATUS (excepción) -> NO se para al click
    public void StartTargetingForStatus(List<TargetableEntity> uiTargets = null)
    {
        SetupReferences();

        onTargetSelected = null;
        validTargetsUI = uiTargets;

        keepTargetingAfterClick = true;
        isTargeting = true;
    }

    private void HandleHover()
    {
        if (eventSystem == null || uiRaycasters == null) return;

        TargetableEntity nextHover = null;

        var pointerData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        foreach (var raycaster in uiRaycasters)
        {
            if (raycaster == null || !raycaster.isActiveAndEnabled)
                continue;

            var results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            foreach (var result in results)
            {
                var target = result.gameObject.GetComponentInParent<TargetableEntity>();
                if (target == null) continue;

                if (validTargetsUI == null || validTargetsUI.Contains(target))
                {
                    nextHover = target;
                    break;
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
            currentHover.OnSelected();
            onTargetSelected?.Invoke(currentHover);
        }

        if (!keepTargetingAfterClick)
            StopTargeting();
    }

    public void StopTargeting()
    {
        currentHover?.OnHoverExit();
        currentHover = null;

        isTargeting = false;
        onTargetSelected = null;
        validTargetsUI = null;

        keepTargetingAfterClick = false;
    }
}