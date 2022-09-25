namespace Infrastructure.Blockchain
{
    public interface IBlockchainService
    {
        Task GetAssetTokens(CancellationToken cancellationToken = default);
    }
}