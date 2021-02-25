namespace Enums
{
	public enum TrialType{
		Three_Targets,
		Four_Targets,
		CentreOut
	}

	public enum TaskSide{
		Left,
		Right
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
		ParticleFX,
		ColourLerp,
		Vibration,
		Audio,
		SpatialAudio
	}

	public enum TrialSettingsValue{
		Repetitions,
		StartDelay,
		RestDuration,
		TargetDuration,
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
		Ready,
		Running,
		Complete
	}

	public enum UserInput{
		GamePad_A,
		GamePad_B,
		GamePad_X,
		GamePad_Y,
		L_Button,
		R_Button,
		Up,
		Down,
		Left,
		Right
	}
}