using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // SampleScene2へ遷移する
    public void OnClickSampleScene2Button()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene3");
    }
}
