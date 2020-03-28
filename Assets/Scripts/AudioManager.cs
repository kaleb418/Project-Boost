using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager:MonoBehaviour {
    [SerializeField] AudioClip[] musicClip;

    private AudioSource audioSource;
    private bool audioIsQueued = false;
    private int lastSongPlayedIndex;
    private bool hasBeenPermanenced = false;

    private void Awake() {
        if(!GameObject.Find("Game Audio").GetComponent<AudioManager>().hasPermanentStatus()) {
            // Only set DontDestroy once, even on future scene loads
            DontDestroyOnLoad(this);
            setPermanentStatus();
            /*
             * Todo
             *
             * Create master scene which loads universal objects
             * and is inaccessible after initial load to prevent dupes
             */
        }
    }

    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
        PlayRandomMusic();
    }

    // Update is called once per frame
    void Update() {
        CheckAudio();
    }

    public void StopMusic() {
        audioSource.Stop();
    }

    public void PlayRandomMusic() {
        int randomMusicIndex;
        do {
            randomMusicIndex = Random.Range(0, 4);
        } while(lastSongPlayedIndex == randomMusicIndex);
        lastSongPlayedIndex = randomMusicIndex;
        audioSource.clip = musicClip[randomMusicIndex];
        audioSource.Play();
        audioIsQueued = false;
    }

    public void CheckAudio() {
        if(!audioSource.isPlaying && !audioIsQueued) {
            Invoke("PlayRandomMusic", 10f);
            audioIsQueued = true;
        }
    }

    public void setPermanentStatus() {
        hasBeenPermanenced = true;
    }

    public bool hasPermanentStatus() {
        return hasBeenPermanenced;
    }
}
