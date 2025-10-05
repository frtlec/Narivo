namespace Narivo.WebUI.StateServices;

public class AppState
{
    public int MemberId { get; private set; }
    public bool BasketBottomSheetIsOpen { get; private set; }

    public event Action OnChange;

    public void SetMemberId(int memberId)
    {
        MemberId = memberId;
        NotifyStateChanged();
    }
    public void SetBasketBottomSheetIsOpen(bool isOpen)
    {
        BasketBottomSheetIsOpen = isOpen;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}