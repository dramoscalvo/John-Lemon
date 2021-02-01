using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public CanvasGroup caughtBackgroundImageCanvasGroup;
    private float timer;
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public GameObject player;
    private bool isPlayerAtExit;
    private bool isPlayerCaught;
    public AudioSource exitAudio;
    public AudioSource caughtAudio;
    private bool hasAudioPlayed;


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == player){
            isPlayerAtExit = true;
        }
    }
    private void Update() {
        if (isPlayerAtExit){
            EndLevel(exitBackgroundImageCanvasGroup, false, exitAudio);
        } else if (isPlayerCaught){
            EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio);
        }
    }

    /// <summary>
    /// Lanza la imagen de fin de la partida
    /// </summary>
    /// <param name="imageCanvasGroup">Imagen de fin de partida correspondiente</param>
    private void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource) {
        
        if (!hasAudioPlayed)
        {
            audioSource.Play();
            hasAudioPlayed = true;
        }
        
        timer += Time.deltaTime;
            imageCanvasGroup.alpha = timer/fadeDuration;

            if(timer > fadeDuration + displayImageDuration){
                if(doRestart)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }else
                {
                    Application.Quit();
                }
            }
    }

    public void CatchPlayer(){
        isPlayerCaught = true;
    }
}
