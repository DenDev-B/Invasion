using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LoadBinary : MonoBehaviour
{
    static public T Load<T>(ref T obj)
    {
      //  string path = Path.Combine(Application.dataPath, "Save.json");
   //     string path = File.Exists(Application.dataPath + "/Save.json");
        if (File.Exists(Application.dataPath + "/SaveData.bs"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/SaveData.bs", FileMode.Open);
            obj = (T)bf.Deserialize(file);
            file.Close();
            return obj;
        }
        else
            return default(T);
    }
}
