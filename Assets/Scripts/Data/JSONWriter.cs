using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONWriter{
    
    public bool debugDataWrite = false;
    
    private SettingsDataObject settingsData;
    
    private ScoreBlockDataObject scoreBlockData;
    private TargetHitFormat targetsHitBlock;
    
    private ScoreSessionDataObject scoreSessionData;
    private TargetHitFormat targetsHitSession;

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
    
    //SCORE BLOCK
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

        if (debugDataWrite){
            Debug.Log("score written **** BLOCK ****");
        }

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

        if (debugDataWrite){
            Debug.Log("score written **** SESSION ****");
        }

        //File.WriteAllText(Application.persistentDataPath+"/settings.txt",output);
    }
    
    
    //TARGETS HIT BLOCK (Called after score block)
    public void OutputTargetHitBlockJSON(TargetHitFormat targetsHitBlock)
    {
        this.targetsHitBlock = targetsHitBlock;

        string saveName = "Targets Hit R_" + scoreBlockData.run + "_B_" + scoreBlockData.block + ".txt"; 
        
        string output = JsonUtility.ToJson(this.targetsHitBlock,true);
        
        string folderName = "MotionData" + "_" + Settings.instance.sessionName + "_Session_" + Settings.instance.sessionNumber.ToString();
        string folderPath = Application.persistentDataPath + "/" + folderName;
        System.IO.Directory.CreateDirectory(folderPath);
        string path = Application.persistentDataPath + "/" + folderName + "/" + saveName;
        File.WriteAllText(path,output);

        if (debugDataWrite){
            Debug.Log("targets hit written **** BLOCK ****");
        }

        //File.WriteAllText(Application.persistentDataPath+"/settings.txt",output);
    }
    //TARGETS HIT BLOCK (Called after score block)
    public void OutputTargetHitSessionSON(TargetHitFormat targetsHitSession)
    {
        this.targetsHitSession = targetsHitSession;

        string saveName = "Targets Hit Session" + ".txt"; 
        
        string output = JsonUtility.ToJson(this.targetsHitSession,true);
        
        string folderName = "MotionData" + "_" + Settings.instance.sessionName + "_Session_" + Settings.instance.sessionNumber.ToString();
        string folderPath = Application.persistentDataPath + "/" + folderName;
        System.IO.Directory.CreateDirectory(folderPath);
        string path = Application.persistentDataPath + "/" + folderName + "/" + saveName;
        File.WriteAllText(path,output);

        if (debugDataWrite){
            Debug.Log("targets hit written **** SESSION ****");
        }

        //File.WriteAllText(Application.persistentDataPath+"/settings.txt",output);
    }
    
    
}
