using System.Collections.Generic;

public class CapabilityBinding
{
    private readonly Dictionary<CapabilitySigil, Behavior> _bound = new();

    public void Bind(CapabilitySigil capability, Behavior behavior)
    {
        _bound[capability] = behavior;
    }

    public bool Satisfies(CapabilitySigil capability)
    {
        return _bound.TryGetValue(capability, out var behavior) && behavior.satisfies == capability;
    }
}
