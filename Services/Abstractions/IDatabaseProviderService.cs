using LiteDB;

namespace SpotPG.Services.Abstractions
{
    public interface IDatabaseProviderService
    {
        ILiteDatabase Database { get; }
    }
}