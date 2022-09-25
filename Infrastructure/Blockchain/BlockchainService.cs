using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Net;

namespace Infrastructure.Blockchain
{
    public class BlockchainService : IBlockchainService
    {
        public BlockchainService()
        {
        }

        //https://docs.etherscan.io/api-endpoints/accounts

        public async Task GetAssetTokens(CancellationToken cancellationToken = default)
        {
            string apiKey = "3GTWHKTC6GWVB4U7SF7A3EUW5XNJXYTI52";
            var url = "https://api-rinkeby.etherscan.io/";
            // 這個網路的私鑰
            var privateKey = "0xaa0ac0da5a336360c8517fdca341642accc55a8c318d536b302c6f93c95179ff";
            var account = new Account(privateKey);
            var web3 = new Web3(account, url);

            string abi = string.Empty;
            using (var sr = new StreamReader(Path.Combine(
                   Directory.GetCurrentDirectory(), "wwwroot/abi",
                   "abi.txt")))
            {
                abi = sr.ReadToEnd();
            }

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var web3v1 = new Web3("https://api-rinkeby.etherscan.io/");

                var balance = await web3v1.Eth.GetBalance.SendRequestAsync(account.Address);
                var value = Web3.Convert.FromWeiToBigDecimal(balance);
                var str = value.ToString();

                var contract = web3.Eth.GetContract(abi, "0x9E8Bc1F415B627d29e193a0A735341d299A69418");
                var transferFunction = contract.GetFunction("tokenPrice").GetData();
                //var s = contract.GetFunction("sybol").GetData();
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                var ss = await contract.GetFunction("symbol").CallAsync<string>();

                //var balance = await web3.Eth.GetBalance.SendRequestAsync("0xc8Bd59d60961bEdb12c95079250d3075b9328a2d");
            }
            catch (Exception ex)
            {
            }
        }
    }
}