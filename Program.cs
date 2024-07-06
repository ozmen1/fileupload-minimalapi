var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/upload", async (HttpContext context) =>
{
    var form = await context.Request.ReadFormAsync();
    var file = form.Files["file"];
    var savePath = form["savePath"];

    if (file == null || file.Length == 0)
    {
        return Results.BadRequest(new { message = "No file uploaded" });
    }

    if (string.IsNullOrEmpty(savePath))
    {
        return Results.BadRequest(new { message = "Save path not provided" });
    }

    var filePath = Path.Combine(savePath, file.FileName);
    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    return Results.Ok(new { message = "File uploaded successfully", filePath });
});

app.Run();


//to use: curl -F "file=@C:/Users/Viya/Desktop/log.txt" -F "savePath=C:/uploads" http://localhost:5296/upload