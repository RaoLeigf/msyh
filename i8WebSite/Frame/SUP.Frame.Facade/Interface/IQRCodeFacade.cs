using System.Data;

namespace SUP.Frame.Facade
{
    public interface IQRCodeFacade
    {
        DataTable getUrlByCode(string code);
    }
}
