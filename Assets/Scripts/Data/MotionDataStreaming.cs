using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

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
                case MotionTag.LeftHand:
                    motionData = dao.MotionData_Head;
                    break;
                case MotionTag.RightHand:
                    motionData = dao.MotionData_RightHand;
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
                case MotionTag.LeftHand:
                    dao.MotionData_LeftHand = data;
                    break;
                case MotionTag.RightHand:
                    dao.MotionData_RightHand = data;
                    break;
                case MotionTag.Head:
                    dao.MotionData_Head = data;
                    break;
            }
        }
    }


}
