using Barebone.Components;
using Barebone.Managers;

namespace Barebone.Systems;

public class InteractiveSystem(InteractiveComponent interactiveComponent)
{
    private readonly InteractiveComponent _interactiveComponent = interactiveComponent;

    public bool IsClicked(InputManager input) => input.IsMouseClicked && _interactiveComponent.InteractiveArea.Contains(input.CurrentMousePoint);
}
