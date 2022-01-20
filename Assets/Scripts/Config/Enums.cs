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
	    TrialSequenceStarted,
	    PreTrialPhase,
	    Initialise,
		Fixation,
		Indication,
		TargetPresentation,
		Observation,
		Rest,
		Complete,
		PostTrialPhase,
		TrialComplete,
		TrialSequenceComplete,
		Debug
    }
	public enum RigType {AvatarRig, GhostRig}
	
	public enum TrialParadigm{
		Null,
		Horizontal,
		Vertical,
		Circle,
		CentreOut,
		RandomPosition
	}

	public enum SequenceType{
		Linear,
		Permutation
	}
	public enum Handedness{
		Left,
		Right
	}

	public enum ParadigmTargetCount{
		One,Two,Three,Four,Eight,Sixteen
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
	
	// public enum TrialParadigm{
	// 	Avatar3D,
	// 	Screen2D
	// }

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
		Runs,
		BlocksPerRun,
		Repetitions,
		Countdown,
		visibleCountdown,
		InterRunRestPeriod,
		preTrialWaitPeriod,
		TargetPresentationPeriod,
		FixationPeriod,
		IndicationPeriod,
		ObservationPeriod,
		RestPeriodMin,
		RestPeriodMax,
		PostTrialWaitPeriod,
		PostBlockWaitPeriod,
		PostRunWaitPeriod
	}

	public enum SessionMetric{
		TrialsPerBlock,
		TrialsPerRun,
		TrialsPerSession,
		EstimatedTrialDuration,
		EstimatedBlockDuration,
		EstimatedRunDuration,
		EstimatedSessionDuration
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
		Initialised,
		Orientation,
		Preparation,
		WaitingForInput,
		SetStartButton,
		DisplayBlockMenu,
		DisplayRunMenu,
		DisplayMenu,
		Ready,
		Countdown,
		VisibleCountdown,
		CountdownComplete,
		RunningTrials,
		BlockStarted,
		BlockComplete,
		AllBlocksComplete,
		RunStarted,
		RunComplete,
		AllRunsComplete,
		GameComplete,
		Complete,
		Paused,
		Unpaused,
		Reset,
		Generic,
		Progress,
		Debug
	}

	public enum UserInputType{
		Start,
		Stop,
		Pause,
		Reset,
		Sensel
	}

	public enum BCI_ControlType
	{
		Velocity,
		ForceVelocity,
		Translate,
		Position
	}

	public enum RunType
	{
		Kinematic,
		Imagined
	}

}