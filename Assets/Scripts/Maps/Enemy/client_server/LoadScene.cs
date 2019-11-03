using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public AsyncOperation AsOp;
    public float loading_progress = 0;
    public int Round_load = 0;
    public Texture2D texture_loading;
    public Texture2D texture_Fon;
    // Start is called before the first frame update
    void Start()
    {
        AsOp = Application.LoadLevelAsync("World1");
        AsOp.allowSceneActivation = false;
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture_Fon);
       // GUI.DrawTexture(new Rect(Screen.width / 2 - texture_Fon.width / 2, Screen.height / 2 - texture_Fon.height / 2, 0, 0), texture_Fon);

        if (loading_progress < 100)
        {
            loading_progress += Time.deltaTime * 25;
            Round_load = Mathf.RoundToInt(loading_progress);
        }
        if(loading_progress >= 100)
        {
            AsOp.allowSceneActivation = true;
        }

        if(AsOp != null)
        {
            GUI.Label(new Rect(Screen.width / 2 + 50, Screen.height / 2 + 7, 50, 50), "" + Round_load + "%");
            GUI.DrawTexture(new Rect(Screen.width/2-100,Screen.height/2-5,loading_progress*2,5),texture_loading);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
