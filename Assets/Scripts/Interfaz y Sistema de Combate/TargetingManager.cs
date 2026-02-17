using System;
using UnityEngine;

public class TargetingManager : MonoBehaviour
{
    public static TargetingManager Instance;
    public bool isTargeting;
    public ITargetable currentHover;

    private Action<ITargetable> onTargetSelected;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!isTargeting)
        {
            return;
        }
        HandleHover();
        HandleClick();
    }

    void HandleHover()
    {
        ITargetable nextHover = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            BaseEntity entity = hit.collider.GetComponentInParent<BaseEntity>();

            if (entity != null && entity.isEnemy)
            {
                nextHover = entity;
            }
        }

        if (nextHover != currentHover)
        {
            currentHover?.OnHoverExit();
            currentHover = nextHover;
            currentHover?.OnHoverEnter();
        }
    }


    void HandleClick()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (currentHover == null)
        {
            StopTargeting();
            return;
        }

        onTargetSelected?.Invoke(currentHover);
        StopTargeting();
    }

    public void StartTargeting(Action<ITargetable> onTargetSelected)
    {
        this.onTargetSelected = onTargetSelected;
        isTargeting = true;
    }

    public void StopTargeting()
    {
        currentHover?.OnHoverExit();
        currentHover = null;
        isTargeting = false;
        onTargetSelected = null;
    }

}
