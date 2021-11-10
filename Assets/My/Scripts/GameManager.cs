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
    //���˵ļ��ϣ�Ϊ�յ�ʱ������ſ��Կ�

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
    /// ���¼��ص�ǰ����
    /// </summary>
    public void RestartScene()
    {
          PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("sceneIndex", SceneManager.GetActiveScene().buildIndex);//Ѫ���ļ�ֵ���ڳ�ʼ��ʱ�������Լ����õ�
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//��ȡ��ǰ����Ϸ�����������л���ȥ
       


  



        //ע�����棬�Ϳ����¼��س�����ʱ���������ʷ��¼Ѫ
        // PlayerPrefs.DeleteKey("PlayerHealth");//���»ָ�3��Ѫ


    }




    /// <summary>
    /// �ص���ʼҳ��
    /// </summary>
    public void GoToMainMenu()
    {
        if (player != null)//�������ǹؿ�û�����ǽű�
            if (!player.isDead)
            {
                //д����ǰ�����Ѫ��
                PlayerPrefs.SetFloat("PlayerHealth", player.health);
                PlayerPrefs.SetInt("sceneIndex", SceneManager.GetActiveScene().buildIndex);
            }

        SceneManager.LoadScene(0);

    }


    /// <summary>
    /// ȥ����һ�����������ֵ��һ
    /// </summary>
    public void NextLevel()
    {
        SaveDate();
        if (SceneManager.GetActiveScene().buildIndex + 1 <= 3)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else 
            SceneManager.LoadScene(0);

    }


    #region ���¿�ʼ���������뿪

    /// <summary>
    /// �µ���Ϸ���¿�ʼ
    /// </summary>
    public void NewGame()
    {

        PlayerPrefs.DeleteAll();//ɾ����������

        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// ������ʷ��¼�Ĺؿ�
    /// </summary>
    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("sceneIndex"))
            SceneManager.LoadScene(PlayerPrefs.GetInt("sceneIndex"));

        else NewGame();
    }



    /// <summary>
    /// �뿪��Ϸ
    /// </summary>

    public void QuitGame()
    {

        Application.Quit();//�����������ʵ���Ƴ���Ϸ

    } 
    #endregion



    //���س���������ݱ������ǵ�Ѫ��
    public float LoadHealth()
    {

        if (!PlayerPrefs.HasKey("PlayerHealth"))
        {
            PlayerPrefs.SetFloat("PlayerHealth", 3f);

        }

        //��ǰ��Ѫ��
        //var temp = PlayerPrefs.GetFloat("PlayerHealth");
        //float currentHealth = temp == 0 ? 3 : temp;

        float currentHealth= PlayerPrefs.GetFloat("PlayerHealth");
        return currentHealth;
    }


    /// <summary>
    /// ����Ѫ������
    /// </summary>
    public void SaveDate()
    {

        //д����ǰ�����Ѫ��
        PlayerPrefs.SetFloat("PlayerHealth", player.health);


        PlayerPrefs.SetInt("sceneIndex", SceneManager.GetActiveScene().buildIndex + 1);//������һ�����������кţ�������Ϸ��ʱ�����
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

            
            doorExit.OpenDoor();//���Ŷ���
        }



    }




}
