namespace Claims.Contracts
{
    public class CreateClaimRequest
    {
        public string CoverId { get; init; } = string.Empty;
        public DateTime Created { get; init; }
        public string Name { get; init; } = string.Empty;
        public ClaimType Type { get; init; }
        public decimal DamageCost { get; init; }

        public Claim ToClaim() => new()
        {
            CoverId = CoverId,
            Created = Created,
            Name = Name,
            Type = Type,
            DamageCost = DamageCost
        };
    }
}
