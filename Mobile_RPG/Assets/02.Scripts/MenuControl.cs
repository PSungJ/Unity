using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private RectTransform PauseBGImg;
    [SerializeField] private RectTransform PauseMenu;
    [SerializeField] private RectTransform SoundMenu;
    [SerializeField] private RectTransform ScreenMenu;
    [SerializeField] private GameObject bladeGirl;
    bool isPause = false;
    void Start()
    {
        PauseBGImg = GameObject.Find("Canvas-UI").transform.GetChild(4).GetComponent<RectTransform>();
        PauseMenu = PauseBGImg.GetChild(0).GetComponent<RectTransform>();
        SoundMenu = PauseBGImg.GetChild(1).GetComponent<RectTransform>();
        ScreenMenu = PauseBGImg.GetChild(2).GetComponent<RectTransform>();
        bladeGirl = GameObject.FindWithTag("Player").gameObject;
    }

    void Update()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                ESC();
                break;
            case RuntimePlatform.WindowsEditor:
                ESC();
                break;
            case RuntimePlatform.IPhonePlayer:
                ESC();
                break;
        }
    }
    private void ESC()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }
    public void Pause()
    {
        isPause = !isPause;
        if (!PauseBGImg.gameObject.activeInHierarchy)
        {
            if (!PauseMenu.gameObject.activeInHierarchy)
            {
                PauseMenu.gameObject.SetActive(true);
                SoundMenu.gameObject.SetActive(false);
                ScreenMenu.gameObject.SetActive(false);
            }
            PauseBGImg.gameObject.SetActive(true);
            Time.timeScale = 0f;    // 시간 정지
            bladeGirl.gameObject.SetActive(false);
        }
    }
    public void Resume()
    {
        PauseMenu.gameObject.SetActive(false);
        SoundMenu.gameObject.SetActive(false);
        ScreenMenu.gameObject.SetActive(false);

        PauseBGImg.gameObject.SetActive(false);
        Time.timeScale = 1f;    // 시간 정지 해제
        bladeGirl.gameObject.SetActive(true);
    }
    public void Sounds(bool isOpen)
    {
        if (isOpen)
        {
            PauseMenu.gameObject.SetActive(false);
            SoundMenu.gameObject.SetActive(true);
            ScreenMenu.gameObject.SetActive(false);
        }
        else
        {
            PauseMenu.gameObject.SetActive(true);
            SoundMenu.gameObject.SetActive(false);
            ScreenMenu.gameObject.SetActive(false);
        }
    }
    public void Screen(bool isOpen)
    {
        if (isOpen)
        {
            PauseMenu.gameObject.SetActive(false);
            SoundMenu.gameObject.SetActive(false);
            ScreenMenu.gameObject.SetActive(true);
        }
        else
        {
            PauseMenu.gameObject.SetActive(true);
            SoundMenu.gameObject.SetActive(false);
            ScreenMenu.gameObject.SetActive(false);
        }
    }
    public void Exit()
    {
        Application.Quit();
        Debug.Log("종료");
    }
}
