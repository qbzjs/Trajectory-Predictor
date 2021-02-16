
namespace Enums 
{
    public enum TaskSide {
        Left,Right
    }
    public enum TrialType {
        Three_Targets,
        Four_Targets,
        CentreOut
    }

    public enum CharacterColourType {
        Dynamic,
        Static
    }

    public enum InterfaceElement {
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
        Score
    }

    public enum SettingsValue {
        Repetitions,
        StartDelay,
        RestDuration,
        TargetDuration
    }
    public enum GameStatus{Ready, Running, Complete}
    
    public enum UserInput{GamePad_A, GamePad_B, GamePad_X, GamePad_Y, L_Button, R_Button, Up, Down, Left, Right}
}
