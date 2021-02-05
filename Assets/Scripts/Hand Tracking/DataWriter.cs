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

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
    }

    public void WriteTrajectoryData(Vector3 p, Vector3 r, string ts, string elapsed, string tag){
    //    position = "x:"+p.x.ToString()+"y:"+p.y.ToString()+"z"+p.z.ToString();
    //    duration = "d:"+t.ToString();
    
    stringBuilder.Append(ts).Append (",")
                 .Append(elapsed).Append (",")
                 .Append(p.x).Append (",")
                 .Append(p.y).Append (",")
                 .Append(p.z).Append (",")
                 .Append(r.x).Append (",")
                 .Append(r.y).Append (",")
                 .Append(r.z).Append (",")
                 .Append(tag).AppendLine ();
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
    }

}
