namespace PropBehaviours
{
    public interface IID
    {
        int ID => this.GetHashCode();
    }
}