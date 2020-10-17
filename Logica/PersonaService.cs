using System;
using Datos;
using Entidad;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

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
        public ServiceResponse Save(Persona persona)
        {
            try
            {          
                conexion.Open();
                repositorio.Save(persona);
                conexion.Close();
                return new ServiceResponse(persona);
            }
            catch (Exception e)
            {return new ServiceResponse($"Error: {e.Message}");}
            finally { conexion.Close(); }
        }
    
        public ConsultaPersonaResponse GetList()
        {
            try
            {
                conexion.Open();
                IList<Persona>personas = repositorio.GetList();
                conexion.Close();
                
                return new ConsultPersonaResponse(personas);
            }
            catch (Exception e)
            {
                return new ConsultaPersonaResponse($"Error: {e.Message}");
            }
            finally { conexion.Close(); }
        }
        public void Delete(Persona Persona)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "Delete from Persona where Identificacion=@Identificacion";
                command.Parameters.AddWithValue("@Identificacion", Persona.Identificacion);
                command.ExecuteNonQuery();
            }
        }
        public List<Persona> GetList()
        {
            SqlDataReader dataReader;
            List<Persona> Personas = new List<Persona>();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "Select * from Person ";
                dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        Persona persona = DataReaderMapearPersona(dataReader);
                        Personas.Add(persona);
                    }
                }
            }
            return Personas;
        }
        private Persona DataReaderMapearPersona(SqlDataReader dataReader)
        {
            if (!dataReader.HasRows) return null;
            Persona persona = new Persona();
            persona.Identificacion = (string)dataReader["Identificacion"];
            persona.Nombre = (string)dataReader["Nombre"];
            persona.Sexo = (string)dataReader["Sexo"];
            persona.Edad = (int)dataReader["Edad"];

            persona.ValorApoyo = (decimal)dataReader["Pulsation"];
            return persona;
        }
        public int Totalizar()
        {
            return GetList().Count();
        }

    }
}
