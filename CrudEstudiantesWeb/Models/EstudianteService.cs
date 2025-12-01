namespace CrudEstudiantesWeb.Models
{
    public class EstudianteService
    {
        private static List<Estudiante> _estudiantes = new();
        private static int _nextId = 1;

        public List<Estudiante> GetAll()
        {
            return _estudiantes;
        }

        public Estudiante? GetById(int id)
        {
            return _estudiantes.FirstOrDefault(e => e.Id == id);
        }

        // 👇 NUEVO: verificar duplicados

        public bool MatriculaExiste(string matricula, int? idExcluir = null)
        {
            return _estudiantes.Any(e =>
                e.Matricula == matricula &&
                (!idExcluir.HasValue || e.Id != idExcluir.Value));
        }

        public bool CorreoExiste(string correo, int? idExcluir = null)
        {
            return _estudiantes.Any(e =>
                e.Correo == correo &&
                (!idExcluir.HasValue || e.Id != idExcluir.Value));
        }

        public void Create(Estudiante estudiante)
        {
            estudiante.Id = _nextId++;
            _estudiantes.Add(estudiante);
        }

        public void Update(Estudiante estudiante)
        {
            var existing = GetById(estudiante.Id);
            if (existing != null)
            {
                existing.Matricula = estudiante.Matricula;
                existing.Nombre = estudiante.Nombre;
                existing.Carrera = estudiante.Carrera;
                existing.Correo = estudiante.Correo;
            }
        }

        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing != null)
            {
                _estudiantes.Remove(existing);
            }
        }
    }
}
