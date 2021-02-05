
namespace Enums 
{
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
        CharacterVisible_C,CharacterVisible_L, CharacterVisible_T, CharacterVisible_R, CharacterVisible_B, CharacterSmooth, ColourSmooth, Labels,Rest,Status,Progress,Framerate, AnimateTargets, Score
    }

    public enum SettingsValue {
        Repetitions,
        StartDelay,
        RestDuration,
        TargetDuration
    }
    public enum GameStatus{Ready, Running, Complete}
}
