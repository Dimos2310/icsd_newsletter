//ΚΥΡΙΑΚΙΔΗΣ ΔΗΜΟΣΘΕΝΗΣ icsd19102
//ΖΑΓΑΛΙΩΤΗ ΦΩΤΕΙΝΗ icsd19054
//ΓΚΑΤΖΙΑΣ ΣΠΥΡΟΣ icsd19030

using Microsoft.AspNetCore.Diagnostics;
using NewsLetter.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

var app = builder.Build();



app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features
        .Get<IExceptionHandlerPathFeature>()
        .Error;
    var response = new { error = exception.Message };
    await context.Response.WriteAsJsonAsync(response);
}));



UserServices us = new UserServices();
//Console.WriteLine("starting");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

