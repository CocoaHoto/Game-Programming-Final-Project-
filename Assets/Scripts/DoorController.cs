using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public string nextScene;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            var AI = GameObject.FindGameObjectsWithTag("AI");
            if(AI.Length == 0) {
                UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
            }
        }
    }
}
