namespace CHESS2THESEQUELTOCHESS.scripts.core.buffs;

public interface IBuff
{
    public bool ConditionsMet();

    public void Execute();
}
