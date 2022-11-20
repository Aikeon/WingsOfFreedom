using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LueurBehaviour : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float baseSpeed;
    [SerializeField] float speedCap;
    [SerializeField] float distMult;
    [SerializeField] GameObject endView;
    [SerializeField] Image endBackground;
    [SerializeField] TextMeshProUGUI endText;
    bool ending;
    private string line1;
    private string line2_1;
    private string line2_2;
    private string line2_3;
    private string line2_4;
    private string line3;

    // Update is called once per frame
    void Update()
    {
        var distSpeed = distMult / Mathf.Abs(transform.position.z - player.transform.position.z);
        var trueSpeed = Mathf.Min(baseSpeed * Time.deltaTime * distSpeed, speedCap * Time.deltaTime);
        transform.position += Vector3.forward * trueSpeed;
        line1 = "A cet instant, il est bel et bien devenu l'homme le plus heureux du monde.";
        line2_1 = "Les rumeurs étaient donc vraies, mais";
        line2_2 = "... ";
        line2_3 = "cela en valait-t-il le coup";
        line2_4 = "...?";
        line3 = "Je suis mieux ici, à vivre paisiblement.";
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("touched");
        if (other.gameObject.TryGetComponent<Player.Mouvement>(out var p) && !ending)
        {
            ending = true;
            StartCoroutine(endCinematic());
        }
    }

    IEnumerator endCinematic()
    {
        var timeEllapsed = 0f;
        while (timeEllapsed < 2f)
        {
            Time.timeScale = Mathf.Lerp(1,0,Mathf.Pow(timeEllapsed/2,0.5f));
            timeEllapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        GameManager.Instance.audioManager.Stop("Musique");
        endView.SetActive(true);
        GameManager.Instance.icare.GetComponent<Player.Mouvement>().enabled = false;
        GameManager.Instance.lueur.GetComponent<LueurBehaviour>().enabled = false;

        yield return new WaitForSecondsRealtime(3f);
        GameManager.Instance.gameCam.gameObject.SetActive(false);
        GameManager.Instance.menuCam.gameObject.SetActive(false);
        GameManager.Instance.menuCam.gameObject.GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 11;
        GameManager.Instance.audioManager.Play("Chimes");
        yield return new WaitForSecondsRealtime(3f);
        GameManager.Instance.menuCam.gameObject.SetActive(true);

        int i = 0;
        while (i < line1.Length)
        {
            endText.text += line1[i];
            i++;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        yield return new WaitForSecondsRealtime(4f);
        endText.text = "";
        int j = 0;
        while (j < line2_1.Length)
        {
            endText.text += line2_1[j];
            j++;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        j = 0;
        while (j < line2_2.Length)
        {
            endText.text += line2_2[j];
            j++;
            yield return new WaitForSecondsRealtime(1f);
        }
        j = 0;
        while (j < line2_3.Length)
        {
            endText.text += line2_3[j];
            j++;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        j = 0;
        while (j < line2_4.Length)
        {
            endText.text += line2_4[j];
            j++;
            yield return new WaitForSecondsRealtime(1f);
        }

        GameManager.Instance.menuCam.position = new Vector3(-2.6f,24.5f,-2.6f);
        // Camera.main.transform.position = new Vector3(-2.6f,24.5f,-2.6f);
        // Camera.main.transform.eulerAngles = GameManager.Instance.menuCam.eulerAngles;

        yield return new WaitForSecondsRealtime(1f);
        
        Time.timeScale = 1;

        var timeEllapsed2 = 0f;
        while (timeEllapsed2 < 4f)
        {
            endBackground.color = new Color(0,0,0,Mathf.Lerp(1,0,timeEllapsed2*timeEllapsed2/4));
            timeEllapsed2 += Time.unscaledDeltaTime;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(1f);

        endText.text = "";
        int k = 0;
        while (k < line3.Length)
        {
            endText.text += line3[k];
            k++;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        yield return new WaitForSecondsRealtime(4f);
        endText.text = "";
        var lastSeconds = 0f;
        while (lastSeconds < 4f)
        {
            GameManager.Instance.menuCam.position += Vector3.up * 0.5f * (1 + lastSeconds);
            lastSeconds += Time.deltaTime;
            yield return null;
        }


        SceneManager.LoadScene("MainScene");
    }
}
