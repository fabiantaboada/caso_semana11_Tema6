using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace caso_semana11_Tema6
{
    internal class Program
    {
        static int max_estudiantes = 40;
        static int max_cursos = 4;
        static string[] nombreCursos = { "Matemáticas", "Programación", "Física", "Base de datos" };
        static string[] estudiante = new string[max_estudiantes];
        static double[,] matriznotas = new double[max_estudiantes, max_cursos];
        static int total_estudiantes = 0;
        static string rutaArchivo = "notas_curso.txt";
        public static void RegistrarEstudiante()
        {
            Console.WriteLine("REGISTRO DE ESTUDIANTE");
            if (total_estudiantes >= max_estudiantes)
            {
                Console.WriteLine("Se alcanzó la capacidad máxima");
                return;
            }
            string nombre = Validador.LeerTexto("Ingrese nombres y apellidos:").ToUpper();
            estudiante[total_estudiantes] = nombre;
            Console.WriteLine("Ingrese las notas de 0 a 20: ");
            for (int i = 0; i < max_cursos; i++)
            {
                matriznotas[total_estudiantes, i] = Validador.LeerDecimales($"Ingresar nota de {nombreCursos[i]}:");
            }
            total_estudiantes++;
            Console.WriteLine("Estudiante registrado..!");
        }
        public static void guardarArchivo()
        {
            Console.WriteLine("Guardar registros en archivo");
            if (total_estudiantes == 0)
            {
                Console.WriteLine("Sin Registros a guardar");
                return;
            }
            using (StreamWriter escritor = new StreamWriter(rutaArchivo))
            {
                for (int i = 0; i < total_estudiantes; i++)
                {
                    string linea = estudiante[i];
                    for (int j = 0; j < max_cursos; j++)
                    {
                        linea += ","+matriznotas[i, j];
                        //linea += $",{matriznotas[i, j]}";
                    }
                    escritor.WriteLine(linea);
                }
            }
            Console.WriteLine("Registros guardados..!!");
        }
    public static void CargarRegistros()
        {
            Console.WriteLine("CARGAR REGISTROS DESDE ARCHIVO");
            if (!File.Exists(rutaArchivo))
            {
                Console.WriteLine("No existe el archivo");
                return;
            }
            total_estudiantes = 0;
            using (StreamReader lector = new StreamReader(rutaArchivo))
            {
                string linea;
                while ((linea = lector.ReadLine()) != null && total_estudiantes < max_estudiantes)
                {
                    string[] partes = linea.Split(',');
                    if (partes.Length == max_cursos + 1)
                    {
                        estudiante[total_estudiantes] = partes[0].ToUpper();
                        for (int i = 0; i < max_cursos; i++)
                        {
                            matriznotas[total_estudiantes, i] = Convert.ToDouble(partes[i + 1]);
                        }
                        total_estudiantes++;
                    }
                    
                }
            }
            Console.WriteLine("Se cargaron los registros del archivo..!!");
        }
        public static void mostrarRegistros()
        {
            Console.WriteLine(new string ('=',100));
            Console.WriteLine("\t\tREGISTRO GENERAL DE NOTAS POR CURSO");
            Console.WriteLine(new string('=', 100));
            if (total_estudiantes == 0)
            {
                Console.WriteLine("No existe Registros..!!");
                return;
            }
            Console.Write($"{"ESTUDIANTE",-25}|");
            for (int i = 0; i < max_cursos; i++)
            {
                Console.Write($"{nombreCursos[i],-12}|");
            }
            Console.WriteLine("PROMEDIO");
            Console.WriteLine(new string('-',100));
            for(int i=0;i<total_estudiantes; i++)
            {
                Console.Write($"{estudiante[i],-25}|");
                double suma = 0;
                for (int j = 0; j < max_cursos; j++)
                {
                    Console.Write($"{matriznotas[i, j],-12:F1}|");
                    suma += matriznotas[i, j];
                }
                double promedio = suma / max_cursos;
                string estado = promedio >= 11 ? "Aprobado" : "Desaprobado";
                Console.WriteLine($"{promedio:F2}({estado})");
            }
        }
        static void Main(string[] args)
        {
            int opc=0;
            while (opc != 5)
            {
                Console.Clear();
                Console.WriteLine("[1]REGISTRAR ESTUDIANTE");
                Console.WriteLine("[2]GUARDAR EN ARCHIVO");
                Console.WriteLine("[3]CARGAR RESGISTROS DEL ARCHIVO");
                Console.WriteLine("[4]MOSTRAR REGISTROS");
                Console.WriteLine("[5]SALIR");
                opc = Validador.LeerEntero("Seleccionar opción[1-5] :");
                switch (opc)
                {
                    case 1:
                        RegistrarEstudiante();
                        break;
                    case 2:
                        guardarArchivo();
                        break;
                    case 3:
                        CargarRegistros();
                        break;
                    case 4:
                        mostrarRegistros();
                        break;
                    case 5:
                        Console.WriteLine("Muchos éxitos para el nuevo semestre");
                        break;
                }
                Console.ReadKey();
            }
        }
    }
}
