using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;


public class VideoScript : MonoBehaviour {

    public Sprite[] image;
    int spriteNum = 0;
    public Image videoImage;
    public AudioSource camAudio;
    public AudioClip[] audios;

	// Use this for initialization
	void Start () {
        InvokeRepeating("Video", 1f, 1/30f);
        camAudio = GameObject.Find("Main Camera").GetComponent<AudioSource>();
    }


    void Video()
    {
        videoImage.sprite = image[spriteNum];
        if (spriteNum == 0)
        {
            camAudio.PlayOneShot(audios[0]);
        }
        if (spriteNum == 60)
        {
            camAudio.PlayOneShot(audios[1]);
        }
        if (spriteNum == 127)
        {
            camAudio.PlayOneShot(audios[2]);
        }
        spriteNum += 1;

        if (spriteNum > 151)
        {
            Application.LoadLevel("Main menu");
        }
    }
}
