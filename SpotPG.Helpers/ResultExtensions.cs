using System;
using System.Linq;
using FluentResults;

namespace SpotPG.Helpers
{
    public static class ResultExtensions
    {
        public static string GetErrorsText<T>(this Result<T> result, string joinWith = ", ")
            => String.Join(joinWith, result.Errors.Select(e => e.Message));
    }
}