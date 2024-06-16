using Microsoft.JSInterop;

namespace TerminalsService.Front.Helpers;

public sealed class StyleHelper : IStyleHelper
{
    private IJSRuntime _js;

    public StyleHelper(IJSRuntime js)
    {
        _js = js;
    }

    public async Task OpenMenuAsync()
    {
        await _js.InvokeVoidAsync("openNav");
    }

    public async Task CloseMenuAsync()
    {
        await _js.InvokeVoidAsync("closeNav");
    }}