using System.Text.Json.Serialization;
using FluentResults;

namespace Jazer.Server.Models;

public sealed class ErrorResponse(IEnumerable<IError> errors)
{
    [JsonPropertyName("errors")]
    public string[] Errors => errors.SelectMany(x => x.Reasons).Select(x => x.Message).ToArray();
}