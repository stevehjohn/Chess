// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Engine.LiChessClient.Client.Models;

[UsedImplicitly]
public class StreamResponse
{
    [JsonPropertyName("white")]
    public Player White { get; set; }
    
    [JsonPropertyName("black")]
    public Player Black { get; set; }
    
    [JsonPropertyName("state")]
    public GameState State { get; set; }
}