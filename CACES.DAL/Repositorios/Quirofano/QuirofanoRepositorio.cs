using CACES.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Quirofano
{
    public class QuirofanoRepositorio : IQuirofanoRepositorio
    {
        private readonly CACESDbContext dbContext;
         public QuirofanoRepositorio(CACESDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> ActualizarCupoAsync(int nuevoCupo)
            
        {
            var result = await dbContext.ConfiguracionQuirofano.FirstOrDefaultAsync();
            if (result == null) return false;
            result.CupoMaximoDiario = nuevoCupo;

            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetCupoMaximoAsync()
        {
            var result = await dbContext.ConfiguracionQuirofano.FirstOrDefaultAsync();
            if (result == null) return 0;
            return result.CupoMaximoDiario;
        }
    }
}
