namespace Claims.Auditing
{
    public interface IAuditer
    {
        void AuditClaim(string id, string httpReqType);
        void AuditCover(string id, string httpReqType);
    }
}
