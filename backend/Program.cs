using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.GraphQL.Queries;
using LibraryApi.GraphQL.Mutations;
using LibraryApi.GraphQL.Subscriptions;
using LibraryApi.GraphQL.Types;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlite("Data Source=library.db"));

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddType<AuthorType>()
    .AddType<BookType>()
    .AddType<ReviewType>()
    .AddType<CategoryType>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddInMemorySubscriptions();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    context.Database.EnsureCreated();
    DataSeeder.Seed(context);
}

app.UseCors();
app.UseWebSockets();
app.MapGraphQL();

app.Run();
