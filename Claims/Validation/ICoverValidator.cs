namespace Claims.Validation
{
    public interface ICoverValidator
    {
        IDictionary<string, string[]> Validate(Cover cover);
    }
}
