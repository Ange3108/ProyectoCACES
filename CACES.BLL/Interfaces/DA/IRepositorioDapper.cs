using Microsoft.Data.SqlClient;

namespace CACES.IBLL.Interfaces.DA
{
    public interface IRepositorioDapper
    {
        SqlConnection ObtenerRepositorio();

    }
}
