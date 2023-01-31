using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using UnityEngine.SceneManagement;

public class DataWriter : MonoBehaviour
{
    private string dataString;
    private StringBuilder stringBuilder = new StringBuilder ();
    private StringBuilder stringBuilderEye = new StringBuilder ();

    private bool writeTitle = true;

    public void WriteTrajectoryData(string ts, string elapsed, string tag, string target, 
        string motion,float motionThreshold, Vector3 pos, Vector3 rot, float speed, Vector3 vel, Vector3 velOrg, Vector3 acc, float accStr, Vector3 dir,
        float angSpeed, Vector3 angVel, Vector3 angAcc, float angAccStr, Vector3 angAxis){

    
        if (writeTitle){
            stringBuilder.Append("Joint Tag").Append (",")
                .Append("Target Number").Append (",")
                .Append("TimeStamp").Append (",")
                .Append("Elapsed Time").Append (",")
                .Append("Motion Detected").Append (",")
                .Append("Motion Threshold").Append (",")
                .Append("Position X").Append (",")
                .Append("Position Y").Append (",")
                .Append("Position Z").Append (",")
                .Append("Rotation X").Append (",")
                .Append("Rotation y").Append (",")
                .Append("Rotation z").Append (",")
                .Append("Speed").Append (",")
                .Append("Velocity X").Append (",")
                .Append("Velocity Y").Append (",")
                .Append("Velocity Z").Append (",")
                .Append("Velocity X (orig)").Append (",")
                .Append("Velocity Y (orig)").Append (",")
                .Append("Velocity Z (orig)").Append (",")
                .Append("Acceleration X").Append (",")
                .Append("Acceleration Y").Append (",")
                .Append("Acceleration Z").Append (",")
                .Append("Acceleration Strength").Append (",")
                .Append("Direction X").Append (",")
                .Append("Direction Y").Append (",")
                .Append("Direction Z").Append (",")
                .Append("Angular Speed").Append(",")
                .Append("Angular Velocity X").Append (",")
                .Append("Angular Velocity Y").Append (",")
                .Append("Angular Velocity Z").Append (",")
                .Append("Angular Acceleration X").Append (",")
                .Append("Angular Acceleration Y").Append (",")
                .Append("Angular Acceleration Z").Append (",")
                .Append("Angular Acceleration Strength").Append (",")
                .Append("Angular Axis X").Append (",")
                .Append("Angular Axis Y").Append (",")
                .Append("Angular Axis Z").AppendLine();

            writeTitle = false;
        }
        else{
            stringBuilder.Append(tag).Append (",")
                .Append(target).Append (",")
                .Append(ts).Append (",")
                .Append(elapsed).Append (",")
                .Append(motion).Append (",")
                .Append(motionThreshold).Append (",")
                .Append(pos.x).Append (",")
                .Append(pos.y).Append (",")
                .Append(pos.z).Append (",")
                .Append(rot.x).Append (",")
                .Append(rot.y).Append (",")
                .Append(rot.z).Append (",")
                .Append(speed).Append (",")
                .Append(vel.x).Append (",")
                .Append(vel.y).Append (",")
                .Append(vel.z).Append (",")
                .Append(velOrg.x).Append (",")
                .Append(velOrg.y).Append (",")
                .Append(velOrg.z).Append (",")
                .Append(acc.x).Append (",")
                .Append(acc.y).Append (",")
                .Append(acc.z).Append (",")
                .Append(accStr).Append (",")
                .Append(dir.x).Append (",")
                .Append(dir.y).Append (",")
                .Append(dir.z).Append (",")
                .Append(angSpeed).Append(",")
                .Append(angVel.x).Append (",")
                .Append(angVel.y).Append (",")
                .Append(angVel.z).Append (",")
                .Append(angAcc.x).Append (",")
                .Append(angAcc.y).Append (",")
                .Append(angAcc.z).Append (",")
                .Append(angAccStr).Append (",")
                .Append(angAxis.x).Append (",")
                .Append(angAxis.y).Append (",")
                .Append(angAxis.z).AppendLine();
        }
    }

    public void WriteTrajectoryData(Vector3 p, Vector3 r, Vector3 a, Vector3 aS, float aAvg, float aAvgS, float v, float vS, string ts, string elapsed, string tag, string target){

        if (writeTitle){
            stringBuilder.Append("Joint Tag").Append (",")
                .Append("Target Number").Append (",")
                .Append("TimeStamp").Append (",")
                .Append("Elapsed Time").Append (",")
                .Append("Position X").Append (",")
                .Append("Position Y").Append (",")
                .Append("Position Z").Append (",")
                .Append("Rotation X").Append (",")
                .Append("Rotation y").Append (",")
                .Append("Rotation z").Append (",")
                .Append("Acceleration X").Append (",")
                .Append("Acceleration Y").Append (",")
                .Append("Acceleration Z").Append (",")
                .Append("Acceleration X (Smooth)").Append(",")
                .Append("Acceleration Y (Smooth)").Append(",")
                .Append("Acceleration Z (Smooth)").Append(",")
                .Append("Average Acceleration").Append (",")
                .Append("Average Acceleration (Smooth)").Append(",")
                .Append("Velocity").Append(",")
                .Append("Velocity (Smooth)").AppendLine();

            writeTitle = false;
        }
        else{
            stringBuilder.Append(tag).Append (",")
                .Append(target).Append (",")
                .Append(ts).Append (",")
                .Append(elapsed).Append (",")
                .Append(p.x).Append (",")
                .Append(p.y).Append (",")
                .Append(p.z).Append (",")
                .Append(r.x).Append (",")
                .Append(r.y).Append (",")
                .Append(r.z).Append (",")
                .Append(a.x).Append (",")
                .Append(a.y).Append (",")
                .Append(a.z).Append (",")
                .Append(aS.x).Append(",")
                .Append(aS.y).Append(",")
                .Append(aS.z).Append(",")
                .Append(aAvg).Append (",")
                .Append(aAvgS).Append(",")
                .Append(v).Append(",")
                .Append(vS).AppendLine ();
        }

    }

    public void WriteEyeData(string ts, string elapsed, string tag, string target,
    string blinking, float eyeOpennessLeft, float eyeOpennessRight, Vector3 gazeLeft, Vector3 gazeRight,
    float pupilDiameterLeft, float pupilDiameterRight, float xCoord2D, float yCoord2D)
    {
        if (writeTitle){
            stringBuilder.Append("Joint Tag").Append (",")
                .Append("Target Number").Append (",")
                .Append("TimeStamp").Append (",")
                .Append("Elapsed Time").Append (",")
                .Append("Blinking").Append (",")
                .Append("Eye Openness Left").Append (",")
                .Append("Eye Openness Right").Append (",")
                .Append("Gaze Left X").Append (",")
                .Append("Gaze Left Y").Append (",")
                .Append("Gaze Left Z").Append (",")
                .Append("Gaze Right X").Append (",")
                .Append("Gaze Right Y").Append (",")
                .Append("Gaze Right Z").Append (",")
                .Append("Pupil Diameter Left").Append (",")
                .Append("Pupil Diameter Right").Append (",")
                .Append("X Coord 2D").Append (",")
                .Append("Y Coord 2D").AppendLine();

            writeTitle = false;
        }
        else{
            stringBuilder.Append(tag).Append (",")
                .Append(target).Append (",")
                .Append(ts).Append (",")
                .Append(elapsed).Append (",")
                .Append(blinking).Append (",")
                .Append(eyeOpennessLeft).Append (",")
                .Append(eyeOpennessRight).Append (",")
                .Append(gazeLeft.x).Append (",")
                .Append(gazeLeft.y).Append (",")
                .Append(gazeLeft.z).Append (",")
                .Append(gazeRight.x).Append (",")
                .Append(gazeRight.y).Append (",")
                .Append(gazeRight.z).Append (",")
                .Append(pupilDiameterLeft).Append (",")
                .Append(pupilDiameterRight).Append (",")
                .Append(xCoord2D).Append (",")
                .Append(yCoord2D).AppendLine();
        }
    }

    public void WriteEyeClassificationData(string tag, string ts, string elapsed,
        int trigger, string phaseName, int phase, 
        int reachTarget, int lookTarget, string lookTargetName, int blink, string blinking){
        if (writeTitle){
            stringBuilder.Append("Data Tag").Append(",")
                .Append("TimeStamp").Append(",")
                .Append("Elapsed Time").Append(",")
                .Append("Trigger").Append(",")
                .Append("Phase Name").Append(",")
                .Append("Phase").Append(",")
                .Append("Reach Target").Append(",")
                .Append("Look Target").Append(",")
                .Append("Look Target Name").Append(",")
                .Append("Blink").Append(",")
                .Append("Blinking").Append(",");
            
            writeTitle = false;
        }
        else{
            //todo write data
            stringBuilder.Append(tag).Append(",")
                .Append(ts).Append(",")
                .Append(elapsed).Append(",")
                .Append(trigger).Append(",")
                .Append(phaseName).Append(",")
                .Append(phase).Append(",")
                .Append(reachTarget).Append(",")
                .Append(lookTarget).Append(",")
                .Append(lookTargetName).Append(",")
                .Append(blink).Append(",")
                .Append(blinking).Append(",");
        }
    }
        
        
    //TODO - fix naming with new classes---------------------
    public void WriteData(string fileName)
    {
        dataString = stringBuilder.ToString();
        
        string folderName = "MotionData" + "_" + Settings.instance.sessionName + "_Session_" + Settings.instance.sessionNumber.ToString();
        
        string folderPath = Application.persistentDataPath + "/" + folderName + "/Run_" + RunManager.instance.runIndex;

        if (!Directory.Exists(folderPath))
        {
            System.IO.Directory.CreateDirectory(folderPath);
        }

        //string path = Application.persistentDataPath + "/" + folderName + "/"  + fileName  + ".csv";
        string path = folderPath  + "/" + fileName  + ".csv";
        
        System.IO.File.WriteAllText (path, dataString);
        Debug.Log("Data Written - "+path);
        
        // Debug.Log(dataString);
        stringBuilder.Clear();
        
        writeTitle = true;
    }
    // public void WriteData(string fileName)
    // {
    //     dataString = stringBuilder.ToString();
    //
    //     // need to make the folder create dynamically
    //     string folderName = "MotionData";
    //     
    //     string folderPath = Application.persistentDataPath + "/" + folderName;
    //     System.IO.Directory.CreateDirectory(folderPath);
    //     string path = Application.persistentDataPath + "/" + folderName + "/"  + fileName  + ".csv";
    //     System.IO.File.WriteAllText (path, dataString);
    //     Debug.Log("Data Written - "+path);
    //     // Debug.Log(dataString);
    //     stringBuilder.Clear();
    //     
    //     writeTitle = true;
    // }

}
