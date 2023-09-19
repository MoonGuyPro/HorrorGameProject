public enum LogicGate
{
    AND,
    OR,
    XOR,
}

public enum BinaryState
{
    ON,
    OFF,
}

public struct Signal
{
    public BinaryState state;
    int id;
    
    public Signal(BinaryState state, int id)
    {
        this.state = state;
        this.id = id;
    }
    
    public int GetID()
    {
        return id;
    }
}

public enum InteractionType
{
    Trigger,
    Toggle,
}

public enum ToggleState
{
    On,
    Off,
}