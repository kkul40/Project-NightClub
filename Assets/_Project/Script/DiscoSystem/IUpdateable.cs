namespace DiscoSystem
{
    public interface IUpdateable
    {
        virtual void TickUpdate(float deltaTime){}
        virtual void TickFixedUpdate(float deltaTime){}
    }
}