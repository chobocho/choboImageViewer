namespace ImageViewer;

public interface IFileManager
{
    public string head();
    public string tail();
    public string next();
    public string prev();
    public void setFileName(string filename);
}