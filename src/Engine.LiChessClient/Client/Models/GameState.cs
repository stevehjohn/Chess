// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Engine.LiChessClient.Client.Models;

[UsedImplicitly]
public class GameState
{
    [JsonPropertyName("moves")]
    public string Moves { get; set; }
    
    [JsonPropertyName("text")]
    public string Text { get; set; }
}