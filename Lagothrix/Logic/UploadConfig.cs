namespace Logic;
public record UploadConfig
{
    public string FileSavePath { get; set; }
    public ICollection<string> SupportedFormats { get; set; }
}
