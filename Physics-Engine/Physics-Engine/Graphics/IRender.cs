namespace Physics_Engine.Graphics
{
    public interface IRender
    {
        int GetSortingOrder();
        LayerMask GetLayerMask();
        void Draw();
    }
}