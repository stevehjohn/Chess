// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Engine.LiChessClient.Client.Models;

[UsedImplicitly]
public class ChallengeResponse
{
    public string Id { get; set; }
    
    public string Status { get; set; }
}