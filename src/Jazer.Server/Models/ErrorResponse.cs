using System.Text.Json.Serialization;
using FluentResults;

namespace Jazer.Server.Models;

// TODO: revisit
public sealed class ErrorResponse(IEnumerable<IError> errors)
{
    [JsonPropertyName("errors")]
    public IEnumerable<IError> Errors { get; } = errors;
}