using Channels;
using Cinemachine;
using Managers;
using Objects;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Camera : Singleton<Camera>
{
    public InputChannel inputChannel;
    
    private void Start()
    {
        LevelManager.HUDCanvas = transform.Find("HudCanvas").gameObject;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            transform.Find("HudCanvas").gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;
        var cinemachine = transform.Find("Cinemachine").GetComponent<CinemachineVirtualCamera>();
        if (cinemachine.Follow == null && FindObjectOfType<PlayerController>() != null)
        {
            cinemachine.Follow = FindObjectOfType<PlayerController>().transform;
        }
    }

    private void OnEnable()
    {
        inputChannel.OnEscapeHeld += Escape;
    }

    private void OnDisable()
    {
        inputChannel.OnEscapeHeld -= Escape;
    }

    private void Escape()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;
        FindObjectOfType<LevelManager>().LoadLevel(0);
    }

    public void EnableTimer(bool input)
    {
        PlayerPrefs.SetInt("EnableTimer", input ? 1 : 0);
    }
}