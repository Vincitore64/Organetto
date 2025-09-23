namespace Organetto.UseCases.Boards.Columns.Cards.Data
{
    public sealed record UploadFile(Stream Content, string FileName, long Length);

}
