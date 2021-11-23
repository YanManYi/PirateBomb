using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{


    public static GameManager instance;
    [HideInInspector]
    public  PlayerController player;
    public bool gameOver;

    private Door doorExit;

    public List<Enemy> enemylist = new List<Enemy>();
    //敌人的集合，为空的时候这个门可以开

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);

        player = FindObjectOfType<PlayerController>();

        doorExit = FindObjectOfType<Door>();


    }

    private void Update()
    {
        gameOver = player.isDead;
        UIManager.instance.GameOverUI(gameOver);

    }

    /// <summary>
    /// 重新加载当前场景
    /// </summary>
    public void RestartScene()
    {
          PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("sceneIndex", SceneManager.GetActiveScene().buildIndex);//血量的键值对在初始的时候人物自己会拿到
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//获取当前的游戏场景名字且切换过去
       


  



        //注销下面，就可重新加载场景的时候继续是历史记录血
        // PlayerPrefs.DeleteKey("PlayerHealth");//重新恢复3滴血


    }




    /// <summary>
    /// 回到开始页面
    /// </summary>
    public void GoToMainMenu()
    {
        if (player != null)//最后结束那关卡没有主角脚本
            if (!player.isDead)
            {
             
                 //写进当前的玩家血量
                 PlayerPrefs.SetFloat("PlayerHealth", player.health);
                PlayerPrefs.SetInt("sceneIndex", SceneManager.GetActiveScene().buildIndex);
            }
        Time.timeScale = 1;
        SceneManager.LoadScene(0);

    }


    /// <summary>
    /// 去到下一个场景，编号值加一
    /// </summary>
    public void NextLevel()
    {
        SaveDate();
        if (SceneManager.GetActiveScene().buildIndex + 1 <= 3)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else 
            SceneManager.LoadScene(0);

    }


    #region 重新开始，继续，离开

    /// <summary>
    /// 新的游戏重新开始
    /// </summary>
    public void NewGame()
    {

        PlayerPrefs.DeleteAll();//删除所有数据

        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// 加载历史记录的关卡
    /// </summary>
    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("sceneIndex"))
            SceneManager.LoadScene(PlayerPrefs.GetInt("sceneIndex"));

        else NewGame();
    }



    /// <summary>
    /// 离开游戏
    /// </summary>

    public void QuitGame()
    {

        Application.Quit();//打包出来后真实的推出游戏

    } 
    #endregion



    //加载场景后的数据保存主角的血量
    public float LoadHealth()
    {

        if (!PlayerPrefs.HasKey("PlayerHealth"))
        {
            PlayerPrefs.SetFloat("PlayerHealth", 3f);

        }

        //当前的血量
        //var temp = PlayerPrefs.GetFloat("PlayerHealth");
        //float currentHealth = temp == 0 ? 3 : temp;

        float currentHealth= PlayerPrefs.GetFloat("PlayerHealth");
        return currentHealth;
    }


    /// <summary>
    /// 保存血量数据
    /// </summary>
    public void SaveDate()
    {

        //写进当前的玩家血量
        PlayerPrefs.SetFloat("PlayerHealth", player.health);


        PlayerPrefs.SetInt("sceneIndex", SceneManager.GetActiveScene().buildIndex + 1);//保存下一个场景的序列号，继续游戏的时候加载
        PlayerPrefs.Save();//


    }







    public void IsEnemy(Enemy enemy)
    {
        enemylist.Add(enemy);
    }


    public void EnemyDead(Enemy enemy)
    {

        enemylist.Remove(enemy);

        if (enemylist.Count == 0)
        {

            
            doorExit.OpenDoor();//播放动画
        }



    }




}
