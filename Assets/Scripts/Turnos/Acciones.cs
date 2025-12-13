using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Acciones : MonoBehaviour
{
    public static Acciones Instance;

    [Header("Botones principales")]
    public Button btnAtaque;
    public Button btnDefensa;
    public Button btnObjeto;
    public Button btnHuir;

    [HideInInspector]
    public bool actionSelected = false;

    private System.Action selectedAction;

    private BaseEntity currentEntity;

    void Awake() => Instance = this;

    void Start()
    {
        GetComponent<Button>("ataquebasico");
        btnDefensa.onClick.AddListener(() => SelectAction(DefendAction));
        btnHuir.onClick.AddListener(() => SelectAction(RunAction));
    }

    public void Show(BaseEntity entity)
    {
        currentEntity = entity;
        actionSelected = false;

        gameObject.SetActive(true);
    }

    private void SelectAction(System.Action action)
    {
        selectedAction = action;
        actionSelected = true;

        gameObject.SetActive(false);
    }

    public IEnumerator PerformSelectedAction()
    {
        selectedAction?.Invoke();
        yield return null;
    }

    private void AttackAction()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            var target = enemies[Random.Range(0, enemies.Length)].GetComponent<BaseEntity>();
            target.AddStat(StatsEnum.Health, -10);
            Debug.Log($"{currentEntity.entityName} atacó a {target.entityName}");
        }
    }

    private void DefendAction()
    {
        Debug.Log($"{currentEntity.entityName} se defiende");
    }

    private void ItemAction()
    {
        Debug.Log($"{currentEntity.entityName} usa un objeto");
    }

    private void RunAction()
    {
        Debug.Log($"{currentEntity.entityName} intenta huir");
    }
}
