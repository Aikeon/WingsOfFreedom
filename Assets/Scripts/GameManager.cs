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
    [SerializeField] bool debugMode;
    private Cinemachine.CinemachineFreeLook freelook;
    public bool seeing;
    [SerializeField] string[] dialogs;
    [SerializeField] TextMeshProUGUI manText;
    
    public AudioManager audioManager;

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

    void Update()
    {
        Debug.Log(Mathf.Acos(Vector2.Angle(Vector2.right, (new Vector2(-icare.transform.position.y + lueur.transform.position.y, -icare.transform.position.x + lueur.transform.position.x)).normalized)));
        freelook.m_XAxis.m_InputAxisValue = Mathf.Acos(Vector2.Angle(Vector2.right, (new Vector2(-icare.transform.position.y + lueur.transform.position.y, -icare.transform.position.x + lueur.transform.position.x)).normalized));
    }

    // Update is called once per frame
    public void startCinematic()
    {
        StartCoroutine(cinematic());
    }

    IEnumerator cinematic()
    {
        var timeEllapsed = 0f;
        while (timeEllapsed < yCurve.keys[yCurve.keys.Length-1].time && (!Input.GetKeyDown(KeyCode.Space) || !debugMode))
        {
            menuCam.position = new Vector3(0,yCurve.Evaluate(timeEllapsed),-22.5f);
            timeEllapsed += Time.deltaTime;
            yield return null;
        }
        menuCam.position = new Vector3(0,27.5f, -22.5f);
        var timeEllapsed2 = 0f;
        icare.GetComponent<Animator>().Play("Walk");
        icare.transform.eulerAngles = new Vector3(0,180,0);
        while (icare.transform.position.z < -6 && !Input.GetKeyDown(KeyCode.Space))
        {
            icare.transform.position += Vector3.forward * Time.deltaTime;
            timeEllapsed2 += Time.deltaTime;
            yield return null;
        }
        icare.transform.position = new Vector3(0,icare.transform.position.y, -6);
        icare.GetComponent<Animator>().Play("Idle");
        seeing = true;

        int i = 0;
        while (i < dialogs[0].Length && (!Input.GetKeyDown(KeyCode.Space) || !debugMode))
        {
            manText.text += dialogs[0][i];
            i++;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        manText.text = dialogs[0];

        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;

        manText.text = "";

        menuCam.eulerAngles = new Vector3(-12,250,0);
        menuCam.position = new Vector3(3,25.2f,-4f);

        int j = 0;
        while (j < dialogs[1].Length && (!Input.GetKeyDown(KeyCode.Space) || !debugMode))
        {
            manText.text += dialogs[1][j];
            j++;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        manText.text = dialogs[1];

        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;

        manText.text = "";

        menuCam.eulerAngles = new Vector3(-1.909f,0.955f,0);
        menuCam.position = new Vector3(-1,26,-10);

        int k = 0;
        while (k < dialogs[2].Length && (!Input.GetKeyDown(KeyCode.Space) || !debugMode))
        {
            manText.text += dialogs[2][k];
            k++;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        manText.text = dialogs[2];

        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;

        manText.text = "";

        menuCam.eulerAngles = new Vector3(-24,154.5f,0);
        menuCam.position = new Vector3(-2.6f,24.5f,-2.6f);

        int l = 0;
        while (l < dialogs[3].Length && (!Input.GetKeyDown(KeyCode.Space) || !debugMode))
        {
            manText.text += dialogs[3][l];
            l++;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        manText.text = dialogs[3];

        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;

        manText.text = "";

        menuCam.gameObject.SetActive(false);
        gameCam.gameObject.SetActive(true);

        int m = 0;
        while (m < dialogs[4].Length && (!Input.GetKeyDown(KeyCode.Space) || !debugMode))
        {
            manText.text += dialogs[4][m];
            m++;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        manText.text = dialogs[4];

        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;
        
        audioManager.Stop("Chimes");
        audioManager.Play("Musique");

        manText.text = "";
        icare.GetComponent<Player.Mouvement>().enabled = true;
        lueur.GetComponent<LueurBehaviour>().enabled = true;
    }
}
