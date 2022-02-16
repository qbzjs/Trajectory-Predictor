using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONWriter
{
    private SettingsDataObject settingsData;
    private ScoreBlockDataObject scoreBlockData;
    private ScoreSessionDataObject scoreSessionData;

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
    public void OutputScoreBlockJSON(ScoreBlockDataObject scoreBlock)
    {
        scoreBlockData = scoreBlock;

        string saveName = "Score R_" + scoreBlockData.run + "_B_" + scoreBlockData.block + ".txt"; 
        
        string output = JsonUtility.ToJson(scoreBlockData,true);
        
        string folderName = "MotionData" + "_" + Settings.instance.sessionName + "_Session_" + Settings.instance.sessionNumber.ToString();
        string folderPath = Application.persistentDataPath + "/" + folderName;
        System.IO.Directory.CreateDirectory(folderPath);
        string path = Application.persistentDataPath + "/" + folderName + "/" + saveName;
        File.WriteAllText(path,output);
        
        Debug.Log("score written **** BLOCK ****");
        
        //File.WriteAllText(Application.persistentDataPath+"/settings.txt",output);
    }
    public void OutputScoreSessionJSON(ScoreSessionDataObject scoreSession)
    {
        scoreSessionData = scoreSession;
    
        string saveName = "Session Score" + ".txt"; 
        
        string output = JsonUtility.ToJson(scoreSessionData,true);
        
        string folderName = "MotionData" + "_" + Settings.instance.sessionName + "_Session_" + Settings.instance.sessionNumber.ToString();
        string folderPath = Application.persistentDataPath + "/" + folderName;
        System.IO.Directory.CreateDirectory(folderPath);
        string path = Application.persistentDataPath + "/" + folderName + "/" + saveName;
        File.WriteAllText(path,output);
        
        Debug.Log("score written **** SESSION ****");
        
        //File.WriteAllText(Application.persistentDataPath+"/settings.txt",output);
    }
}
