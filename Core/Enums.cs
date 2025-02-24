namespace Barebone.Core;

public enum NpcState
{
    Idle,
    Talk,
    Use,
    Take
}

public enum ItractiveItemStates
{
    Init,
    Use,
}

public enum IventoryItemStates
{
    Active,
    Pickup,
}

/// <summary>
/// Enum describes the screen transition state.
/// </summary>
public enum ScreenState
{
    TransitionOn,
    Active,
    TransitionOff,
    Hidden
}

public enum CursorTextureType
{
    Pointer,
    Interact,
    Talk,
    Walk,
    Exit,
}