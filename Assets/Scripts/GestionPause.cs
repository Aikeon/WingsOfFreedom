using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionPause : MonoBehaviour
{
    [SerializeField] private GameObject UIPause;
    [SerializeField] private GameObject UIMenu;
    [SerializeField] private GameObject UIOptions;
    [SerializeField] private GameObject UICredits;

    private int compteEsc = 0;
    private void Pause()
    {
        UIPause.SetActive(true);
    }
    
    private void Reprendre()
    {
        UIPause.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !UIMenu.activeInHierarchy && !UICredits.activeInHierarchy && !UIOptions.activeInHierarchy)
        {
            compteEsc += 1;
            if (compteEsc % 2 == 1)
            {
                Pause();
            }
            else
            {
                Reprendre();
            }
        }
    }
}
