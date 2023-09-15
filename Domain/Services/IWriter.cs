namespace backend_test.Domain.Services;

public interface IWriter<T>
{
    void WriteToFile(List<T> list, string filePath, string fileName);
}