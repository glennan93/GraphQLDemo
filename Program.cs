using GraphQLDemo.DataLoaders;
using GraphQLDemo.Services;
using GraphQLDemo.Services.Courses;
using GraphQLDemo.Services.Instructors;
using Microsoft.EntityFrameworkCore;
using FirebaseAdminAuthentication.DependencyInjection.Extensions;
using FirebaseAdmin;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("default");
builder.Services.AddPooledDbContextFactory<SchoolDbContext>(o => o.UseSqlite(connectionString).LogTo(Console.WriteLine));

builder
    .AddGraphQL()
    .AddTypes()
    .AddInMemorySubscriptions()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddAuthorization();

builder.Services.AddSingleton(FirebaseApp.Create());

builder.Services.AddScoped<CoursesRepository>();
builder.Services.AddScoped<InstructorsRepository>();
builder.Services.AddScoped<InstructorDataLoader>();
builder.Services.AddDbContext<SchoolDbContext>();
builder.Services.AddFirebaseAuthentication();


var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IDbContextFactory<SchoolDbContext> contextFactory =
        scope.ServiceProvider.GetRequiredService<IDbContextFactory<SchoolDbContext>>();

    using(SchoolDbContext context = contextFactory.CreateDbContext())
    {
        context.Database.Migrate();
    }
};


app.MapGraphQL();

app.UseRouting();

app.UseAuthentication();

app.UseWebSockets();

app.RunWithGraphQLCommands(args);
