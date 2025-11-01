namespace PureMasker;

public class MaskOptions
{
    public int? ShowFirst { get; set; }
    public int? ShowLast { get; set; }
    public char MaskChar { get; set; } = '*';
    
    public static MaskOptions Default => new MaskOptions { ShowFirst = 2, ShowLast = 2 };
}
