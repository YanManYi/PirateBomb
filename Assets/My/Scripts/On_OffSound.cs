using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class On_OffSound : MonoBehaviour
{
    public Button onSound,offSound;
    public GameObject soundManager;

    private void Awake()
    {
       soundManager= GameObject.Find("Sound").transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        onSound.onClick.AddListener(()=> { soundManager.SetActive(false); offSound.gameObject.SetActive(true); onSound.gameObject.SetActive(false); });
        offSound.onClick.AddListener(()=>{ soundManager.SetActive(true); offSound.gameObject.SetActive(false); onSound.gameObject.SetActive(true); });

        if (!soundManager.activeSelf)
        {
            offSound.gameObject.SetActive(true); onSound.gameObject.SetActive(false);
        }

    }

 

}
