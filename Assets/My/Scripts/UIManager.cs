using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;//单例

    public GameObject heartBar;

    [Header("UI Elements")]
    public GameObject pauseMenu;//暂停菜单
    public Slider bossHealthBar;



    private void Awake()
    {
        if (instance == null)//因为需要场景切换，所以每次都需要唯一的
        
            instance = this;
       
        else
            Destroy(gameObject);

       
    }



    /// <summary>
    /// 实时显示血量的方法 currentHearth:当前血量
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
    /// 暂停游戏的时候菜单出来，游戏要停止
    /// </summary>
    public void PauseGame() {

        pauseMenu.SetActive(true);
        Time.timeScale = 0;//游戏暂停，1是游戏恢复正常

    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    public void ResumeGame()
    {

        pauseMenu.SetActive(false);
        Time.timeScale = 1;//游戏暂停，1是游戏恢复正常

    }

    /// <summary>
    /// 设置boss的最大值
    /// </summary>
    public void SetBossHealth(float maxHealth) {

        bossHealthBar.maxValue = maxHealth;

    }

    /// <summary>
    /// 设置boss的血量和ui图标一样
    /// </summary>
    /// <param name="health"></param>
    public void UpdateBossHealth(float health)
    {

        bossHealthBar.value = health;
    }


    public GameObject gameOverPanel;//游戏结束画布

    public void GameOverUI(bool playerDead)
    {

        gameOverPanel.SetActive(playerDead);


    }

}
