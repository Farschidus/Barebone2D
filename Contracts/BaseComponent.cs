namespace Barebone.Contracts;

public abstract class BaseComponent(string name, float layerDepth)
{
    public string Name { get; private set; } = name;
    public float LayerDepth { get; private set; } = layerDepth;
}