using CinemaCriticApp.Client;
using CinemaCriticApp.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Headers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7061/") });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://api.themoviedb.org/3/") });
//builder.Services.AddHttpClient<IMoviesService, MoviesService>(client =>
//{
//    client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
//});

//builder.Services.AddHttpClient<ICommentService, CommentService>(client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7061/");
//});

builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IMoviesService, MoviesService>();

await builder.Build().RunAsync();
