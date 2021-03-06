using ApiAsamblea.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAsamblea.Repository.IRepository
{
   public interface IAsambleistaRepository
    {
        ICollection<Asambleista> GetAsambleistas();
        Asambleista GetAsambleista(int AsambleistaId);
        bool ExisteAsambleista(string nombre);
        bool ExisteAsambleista(int id);
        bool CrearAsambleista(Asambleista asambleista);
        bool ActualizarAsambleista(Asambleista asambleista);
        bool BorrarAsambleista(Asambleista asambleista);
        bool Guardar();

    }
}
