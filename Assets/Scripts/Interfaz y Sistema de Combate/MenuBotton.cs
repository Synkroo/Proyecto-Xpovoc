using UnityEngine;

public class MenuBotton : MonoBehaviour
{
    public GameObject menuQueAbre;
    public GameObject[] menusQueCierran;

    public void ToggleMenu()
    {

        if (menuQueAbre == null) return;

        bool isActive = !menuQueAbre.activeSelf;

        foreach (GameObject m in menusQueCierran)
        {
            if (m != null)
                m.SetActive(false);
        }

        menuQueAbre.SetActive(isActive);
    }
}
