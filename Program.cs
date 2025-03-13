var builder = WebApplication.CreateBuilder(args);

builder
    .AddGraphQL()
    .AddTypes()
    .AddInMemorySubscriptions();

var app = builder.Build();

app.MapGraphQL();

app.UseRouting();

app.UseWebSockets();

app.RunWithGraphQLCommands(args);
