using Narivo.Shared.Exceptions;
using Refit;
using System.Net;

namespace Narivo.Shared.Helpers;


public static class RefitWrapper
{
    /// <summary>
    /// Refit çağrısını güvenli şekilde sarmalar.
    /// Başarılıysa content döner, hata varsa AppException fırlatır.
    /// </summary>
    public static async Task<T> ExecuteAsync<T>(Func<Task<ApiResponse<T>>> action)
    {
        try
        {
            var response = await action();

            if (response.IsSuccessStatusCode)
            {
                if (response.Content == null)
                    throw new AppException("Sunucudan beklenen veri gelmedi.", HttpStatusCode.NoContent, "NO_CONTENT");

                return response.Content;
            }

            // Refit response'dan hata bilgisi
            var errorBody = response.Error?.Content ?? "Sunucudan hata yanıtı alındı.";
            throw new AppException(errorBody, response.StatusCode, $"HTTP_{(int)response.StatusCode}");
        }
        catch (ApiException apiEx)
        {
            // Refit özel hata tipi
            throw new AppException(apiEx.Content ?? apiEx.Message, apiEx.StatusCode, "API_EXCEPTION");
        }
        catch (Exception ex)
        {
            // Beklenmeyen hata
            throw new AppException(ex.Message, HttpStatusCode.InternalServerError, "UNEXPECTED_ERROR");
        }
    }
}
