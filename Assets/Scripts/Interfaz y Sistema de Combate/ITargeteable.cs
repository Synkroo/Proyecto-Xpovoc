public interface ITargetable
{
    void OnHoverEnter();
    void OnHoverExit();
    void OnSelected();

    string TargetName { get; }
}
