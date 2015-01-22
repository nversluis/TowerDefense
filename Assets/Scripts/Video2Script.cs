using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Video2Script : MonoBehaviour {
    Sprite[] image;
    public AudioClip[] audio;
    public Image videoImage;
    int i;
    AudioSource camAudio;
	// Use this for initialization
	void Start () {
        Time.timeScale = 1;

        image = Resources.LoadAll<Sprite>("VideoSprites");
        i = 0;
        InvokeRepeating("Video", 1f, 1 / 30f);
        camAudio = GameObject.Find("Main Camera").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Video () {
        videoImage.sprite = image[i];

        if (i == 5)
        {
            camAudio.PlayOneShot(audio[0]);
        }

        if (i == 60)
        {
            camAudio.PlayOneShot(audio[1]);
        }

        if (i == 125)
        {
            camAudio.PlayOneShot(audio[2]);
        }

        i++;

        if (i > 177)
        {
            Application.LoadLevel("Login");
        }
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
        {
            Application.LoadLevel("Login");
        }
    }
}
