using Marzi.Client.Core.Models;
using Refit;

namespace Marzi.Client.Infrastructure.Api;

public interface IChatApi
{
    [Post("/api/chat/messages")]
    Task<ApiResponse<SendChatMessageResponse>> SendAsync(
        [Body] SendChatMessageRequest request);

    [Get("/api/chat/messages/{conversationId}")]
    Task<ApiResponse<List<ChatMessageDto>>> GetMessagesAsync(
        Guid conversationId);

    [Get("/api/chat/conversations")]
    Task<ApiResponse<List<ChatConversationDto>>> GetConversationsAsync();

    [Post("/api/chat/reply")]
    Task<IApiResponse> ReplyAsync([Body] ReplyChatRequest request);
}

public sealed class SendChatMessageRequest
{
    public Guid ConversationId { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? Body { get; set; }
}

public sealed class SendChatMessageResponse
{
    public Guid ConversationId { get; set; }
}

public sealed class ReplyChatRequest
{
    public Guid ConversationId { get; set; }

    public string? Body { get; set; }
}
