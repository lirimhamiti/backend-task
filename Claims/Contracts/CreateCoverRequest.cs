namespace Claims.Contracts
{
    public class CreateCoverRequest
    {
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public CoverType Type { get; init; }

        public Cover ToCover() => new()
        {
            StartDate = StartDate,
            EndDate = EndDate,
            Type = Type
        };
    }
}
