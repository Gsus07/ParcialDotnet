using System.ComponentModel;
using System.IO.Pipes;
using System;
using System.Security.Permissions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Entidad;
using MySql.Data.MySqlClient;

namespace Datos
{
    public class PersonaRepository
    {
        private readonly MySqlConnection _connection;
        private readonly List<Persona> _personas = new List<Persona>();
        public PersonaRepository(ConnectionManager connection)
        {
            _connection = connection._connection;
        }
        public void Guardar(Persona persona)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = @"Insert Into Persona (identificacion,nombre,edad,sexo,departamento,ciudad,valorApoyo,modalidadApoyo,fecha) 
                                        values (@Identificacion,@Nombre,@Edad,@Sexo,@Departamento,@Ciudad,@ValorApoyo,@ModalidadApoyo,@Fecha)";
                command.Parameters.AddWithValue("@Identificacion", persona.Identificacion);
                command.Parameters.AddWithValue("@Nombre", persona.Nombre);
                command.Parameters.AddWithValue("@Sexo", persona.Sexo);
                command.Parameters.AddWithValue("@Edad", persona.Edad);
                command.Parameters.AddWithValue("@Departamento", persona.Departamento);
                command.Parameters.AddWithValue("@Ciudad",persona.Ciudad);
                command.Parameters.AddWithValue("@ValorApoyo",persona.ValorApoyo);
                command.Parameters.AddWithValue("@ModalidadApoyo",persona.ModalidadApoyo);
                command.Parameters.AddWithValue("@Fecha",persona.Fecha);
                var filas = command.ExecuteNonQuery();
            }
        }
        public void Eliminar(Persona persona)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "Delete from persona where identificacion=@Identificacion";
                command.Parameters.AddWithValue("@Identificacion", persona.Identificacion);
                command.ExecuteNonQuery();
            }
        }
        public List<Persona> ConsultarTodos()
        {
            MySqlDataReader dataReader;
            List<Persona> personas = new List<Persona>();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "Select * from persona ";
                dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        Persona persona = DataReaderMapToPerson(dataReader);
                        personas.Add(persona);
                    }
                }
            }
            return personas;
        }
        public Persona BuscarPorIdentificacion(string identificacion)
        {
            MySqlDataReader dataReader;
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "select * from persona where identificacion=@Identificacion";
                command.Parameters.AddWithValue("@Identificacion", identificacion);
                dataReader = command.ExecuteReader();
                dataReader.Read();
                return DataReaderMapToPerson(dataReader);
            }
        }
        private Persona DataReaderMapToPerson(MySqlDataReader dataReader)
        {
            if(!dataReader.HasRows) return null;
            Persona persona = new Persona();
            persona.Identificacion = (string)dataReader["identificacion"];
            persona.Nombre = (string)dataReader["nombre"];
            persona.Sexo = (string)dataReader["sexo"];
            persona.Edad = (int)dataReader["edad"];
            persona.Departamento = (string)dataReader["departamento"];
            persona.Ciudad = (string)dataReader["ciudad"];
            persona.ValorApoyo = (decimal)dataReader["valorApoyo"];
            persona.ModalidadApoyo = (string)dataReader["modalidadApoyo"];
            persona.Fecha = (DateTime)dataReader["fecha"];
            return persona;
        }
        public int Totalizar()
        {
            return _personas.Count();
        }
        public int TotalizarMujeres()
        {
            ConsultarTodos();
            return _personas.Where(p => p.Sexo.Equals("F")).Count();
        }
        public int TotalizarHombres()
        {
            ConsultarTodos();
            return _personas.Where(p => p.Sexo.Equals("M")).Count();
        }
    }
}