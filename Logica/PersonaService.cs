using System;
using Datos;
using Entidad;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Logica
{
    public class PersonaService
    {
        private readonly ConnectionManager conexion;
        private readonly PersonaRepository repositorio;
        public PersonaService(string connectionString)
        {
            conexion = new ConnectionManager(connectionString);
            repositorio = new PersonaRepository(conexion);
        }
        public bool Guardar(Persona persona)
        {
            try
            {          
                conexion.Open();
                repositorio.Guardar(persona);
                conexion.Close();
                return true;
            }
            catch (Exception e)
            { return false; }
            finally { conexion.Close(); }
        }
    
        
        
        

    }
}
