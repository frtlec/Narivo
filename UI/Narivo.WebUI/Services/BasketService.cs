using Narivo.WebUI.Models;

namespace Narivo.WebUI.Services;

public class BasketService
{
    private readonly List<BasketItem> _items = new();

    public event Action OnChange;
    public event Action OnOpenBasket; // Bu event basket açmayı tetikleyecek

    public void AddToBasket(BasketItem item)
    {
        _items.Add(item);
        NotifyStateChanged();
        OpenBasket(); // ürün eklenince basket açılır
    }

    public List<BasketItem> GetBasketItems() => _items;

    public void ClearBasket()
    {
        _items.Clear();
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();

    private void OpenBasket() => OnOpenBasket?.Invoke();
}