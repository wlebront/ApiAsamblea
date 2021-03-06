using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAsamblea.Models
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    public class Asambleista

    {
        [Key]
        public int Id { get; set; }
        public string Provincia { get; set; }
        public string Municipio { get; set; }
        public string Circ { get; set; }
        public string DistritoMunicipal { get; set; }
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string DescripcionOrgano { get; set; }
        public string Autoridad { get; set; }
        public string CargoElectivo { get; set; }
        public string TribunalDisc { get; set; }
        public string Sexo { get; set; }
        public string FechaFac { get; set; }
        public string NombreJce { get; set; }
        public string ApellidoJce { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string TelJce { get; set; }
        public string Celular { get; set; }
        public string Celular2 { get; set; }
        public string Direccion { get; set; }
        public short? Prov { get; set; }
        public short? Mun { get; set; }
        public short? CodDm { get; set; }
        public short? CodOrgano { get; set; }
        public byte[] Imagen { get; set; }
    }

}
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente

