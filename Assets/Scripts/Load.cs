using System.IO;
using UnityEngine;

public class Load : MonoBehaviour
{
    
    static public void SaveJson(string type,object obj)
    {
        string path = Path.Combine(Application.dataPath, type);
        File.WriteAllText(path, JsonUtility.ToJson(obj));
    }

    static public T LoadingJSSON<T>(string type)
    {
        string sPath = Path.Combine(Application.dataPath, "/Setting");
        string path = Path.Combine(Application.dataPath, "/Setting"+ type);
        if (File.Exists(path))
        {
            return JsonUtility.FromJson<T>(File.ReadAllText(path));
        }
        else return default(T);
    }
}
