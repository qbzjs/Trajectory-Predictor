namespace Enums
{

	public enum SampleRate
	{
		Hz50,
		Hz60,
		Hz75,
		Hz100
	}
	
	public enum TrialEventType
    {
	    Null,
	    Ready,
	    PreTrialPhase,
	    Initialise,
		Fixation,
		Arrow,
		Target,
		Observation,
		Rest,
		Complete,
		PostTrialPhase
    }
	public enum RigType {AvatarRig, GhostRig}
	
	public enum TrialType{
		Horizontal,
		Vertical,
		CentreOut
	}

	public enum SequenceType{
		Linear,
		Permutation
	}
	public enum TaskSide{
		Left,
		Right
	}

	public enum MotionTag{
		Null, 
		LeftHand, 
		RightHand, 
		Head,
		RightElbow,
		LeftElbow,
		RightShoulder,
		LeftShoulder,
		RightWrist,
		LeftWrist,
		RightWrist_Leap,
		LeftWrist_Leap,
		LeftPointerFinger_Leap,
		RightPointerFinger_Leap,
		Waist,
		Eyes
	}
	
	public enum TrialParadigm{
		Avatar3D,
		Screen2D
	}

	public enum CharacterColourType{
		Dynamic,
		Static
	}

	public enum VisualInterface{
		CharacterVisible_C,
		CharacterVisible_L,
		CharacterVisible_T,
		CharacterVisible_R,
		CharacterVisible_B,
		CharacterSmooth,
		ColourSmooth,
		Labels,
		Rest,
		Status,
		Progress,
		Framerate,
		AnimateTargets,
		Score,
		Environment3D,
		Interface3D,
		RenderTexture2D,
		ActionObservation,
		RecordTrajectory,
		Countdown,
		ParticleFX,
		ColourLerp,
		Vibration,
		Audio,
		SpatialAudio
	}

	public enum TrialSettingsValue{
		TrialBlocks,
		Repetitions,
		StartDelay,
		InterBlockRestPeriod,
		RestDurationMin,
		RestDurationMax,
		TargetDuration,
		Fixation,
		Arrow,
		Observation
	}
	public enum TrialSettings{
		ActionObservation,
		RecordTrajectory
	}
	public enum Feedback{
		AnimateTarget,
		ParticleFX,
		ColourLerp,
		Vibration,
		Audio,
		SpatialAudio
	}

	public enum FeedbackType{
		OneShot,
		Dynamic
	}

	public enum HapticFeedback{
		AnimateTarget,
		ParticleFX,
		ColourLerp
	}

	public enum GameStatus{
		Null,
		Orientation,
		Preparation,
		Ready,
		Countdown,
		
		RunningTrials,
		BlockComplete,
		AllBlocksComplete,
		RunComplete,
		AllRunsComplete,
		GameComplete,
		Complete
	}

	public enum UserInputType{
		Start,
		Stop,
		Sensel
	}

}