namespace ImageViewer;

class FileManager : IFileManager
{
    private string[]? _filesList;
    private int _index = -1;

    public string next()
    {
        if (_filesList == null) return String.Empty;
        _index = (_index + 1) % _filesList.Length;
        return _filesList[_index];
    }

    public string prev()
    {
        if (_filesList == null) return String.Empty;
        _index--;
        if (_index < 0) _index = _filesList.Length - 1;
        return _filesList[_index];
    }

    public string head()
    {
        if (_filesList == null) return String.Empty;
        _index = 0;
        return _filesList[_index];
    }

    public string tail()
    {
        if (_filesList == null) return String.Empty;
        _index = _filesList.Length-1;
        return _filesList[_index];
    }

    public void setFileName(string? filename)
    {
        if (filename == null || !File.Exists(filename)) return;
        string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", "ico" }; 
        
        _filesList = Directory.GetFiles(Path.GetDirectoryName(@filename) ?? string.Empty)
            .Where(file => imageExtensions.Contains(Path.GetExtension(file).ToLower()))
            .Select(Path.GetFullPath).ToArray();

        Array.Sort(_filesList);
        _index = Array.IndexOf(_filesList, filename);
    }
}