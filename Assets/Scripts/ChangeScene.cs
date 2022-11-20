using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private GameObject UIMenu;
    [SerializeField] private GameObject UIOptions;
    [SerializeField] private GameObject UICredits;
    [SerializeField] private GameObject UIPause;

    public void QuitGame()
    {
        Application.Quit ();
    }

    public void LoadUI(bool isOption)
    {
        if (isOption)
        {
            UIOptions.SetActive(true);
            UICredits.SetActive(false);
            UIMenu.SetActive(false);
        }
        else
        {
            UICredits.SetActive(true);
            UIOptions.SetActive(false);
            UIMenu.SetActive(false);
        }
    }
    
    public void UnloadUI()
    {
        UICredits.SetActive(false);
        UIOptions.SetActive(false);
        UIMenu.SetActive(true);
    }
    
    public void Play()
    {
        GameManager.Instance.startCinematic();
        UIMenu.SetActive(false);
        UICredits.SetActive(false);
        UIOptions.SetActive(false);
    }

    public void LoadMenu()
    {
        UIPause.SetActive(false);
        UIMenu.SetActive(true);
        SceneManager.LoadScene("MainScene");
    }

    public void Reprendre()
    {
        UIMenu.SetActive(false);
        UICredits.SetActive(false);
        UIOptions.SetActive(false);
        UIPause.SetActive(false);
    }
}
