using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlButton : MonoBehaviour
{
    public void OnButtonPressed(){
        SceneManager.LoadScene(1);
    }
}
