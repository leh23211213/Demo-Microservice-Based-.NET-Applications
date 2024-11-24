namespace App.Services.OrderAPI;
public class SnowflakeIdGenerator
{
    private const long Epoch = 1672531200000L; // Thời gian bắt đầu (Jan 1, 2023 in milliseconds)
    private const int DatacenterIdBits = 5;   // Số bit cho Datacenter ID
    private const int WorkerIdBits = 5;       // Số bit cho Worker ID
    private const int SequenceBits = 12;      // Số bit cho Sequence

    private const long MaxDatacenterId = (1L << DatacenterIdBits) - 1;
    private const long MaxWorkerId = (1L << WorkerIdBits) - 1;
    private const long MaxSequence = (1L << SequenceBits) - 1;

    private const int WorkerIdShift = SequenceBits;
    private const int DatacenterIdShift = SequenceBits + WorkerIdBits;
    private const int TimestampShift = SequenceBits + WorkerIdBits + DatacenterIdBits;

    private readonly object _lock = new();
    private long _lastTimestamp = -1L;
    private long _sequence = 0L;

    public long DatacenterId { get; }
    public long WorkerId { get; }

    public SnowflakeIdGenerator(long datacenterId, long workerId)
    {
        if (datacenterId < 0 || datacenterId > MaxDatacenterId)
            throw new ArgumentOutOfRangeException(nameof(datacenterId), $"DatacenterId must be between 0 and {MaxDatacenterId}");

        if (workerId < 0 || workerId > MaxWorkerId)
            throw new ArgumentOutOfRangeException(nameof(workerId), $"WorkerId must be between 0 and {MaxWorkerId}");

        DatacenterId = datacenterId;
        WorkerId = workerId;
    }

    public long GenerateId()
    {
        lock (_lock)
        {
            var timestamp = GetCurrentTimestamp();

            if (timestamp < _lastTimestamp)
                throw new InvalidOperationException("Clock moved backwards. Refusing to generate id");

            if (timestamp == _lastTimestamp)
            {
                _sequence = (_sequence + 1) & MaxSequence;

                if (_sequence == 0)
                {
                    // Sequence exhausted, wait for the next millisecond
                    timestamp = WaitForNextTimestamp(_lastTimestamp);
                }
            }
            else
            {
                _sequence = 0;
            }

            _lastTimestamp = timestamp;

            return ((timestamp - Epoch) << TimestampShift) |
                   (DatacenterId << DatacenterIdShift) |
                   (WorkerId << WorkerIdShift) |
                   _sequence;
        }
    }

    private static long GetCurrentTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    private static long WaitForNextTimestamp(long lastTimestamp)
    {
        var timestamp = GetCurrentTimestamp();
        while (timestamp <= lastTimestamp)
        {
            timestamp = GetCurrentTimestamp();
        }
        return timestamp;
    }
}