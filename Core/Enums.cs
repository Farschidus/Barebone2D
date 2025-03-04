namespace Barebone.Core;

public enum GameObjectType
{
    Npc,
    Item,
    Exit,
    Player,
    Particle,
}
public enum NpcState
{
    Idle,
    Talk,
    Use,
    Take
}

public enum ItractiveItemState
{
    Init,
    Use,
}

public enum IventoryItemState
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