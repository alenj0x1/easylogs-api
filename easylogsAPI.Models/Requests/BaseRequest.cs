namespace easylogsAPI.Models.Requests;

public class BaseRequest
{
    private int _limit;
    private int _offset;
    private const int MaxLimit = 100;

    public int Offset
    {
        get => _offset = (_limit <= 0) ? 0 : _offset;
        set => _offset = (value <= -1) ? 0 : value;
    }

    public int Limit
    {
        get => _limit = _limit <= 0 ? 10 : _limit;
        set => _limit = (value > MaxLimit) ? MaxLimit : (value <= 0) ? 0 : value;
    }
}