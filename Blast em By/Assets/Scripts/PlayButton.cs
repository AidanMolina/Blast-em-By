using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void OnButtonPressed(){
        SceneManager.LoadScene(2);
    }
}
