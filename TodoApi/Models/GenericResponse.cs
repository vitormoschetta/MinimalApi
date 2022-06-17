namespace TodoApi.Models;
public class GenericResponse
{
    public GenericResponse(string message, object data = null)
    {
        Message = message;
        Data = data;
    }

    public GenericResponse(string message)
    {
        Message = message;
    }

    public GenericResponse(Exception ex)
    {
        Message = ex.Message;
    }

    public string Message { get; set; }
    public object Data { get; set; }
}
