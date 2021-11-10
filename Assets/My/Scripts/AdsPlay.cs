using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdsPlay : MonoBehaviour, IUnityAdsListener
{

#if UNITY_ANDROID
    private string gameId = "4180347";
#endif
   // private string gameId = "4180347";

    Button adsButton;
    [HideInInspector]
    public string myPlacementId = "Rewarded_Android";



    private void Start()
    {
        adsButton = GetComponent<Button>();

    
        if (adsButton) adsButton.onClick.AddListener(()=> { Advertisement.Show(myPlacementId); });

        // Initialize the Ads listener and service:
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, true);
    }




    public void OnUnityAdsDidError(string message)
    {
       
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {

        switch (showResult)
        {
            case ShowResult.Failed:
                break;
            case ShowResult.Skipped:
                break;
            case ShowResult.Finished:
                if (GameManager.instance.player.isDead)
                {
                    UIManager.instance.ResumeGame();//关闭当前暂停UI
                    GameManager.instance.RestartScene();//当前场景

                }
                else { GameManager.instance.player.health = 3; UIManager.instance.UpdateHealth(3);UIManager.instance.ResumeGame(); }


                break;
            default:
                break;
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {
       
    }

    public void OnUnityAdsReady(string placementId)
    {
        //if(Advertisement.IsReady(placementId))
        //Debug.Log("广告准备好了");
    }
}
