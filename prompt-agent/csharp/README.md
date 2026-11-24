# C# Prompt Agent Demo

A .NET console app that creates a Foundry prompt agent, starts a conversation, and asks about France's area followed by its capital. The second question uses the same conversation to preserve context.

## Prerequisites

- .NET 10 SDK.
- Azure CLI.
- A Microsoft Foundry project and a deployed model that supports the Responses API.
- **Foundry User** (previously **Azure AI User**) access on the project or its parent Foundry resource for the identity running the app.

## Configure and run

In `Program.cs`, update:

- `foundryProjectEndpoint` to your project endpoint: `https://<resource-name>.services.ai.azure.com/api/projects/<project-name>`.
- The `"gpt-5-mini"` argument in `DeclarativeAgentDefinition` to your exact **model deployment name**, not just the model's catalog name.
- `foundryAgentName` if you want to use a different agent name.

Sign in and select the subscription containing your Foundry resource:

```powershell
az login --tenant "<tenant-id>"
az account set --subscription "<subscription-id>"
```

The demo explicitly uses `AzureCliCredential`, so it authenticates through this Azure CLI session.

From the repository root, run:

```powershell
dotnet run --project .\prompt-agent\csharp\csharp.csproj
```

Dependencies restore automatically. The output shows the agent ID, name, and version, followed by both answers. Running the app creates an agent version and a conversation; it does not delete them afterward. Model usage can incur Azure charges.

## SDK notes

The app references `Azure.AI.Projects`, `Azure.AI.Projects.Agents`, `Azure.AI.Extensions.OpenAI`, and `Azure.Identity`.

The conversation and response APIs are experimental. The demo explicitly suppresses `OPENAI001` and `AAIP002` only around the code using these APIs.

## Troubleshooting

- **403 Forbidden / `agents/write`:** Assign Foundry User to the identity used by Azure CLI at the correct project or resource scope. Allow time for role propagation.
- **404 DeploymentNotFound:** Verify that the deployment name matches the code and that the deployment belongs to the Foundry resource in your endpoint. A new deployment can take a few minutes to become available.

See [Foundry access control](https://learn.microsoft.com/en-us/azure/foundry/concepts/rbac-foundry) for permission details.
