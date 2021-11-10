using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;//����

    public GameObject heartBar;

    [Header("UI Elements")]
    public GameObject pauseMenu;//��ͣ�˵�
    public Slider bossHealthBar;



    private void Awake()
    {
        if (instance == null)//��Ϊ��Ҫ�����л�������ÿ�ζ���ҪΨһ��
        
            instance = this;
       
        else
            Destroy(gameObject);

       
    }



    /// <summary>
    /// ʵʱ��ʾѪ���ķ��� currentHearth:��ǰѪ��
    /// </summary>
    public void UpdateHealth(float currentHearth ) {

        switch (currentHearth)
        {

            case 3:
                heartBar.transform.GetChild(0).gameObject.SetActive(true);
                heartBar.transform.GetChild(1).gameObject.SetActive(true);
                heartBar.transform.GetChild(2).gameObject.SetActive(true);

                break;
            case 2:
                heartBar.transform.GetChild(0).gameObject.SetActive(true);
                heartBar.transform.GetChild(1).gameObject.SetActive(true);
                heartBar.transform.GetChild(2).gameObject.SetActive(false);

                break;
            case 1:
                heartBar.transform.GetChild(0).gameObject.SetActive(true);
                heartBar.transform.GetChild(1).gameObject.SetActive(false);
                heartBar.transform.GetChild(2).gameObject.SetActive(false);

                break;
            case 0:
                heartBar.transform.GetChild(0).gameObject.SetActive(false);
                heartBar.transform.GetChild(1).gameObject.SetActive(false);
                heartBar.transform.GetChild(2).gameObject.SetActive(false);

                break;

        }


    }

    /// <summary>
    /// ��ͣ��Ϸ��ʱ��˵���������ϷҪֹͣ
    /// </summary>
    public void PauseGame() {

        pauseMenu.SetActive(true);
        Time.timeScale = 0;//��Ϸ��ͣ��1����Ϸ�ָ�����

    }

    /// <summary>
    /// ������Ϸ
    /// </summary>
    public void ResumeGame()
    {

        pauseMenu.SetActive(false);
        Time.timeScale = 1;//��Ϸ��ͣ��1����Ϸ�ָ�����

    }

    /// <summary>
    /// ����boss�����ֵ
    /// </summary>
    public void SetBossHealth(float maxHealth) {

        bossHealthBar.maxValue = maxHealth;

    }

    /// <summary>
    /// ����boss��Ѫ����uiͼ��һ��
    /// </summary>
    /// <param name="health"></param>
    public void UpdateBossHealth(float health)
    {

        bossHealthBar.value = health;
    }


    public GameObject gameOverPanel;//��Ϸ��������

    public void GameOverUI(bool playerDead)
    {

        gameOverPanel.SetActive(playerDead);


    }

}
