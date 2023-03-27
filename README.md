# BCI-Trial-Manager-VR

Current Tested Unity version:
2020.3...

Requires Steam VR..
 
Developed for mmy PhD research trials in Brain Computer Interface Motion Decoding at Ulster University Intelligent Systems Research Centre.

Pilot study published here:
https://ieeexplore.ieee.org/document/9967577

This software is a flexible BCI framework paradigm for motion based kinematic limb experiments. It is setup to run an online motion trajectory decoder in an emmbodied upper limb VR scenario. 

This paradigm is used to record motion data using SteamVR Basestations with a Vive Tracker or controller. EEG data is recorded seperately and processed along with the motion data to train a decoder.

The motion decoder is developed seperately in matlab and python.

The framework has adjustible timing and event triggers to work with simulink based motor BCI. Currently, it is using timed/cued trials.

**The development is somewhat ad-hoc as it is currently only intended to be used in lab testing scenarios ISRC SCANi-Hub.**

Kinematic data is recorded in CSV files to your appdata folder (persistent data folder)

