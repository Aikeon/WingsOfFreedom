using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private GameObject UIMenu;
    [SerializeField] private GameObject UIOptions;
    [SerializeField] private GameObject UICredits;
    [SerializeField] private GameObject UIPause;

    [SerializeField] Transform menuCam;
    [SerializeField] AnimationCurve yCurve;
    [SerializeField] GameObject icare;
    public static bool seeing;

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
        UIMenu.SetActive(false);
        UICredits.SetActive(false);
        UIOptions.SetActive(false);
        StartCoroutine(startCinematic());
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

    IEnumerator startCinematic()
    {
        var timeEllapsed = 0f;
        while (timeEllapsed < yCurve.keys[yCurve.keys.Length-1].time)
        {
            menuCam.position = new Vector3(0,yCurve.Evaluate(timeEllapsed),-22.5f);
            timeEllapsed += Time.deltaTime;
            yield return null;
        }
        menuCam.position = new Vector3(0,27.5f, -22.5f);
        var timeEllapsed2 = 0f;
        icare.GetComponent<Animator>().Play("Walk");
        while (timeEllapsed < 19f)
        {
            icare.transform.position += Vector3.forward * Time.deltaTime;
            timeEllapsed2 += Time.deltaTime;
            yield return null;
        }
        icare.GetComponent<Animator>().Play("Idle");
        seeing = true;
    }
}
