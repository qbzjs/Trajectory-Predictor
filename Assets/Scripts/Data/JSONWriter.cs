using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONWriter : MonoBehaviour
{
    private SettingsDataObject settingsData;

    public void OutputSettingsJSON(SettingsDataObject settings)
    {
        settingsData = settings;
        
        string output = JsonUtility.ToJson(settingsData,true);
        
        string folderName = "MotionData" + "_" + Settings.instance.sessionName + "_Session_" + Settings.instance.sessionNumber.ToString();
        string folderPath = Application.persistentDataPath + "/" + folderName;
        System.IO.Directory.CreateDirectory(folderPath);
        string path = Application.persistentDataPath + "/" + folderName + "/settings.txt";
        File.WriteAllText(path,output);
        
        //File.WriteAllText(Application.persistentDataPath+"/settings.txt",output);
    }
}
