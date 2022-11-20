using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    [SerializeField] AnimationCurve yCurve;
    [SerializeField] Transform menuCam;
    [SerializeField] Transform gameCam;
    [SerializeField] GameObject lueur;
    [SerializeField] GameObject icare;
    public bool seeing;
    [SerializeField] string[] dialogs;
    [SerializeField] TextMeshProUGUI manText;

    public static GameManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    public void startCinematic()
    {
        StartCoroutine(cinematic());
    }

    IEnumerator cinematic()
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
        icare.transform.eulerAngles = new Vector3(0,180,0);
        while (icare.transform.position.z < -6)
        {
            icare.transform.position += Vector3.forward * Time.deltaTime;
            timeEllapsed2 += Time.deltaTime;
            yield return null;
        }
        icare.GetComponent<Animator>().Play("Idle");
        seeing = true;

        int i = 0;
        while (i < dialogs[0].Length)
        {
            manText.text += dialogs[0][i];
            i++;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;

        manText.text = "";

        menuCam.eulerAngles = new Vector3(-12,250,0);
        menuCam.position = new Vector3(3,25.2f,-4f);

        int j = 0;
        while (j < dialogs[1].Length)
        {
            manText.text += dialogs[1][j];
            j++;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;

        manText.text = "";

        menuCam.eulerAngles = new Vector3(-1.909f,0.955f,0);
        menuCam.position = new Vector3(-1,26,-10);

        int k = 0;
        while (k < dialogs[2].Length)
        {
            manText.text += dialogs[2][k];
            k++;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;

        manText.text = "";

        menuCam.eulerAngles = new Vector3(-24,154.5f,0);
        menuCam.position = new Vector3(-2.6f,24.5f,-2.5f);

        int l = 0;
        while (l < dialogs[3].Length)
        {
            manText.text += dialogs[3][l];
            l++;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;

        manText.text = "";

        menuCam.gameObject.SetActive(false);
        gameCam.gameObject.SetActive(true);

        int m = 0;
        while (m < dialogs[4].Length)
        {
            manText.text += dialogs[4][m];
            m++;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;

        manText.text = "";
        icare.GetComponent<Player.Mouvement>().enabled = true;
        lueur.GetComponent<LueurBehaviour>().enabled = true;
    }
}
