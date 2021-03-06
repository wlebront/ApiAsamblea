using ApiAsamblea.Data;
using ApiAsamblea.Models;
using ApiAsamblea.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAsamblea.Repository
{
    public class AsambleistaRepository : IAsambleistaRepository
    {
        private readonly ApplicationDbContext _bd;

        public AsambleistaRepository(ApplicationDbContext bd)
        {
           _bd = bd;
        }

        public bool ActualizarAsambleista(Asambleista asambleista)
        {
            _bd.Asambleista.Update(asambleista);
            return Guardar();
        }

        public bool BorrarAsambleista(Asambleista asambleista)
        {
            _bd.Asambleista.Remove(asambleista);
            return Guardar();
        }

        public bool CrearAsambleista(Asambleista asambleista)
        {
            _bd.Asambleista.Add(asambleista);
            return Guardar();
        }

        public bool ExisteAsambleista(string nombre)
        {
            bool valor = _bd.Asambleista.Any(a => a.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;

        }

        public bool ExisteAsambleista(int id)
        {
            return _bd.Asambleista.Any(a => a.Id == id);
        }

        public Asambleista GetAsambleista(int AsambleistaId)
        {
            return _bd.Asambleista.FirstOrDefault(a => a.Id == AsambleistaId);
        }

        public ICollection<Asambleista> GetAsambleistas()
        {
            return _bd.Asambleista.OrderBy(a => a.Id).ToList();
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }
    }
}
