namespace Enigma_Framework.Graphics;

public interface IRender
{
    int GetSortingOrder();
    LayerMask GetLayerMask();
    void Draw();
}