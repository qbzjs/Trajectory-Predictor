using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
/// <summary>
/// This class is a bridge between vive trackers and data access DAO
/// </summary>
public class MotionDataStreaming
{

    private DAO dao;

    public MotionDataFormat motionData;
    private MotionTag tag;

    public MotionDataFormat MotionData
    {
        get { return motionData; }
        set { motionData = value; }
    }

    //get the relevant motion data from DAO
    public MotionDataFormat GetMotionData(MotionTag motionTag)
    {
        if (DAO.instance)
        {
            dao = DAO.instance;
        }
        if (dao != null)
        {
            switch (motionTag)
            {
                case MotionTag.Null:

                    break;
                case MotionTag.LeftWrist:
                    motionData = dao.MotionData_LeftWrist;
                    break;
                case MotionTag.RightWrist:
                    motionData = dao.MotionData_RightWrist;
                    break;
                case MotionTag.Head:
                    motionData = dao.MotionData_Head;
                    break;
            }
        }
        return motionData;
    }
    public void SetMotionData(MotionTag motionTag, MotionDataFormat data)
    {
        if (DAO.instance)
        {
            dao = DAO.instance;
        }
        if (dao != null)
        {
            switch (motionTag)
            {
                case MotionTag.Null:

                    break;
                case MotionTag.LeftWrist:
                    dao.MotionData_LeftWrist = data;
                    break;
                case MotionTag.RightWrist:
                    dao.MotionData_RightWrist = data;
                    break;
                case MotionTag.Head:
                    dao.MotionData_Head = data;
                    break;
            }
        }
    }


}
