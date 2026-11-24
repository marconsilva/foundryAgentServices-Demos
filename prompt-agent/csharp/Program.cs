using Azure.Identity;
using Azure.AI.Projects;
using Azure.AI.Projects.Agents;
using Azure.AI.Extensions.OpenAI;
using OpenAI.Responses;

// Format: "https://resource_name.services.ai.azure.com/api/projects/project_name"
var foundryProjectEndpoint = "https://aislivethoughtsakl4eacfa7.services.ai.azure.com/api/projects/ai-demos"; // "Replace with your project endpoint";
var foundryAgentName = "promptAgent-Demo";

// Create project client to call Foundry API
var credential = new AzureCliCredential();
AIProjectClient projectClient = new(
    endpoint: new Uri(foundryProjectEndpoint),
    tokenProvider: credential);

// Create an agent with a model and instructions
ProjectsAgentDefinition agentDefinition = new DeclarativeAgentDefinition("gpt-5-mini") // supports all Foundry direct models
{
    Instructions = "You are a helpful assistant that answers general questions",
};

ProjectsAgentVersion agent = projectClient.AgentAdministrationClient.CreateAgentVersion(
    foundryAgentName,
    options: new(agentDefinition));
Console.WriteLine($"Agent created (id: {agent.Id}, name: {agent.Name}, version: {agent.Version})");


#pragma warning disable OPENAI001, AAIP002 // Opt in to the SDK's experimental conversation and response APIs.
ProjectOpenAIClient openAIClient = new(new Uri(foundryProjectEndpoint), credential);

// Create a conversation for multi-turn chat
var conversation = openAIClient.GetProjectConversationsClient().CreateProjectConversation().Value;

// Chat with the agent to answer questions
ProjectResponsesClient responsesClient = openAIClient.GetProjectResponsesClientForAgent(
    defaultAgent: foundryAgentName,
    defaultConversationId: conversation.Id);
ResponseResult response = responsesClient.CreateResponse("What is the size of France in square miles?");
Console.WriteLine(response.GetOutputText());

// Ask a follow-up question in the same conversation
response = responsesClient.CreateResponse("And what is the capital city?");
Console.WriteLine(response.GetOutputText());
#pragma warning restore OPENAI001, AAIP002
