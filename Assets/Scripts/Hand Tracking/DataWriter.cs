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

    private bool writeTitle = true;

    public void WriteTrajectoryData(Vector3 p, Vector3 r, Vector3 a, float aavg, float v, string ts, string elapsed, string tag, string target){

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
                .Append("Average Acceleration").Append (",")
                .Append("Velocity").AppendLine ();
            
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
                .Append(aavg).Append (",")
                .Append(v).AppendLine ();
        }

    }
    public void WriteData(string fileName)
    {
        dataString = stringBuilder.ToString();

        // need to make the folder create dynamically
        string folderName = "TrajectoryData";
        string folderPath = Application.persistentDataPath + "/" + folderName;
        System.IO.Directory.CreateDirectory(folderPath);
        string path = Application.persistentDataPath + "/" + folderName + "/"  + fileName  + ".csv";
        System.IO.File.WriteAllText (path, dataString);
        Debug.Log("Data Written - "+path);
        // Debug.Log(dataString);
        stringBuilder.Clear();
        
        writeTitle = true;
    }

}
