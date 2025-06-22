using System.Text.Json.Serialization;
using FluentResults;

namespace Jazer.Server.Models;

public sealed class ErrorResponse
{
    [JsonPropertyName("errors")]
    public string[] Errors { get; private init; }

    public ErrorResponse(IEnumerable<IError> errors)
        : this(errors.SelectMany(x => x.Reasons).Select(x => x.Message).ToArray())
    {
    }

    public ErrorResponse(string error) : this([error])
    {
    }

    public ErrorResponse(string[] errors)
    {
        Errors = errors;
    }
}