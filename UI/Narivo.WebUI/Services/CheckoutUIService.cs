
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop;
using Narivo.Checkout.Core.Business.Dtos.RequestDtos;
using Narivo.Shared.Helpers;
using Narivo.WebUI.HttpClients;
using Narivo.WebUI.StateServices;

namespace Narivo.WebUI.Services;


public class CheckoutUIService
{
    private readonly ICheckoutApiClient _checkoutApiClient;
    private readonly OrderUIService _orderUIService;
    private readonly BasketService _basketService;
    private readonly NavigationManager _navigation;
    private readonly AppState _appState;
    private readonly IJSRuntime _js;
    private int MemberId;

    public CheckoutUIService(ICheckoutApiClient checkoutApiClient, BasketService basketService, NavigationManager navigation, AppState appState, IJSRuntime js, OrderUIService orderUIService)
    {
        _checkoutApiClient = checkoutApiClient;
        _basketService = basketService;
        _navigation = navigation;
        _appState = appState;
        _js = js;
        _orderUIService = orderUIService;
    }

    public async Task<bool> StartCheckout(CheckoutRequestDto checkoutRequestDto)
    {
        return await RefitWrapper.ExecuteAsync<bool>(() => _checkoutApiClient.Start(checkoutRequestDto));            
    }

    public async Task CheckoutBasket()
    {
        if (!_basketService.GetBasketItems().Any())
        {
            await _js.InvokeVoidAsync("alert", "Sepet boş!");
            return;
        }

        try
        {
            MemberId = _appState.MemberId;
            int? orderId = await _orderUIService.CreateOrder(MemberId, _basketService.GetBasketItems());
            if (orderId.HasValue)
            {
                _basketService.ClearBasket();
                _appState.SetBasketBottomSheetIsOpen(!_appState.BasketBottomSheetIsOpen);
                await Checkout(orderId.Value);
            }
            else
            {
                await _js.InvokeVoidAsync("alert", "Sipariş oluşturulamadı!");
            }
        }
        catch (Exception ex)
        {
            await _js.InvokeVoidAsync("alert", $"Sipariş oluşturulurken hata: {ex.Message}");
        }
    }
    public async Task Checkout(int orderId)
    {
        // ✅ CheckoutPage'e yönlendirme
        _navigation.NavigateTo($"/checkout/{orderId}");
    }
}


