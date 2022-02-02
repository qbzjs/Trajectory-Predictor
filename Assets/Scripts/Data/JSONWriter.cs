using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONWriter
{
    private SettingsDataObject settingsData;
    private ScoreDataObject scoreData;

    public void OutputSettingsJSON(SettingsDataObject settings)
    {
        settingsData = settings;
        
        string output = JsonUtility.ToJson(settingsData,true);
        
        string folderName = "MotionData" + "_" + Settings.instance.sessionName + "_Session_" + Settings.instance.sessionNumber.ToString();
        string folderPath = Application.persistentDataPath + "/" + folderName;
        System.IO.Directory.CreateDirectory(folderPath);
        string path = Application.persistentDataPath + "/" + folderName + "/Session Settings.txt";
        File.WriteAllText(path,output);
        
        //File.WriteAllText(Application.persistentDataPath+"/settings.txt",output);
    }
    public void OutputScoreJSON(ScoreDataObject score)
    {
        scoreData = score;

        string saveName = "Score R_" + scoreData.run + "_B_" + scoreData.block + ".txt"; 
        
        string output = JsonUtility.ToJson(scoreData,true);
        
        string folderName = "MotionData" + "_" + Settings.instance.sessionName + "_Session_" + Settings.instance.sessionNumber.ToString();
        string folderPath = Application.persistentDataPath + "/" + folderName;
        System.IO.Directory.CreateDirectory(folderPath);
        string path = Application.persistentDataPath + "/" + folderName + "/" + saveName;
        File.WriteAllText(path,output);
        
        Debug.Log("score written************************");
        
        //File.WriteAllText(Application.persistentDataPath+"/settings.txt",output);
    }
}
