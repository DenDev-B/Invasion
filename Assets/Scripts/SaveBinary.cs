using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveBinary : MonoBehaviour {

    static public void Save(object obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + "/SaveData.bs");
        bf.Serialize(file, obj);
        file.Close();
    }
}