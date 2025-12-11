using UISpaces;

public class Merchant : UIObject1
{
    public int shopId;

    private bool isInit;
    
    public override void OnInteract()
    {
        base.OnInteract();

        if (isInit) return;
        isInit = true;
        // TODO: Shop
        UI_Shop shop = ui as UI_Shop;

        if (shop != null) shop.Init(shopId);
    }
}
