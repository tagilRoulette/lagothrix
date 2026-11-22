namespace Dal;
public class ParameterValueEntity
{
    public Guid Id { get; set; }
    public string Value { get; set; }

    public ParameterEntity Parameter { get; set; }
    public Guid ParameterId { get; set; }
}
