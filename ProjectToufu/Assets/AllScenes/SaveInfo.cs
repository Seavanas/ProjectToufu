using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveInfo : MonoBehaviour {

    public static SaveInfo saveInfo;
    public SaveInfoObject SaveObject;//the object that should be saved
	// Use this for initialization
	void Awake () {
        if (saveInfo == null)
        {
            saveInfo = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (saveInfo != this)
            Destroy(this);
	}
	
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "PlayerInfo.dat");

        bf.Serialize(file, SaveObject);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "PlayerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "PlayerInfo.dat", FileMode.Open);
            SaveObject = (SaveInfoObject)bf.Deserialize(file);
        }

    }
}


[Serializable]
public class SaveInfoObject
{
    public int Lives, Score;
    

}