using Postcodes;
using Postcodes.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddControllers()
            .AddJsonOptions(opts => {
                opts.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            });

builder.Services.AddMvc()
             .AddJsonOptions(options => {
                 options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IPostcodeApiService, PostcodeApiService>();
builder.Services.AddHttpClient<IPostcodeApiService, PostcodeApiService>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("baseApiUrl"));
});

builder.Services.AddResponseCaching();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseResponseCaching();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.RunApiHealthCheck();

app.Run();
