public class SBoxModel:BaseManager<SBoxModel>
{
    public int macId;
    public int coinInFrame;
    public int coinOutFrame;
    public int curCoinOutNum;
    public int totalCoinOutNum;
    public bool isSboxReady;
    public bool isSboxSandboxReady;
    public bool coinOuting;
}

public class CoinInData
{
    public int id;
    public int coinNum;
}

public class CoinOutData
{
    public int id;
    public int coinNum;
}
