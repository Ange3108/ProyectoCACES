using AutoMapper;
using CACES.BLL.DTOs.HorariosDisponibles;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Horarios;

namespace CACES.BLL.Servicios.Horarios
{
    public class HorarioServicio : IHorarioServicio
    {
        private readonly IHorariosRepositorio _horariosRepositorio;
        private readonly IMapper _mapper;
        public HorarioServicio(IHorariosRepositorio horariosRepositorio, IMapper mapper)
        {
            _horariosRepositorio = horariosRepositorio;
            _mapper = mapper;
        }

        public async Task<bool> RegistrarDisponibilidadAsync(RegistrarHorarioDto dto, CancellationToken cancellationToken)
        {
            if (dto == null || dto.Horarios == null || !dto.Horarios.Any())
            {
                return false;
            }
            var entidadesHorarios = _mapper.Map<List<HorariosDisponibles>>(dto.Horarios);
            entidadesHorarios.ForEach(h => h.Id_Medico = dto.IdMedico);
            bool resultado = await _horariosRepositorio.RegistrarHorariosAsync(entidadesHorarios, cancellationToken);
            return resultado;
        }

    }
}
