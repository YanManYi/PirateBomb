using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSound : MonoBehaviour
{
    private static  CutSound instance;

    public static  CutSound Instance { get => instance; set => instance = value; }


    private AudioSource audioSource;
    public AudioClip jump, land, hit, die, bomb,freeBomb,backSound;

    private void Awake()
    {
        
        DontDestroyOnLoad(transform.parent.parent);

        audioSource = GetComponent<AudioSource>();

        if (instance != null)
        {
            Destroy(transform.parent.parent.gameObject);
        }
        else instance = this;
    }

   

    public void JumpSound() { audioSource.clip = jump;audioSource.Play(); }

    public void LandSound() { audioSource.clip = land;audioSource.Play(); }
    public void HitSound(){ audioSource.clip = hit; audioSource.Play(); }
    public void BackSound() { transform.parent.GetChild(0).GetComponent<AudioSource>().clip = backSound; transform.parent.GetChild(0).GetComponent<AudioSource>().Play(); transform.parent.GetChild(0).GetComponent<AudioSource>().loop = true; }
    public void DieSound() { transform.parent.GetChild(0).GetComponent<AudioSource>() .clip = die; transform.parent.GetChild(0).GetComponent<AudioSource>().Play(); transform.parent.GetChild(0).GetComponent<AudioSource>().loop = false; }
    public void FreeBombSound() { audioSource.clip = freeBomb; audioSource.Play(); }
    public void BombSound() {transform.parent.GetChild(2).GetComponent<AudioSource>() .Play(); }


}
