namespace Netscrape.Services.Interfaces;

public interface IHttpClientFacade
{
    Task<string> GetAsync(string uri);
}