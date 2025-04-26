var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Identity_API>("identity-api");

builder.AddProject<Projects.Transaction_API>("transaction-api");

builder.AddProject<Projects.Category_API>("category-api");

builder.AddProject<Projects.Dashboard_API>("dashboard-api");

builder.AddProject<Projects.ApiGateway>("apigateway");

builder.Build().Run();
