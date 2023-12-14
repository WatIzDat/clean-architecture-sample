using Application.Abstractions.Data;
using Domain.Followers;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Interceptors;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .Scan(
        selector => selector
            .FromAssemblies(
                Infrastructure.AssemblyReference.Assembly)
            .AddClasses(false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

builder.Services.AddScoped<IFollowerService, FollowerService>();
//builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(Domain.AssemblyReference.Assembly, Application.AssemblyReference.Assembly);
});

builder.Services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

builder.Services.AddLogging();

builder
    .Services
    .AddInfrastructure(builder.Configuration);

builder.Services.AddControllers().AddApplicationPart(Presentation.AssemblyReference.Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
