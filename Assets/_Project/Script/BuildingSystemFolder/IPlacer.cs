public interface IPlacer
{
    public void StartPlacing(PropSo propSo);
    public void TryPlacing();
    public void TryRotating();
    public void StopPlacing();
}