/*
 * Dirty workaround to use InputLogic from code
 */

public class DummyOutput : InputLogic
{
    public void ForceToggle()
    {
        Toggle();
    }
    
    protected override void Behavior()
    {
        throw new System.NotImplementedException();
    }
}