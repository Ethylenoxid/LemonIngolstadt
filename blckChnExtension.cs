using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;
using System.Text;






namespace Lemon
{
    public class Block  
    {  
        public int Index { get; set; }  
        public DateTime TimeStamp { get; set; }  
        public string PreviousHash { get; set; }  
        public string Hash { get; set; }  
        public string Data { get; set; }  
        public Block(DateTime timeStamp, string previousHash, string data)  
        {  
            Index = 0;  
            TimeStamp = timeStamp;  
            PreviousHash = previousHash;  
            Data = data;  
            Hash = CalculateHash();  
        }  
        public string CalculateHash()  
        {  
            SHA256 sha256 = SHA256.Create();  
            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{Data}");  
            return CalcHash(inputBytes, sha256);
        }
	private string CalcHash(byte[] inputBytes, SHA256 sha256)
	{
	    Console.WriteLine("in CalcHash: inputBytes="+inputBytes);//DEV
	    Console.WriteLine("in CalcHash: inputBytes="+Convert.ToBase64String(inputBytes));//DEV
	    byte[] outputBytes = sha256.ComputeHash(inputBytes);//ori
	    return Convert.ToBase64String(outputBytes);//ori
	    //return "b918b1e89ff06dfc209ec7ab313e19353bfc29e41b469ac12263497b1d157dcd";//test
	}
    }  
    public class Blockchain  
    {  
        public IList<Block> Chain { set;  get; }
        public Blockchain()  
        {  
            InitializeChain();  
            AddGenesisBlock();  
        }
        public void InitializeChain()  
        {  
            Chain = new List<Block>();  
        }  
        public Block CreateGenesisBlock()  
        {  
            return new Block(DateTime.Now, null, "{}");  
        }  
        public void AddGenesisBlock()  
        {  
            Chain.Add(CreateGenesisBlock());  
        }  
        public Block GetLatestBlock()  
        {  
            return Chain[Chain.Count - 1];  
        }  
        public void AddBlock(Block block)  
        {  
            Block latestBlock = GetLatestBlock();  
            block.Index = latestBlock.Index + 1;  
            block.PreviousHash = latestBlock.Hash;  
            block.Hash = block.CalculateHash();
            Chain.Add(block);  
        }
	public void PrintBlockchain()
	{
	    Console.WriteLine("Blockchain object: ");
	    for (var i = 0; i < Chain.Count; i++)
	    {
	        int j=i+1;
	        Console.WriteLine("element{0}: Index={1}, TimeStamp={2}, Hash={3}, PreviousHash={4}, Data={5}", j, Chain[i].Index, Chain[i].TimeStamp, Chain[i].Hash, Chain[i].PreviousHash, Chain[i].Data);
	    }
	}
        public bool IsValid()  
        {  
            for (int i = 1; i < Chain.Count; i++)  
            {  
                Block currentBlock = Chain[i];  
                Block previousBlock = Chain[i - 1];  
                if (currentBlock.Hash != currentBlock.CalculateHash())  
                {  
                    return false;  
                }  
                if (currentBlock.PreviousHash != previousBlock.Hash)  
                {  
                    return false;  
                }  
            }  
            return true;  
        }
        public bool IsValidW()
	{
	    if (!IsValid())
	    {
	         Console.WriteLine("WARNING: Blockchain invalid");
	         return false;
	    }
            return true;
	}
	public bool IsValidE(string hash2compare, int i_chain=-1, bool withWarning=false)
	{
	    if (i_chain==-1)
	    {
	        i_chain+=Chain.Count;
	    }
            Block block2check = Chain[i_chain]; 
            if (block2check.Hash != hash2compare)  
            {
	        if (withWarning)
		{
		    Console.WriteLine("WARNING: Blockchain invalid");
		}
                return false;  
            }
            return true;  
	}





    } 
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Creating 1st blockchain");
	    Blockchain phillyCoin = new Blockchain();
	    bool useDefaultChain = true;
            Console.WriteLine("");
	    string hashToCheck="test";
	    if (useDefaultChain)//default value for having a test case
	    {
	        phillyCoin.AddBlock(new Block(DateTime.Now, null, "{sender:Beckenbauer,receiver:Rummenigge,amount:10}"));
	        phillyCoin.AddBlock(new Block(DateTime.Now, null, "{sender:Rummenigge,receiver:Hoeneß,amount:10}"));
	    }
	    else
	    {
	        //to implement: input of block chain;
	        //to implement: reading input value for "hashToCheck" from unity system, validate the certificate
		//ionteraction with test system in unity ("digital twin").
	    }
            Console.WriteLine("Block chain to be checked:");
	    phillyCoin.PrintBlockchain();
            Console.WriteLine("Checking if certiuficate is valid ...");
	    if (phillyCoin.IsValidE(hashToCheck))
	    {
                Console.WriteLine("Your certificate is valid. Process can be continued ...");
	    }
	    else
	    {
                Console.WriteLine("Your certificate is invalid");
	    }
        }
    }
}
