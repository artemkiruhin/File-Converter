namespace FileConverter.Services.Converters.Base {
    public interface IConverter {
        byte[] Convert(string textToConvert);
    }
}
