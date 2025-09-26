using System;
using System.Collections.Generic;
using System.Linq;

namespace ProyrU
{
    public static class Numeros
    {
        public static bool EsPar(int Numero) => (Numero % 2) == 0;

        public static bool EsPrimo(int numero)
        {
            if (numero <= 1) return false;
            if (numero == 2) return true;
           
            if (EsPar(numero)) return false;
            
            List<int> posiblesDivisores = new List<int>();
            for (int i = 3; i * i <= numero; i += 2)
            {
                posiblesDivisores.Add(i);
            }
            
            return !posiblesDivisores.Any(divisor => numero % divisor == 0);
        }

        public static long Factorial(int numero)
        {
            if (numero < 0)
                throw new ArgumentException("factoriales no estan definidos para numeros negativos");
    
            long[] factorialesBase = { 1, 1 }; 
            
            if (numero <= 1)
                return factorialesBase[numero];
            
            long resultado = 1;
            
            int[] numerosParaMultiplicar = Enumerable.Range(2, numero - 1).ToArray();
            
            foreach (int num in numerosParaMultiplicar)
            {
                resultado *= num;
            }
            
            return resultado;
        }

        public static Dictionary<string, object> ClasificarNumero(int numero)
        {
            List<string> categorias = new List<string>();
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            
            bool esPar = EsPar(numero);
            categorias.Add(esPar ? "Par" : "Impar");
            
            bool esPrimo = EsPrimo(numero);
            if (esPrimo)
                categorias.Add("Primo");
            
            if (EsPerfecto(numero))
                categorias.Add("Perfecto");
            
            // Calcular factorial si es posible
            long factorial = 0;
            string factorialTexto = "";
            try
            {
                factorial = Factorial(numero);
                factorialTexto = factorial.ToString();
            }
            catch (Exception)
            {
                factorialTexto = "no calculable";
            }
         
            resultado["Numero"] = numero;
            resultado["EsPar"] = esPar;
            resultado["EsImpar"] = !esPar;
            resultado["EsPrimo"] = esPrimo;
            resultado["EsPerfecto"] = EsPerfecto(numero);
            resultado["Categorias"] = categorias;
            resultado["Factorial"] = factorialTexto;
            resultado["FactorialValor"] = factorial;
            
            return resultado;
        }
  
        private static bool EsPerfecto(int numero)
        {
            if (numero <= 1) return false;
            
            List<int> divisores = new List<int>();
            
            for (int i = 1; i < numero; i++)
            {
                if (numero % i == 0)
                    divisores.Add(i);
            }
          
            return divisores.Sum() == numero;
        }
        
        public static Dictionary<string, object> ObtenerEstadisticas(List<int> numeros)
        {
            var estadisticas = new Dictionary<string, object>();
            
            if (numeros == null || numeros.Count == 0)
            {
                estadisticas["Error"] = "no hay lista";
                return estadisticas;
            }
            
            List<int> pares = numeros.Where(n => EsPar(n)).ToList();
            List<int> impares = numeros.Where(n => !EsPar(n)).ToList();
            List<int> primos = numeros.Where(n => EsPrimo(n)).ToList();
            List<int> perfectos = numeros.Where(n => EsPerfecto(n)).ToList();
            
            estadisticas["TotalNumeros"] = numeros.Count;
            estadisticas["CantidadPares"] = pares.Count;
            estadisticas["CantidadImpares"] = impares.Count;
            estadisticas["CantidadPrimos"] = primos.Count;
            estadisticas["CantidadPerfectos"] = perfectos.Count;
            estadisticas["NumerosPares"] = pares;
            estadisticas["NumerosImpares"] = impares;
            estadisticas["NumerosPrimos"] = primos;
            estadisticas["NumerosPerfectos"] = perfectos;
            
            return estadisticas;
        }
    }

    class Program
    {
        private static List<int> numerosIngresados = new List<int>();
        
        static void Main(string[] args)
        {
            Console.WriteLine("numeros parte 2");
            
            bool continuar = true;
            
            while (continuar)
            {
                MostrarMenu();
                string opcion = Console.ReadLine() ?? "";
                
                switch (opcion.ToLower())
                {
                    case "1":
                        AnalizarNumeroIndividual();
                        break;
                    case "2":
                        AnalizarVariosNumeros();
                        break;
                    case "3":
                        MostrarEstadisticasGenerales();
                        break;
                    case "4":
                        LimpiarHistorial();
                        break;
                    case "5":
                    case "salir":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("no existe esa opcion\n");
                        break;
                }
            }
            
            Console.WriteLine("\nChao!");
            Console.WriteLine("presiona lo que sea para salir");
            Console.ReadKey();
        }
        
        static void MostrarMenu()
        {
            Console.WriteLine("elige una opcion:");
            Console.WriteLine("1. analizar numero");
            Console.WriteLine("2. analizar varios numeros");
            Console.WriteLine("3. estadisticas");
            Console.WriteLine("4. borrar lista");
            Console.WriteLine("5. salir");
            Console.WriteLine($"\nnumeros analizados hasta ahora: {numerosIngresados.Count}");
            Console.Write("\nselecciona una opción: ");
        }
        
        static void AnalizarNumeroIndividual()
        {
            Console.Write("\ningresa un numero: ");
            
            if (int.TryParse(Console.ReadLine(), out int numero))
            {
                // Agregar el número al historial
                numerosIngresados.Add(numero);
                
                // Clasificar el número
                var clasificacion = Numeros.ClasificarNumero(numero);
                
                // Mostrar resultados
                Console.WriteLine($"\nresultado del analisis {numero}:");
                Console.WriteLine("".PadRight(40, '='));
                
                Console.WriteLine($"numero: {clasificacion["Numero"]}");
                Console.WriteLine($"par o no: {((bool)clasificacion["EsPar"] ? "PAR" : "IMPAR")}");
                Console.WriteLine($"Primo: {((bool)clasificacion["EsPrimo"] ? "si" : "no")}");
                Console.WriteLine($"Perfecto: {((bool)clasificacion["EsPerfecto"] ? "si" : "no")}");
                
                // Mostrar categorías
                var categorias = (List<string>)clasificacion["Categorias"];
                Console.WriteLine($"Categorías: {string.Join(", ", categorias)}");
                
                // Mostrar factorial
                Console.WriteLine($"Factorial: {clasificacion["Factorial"]}");
                
                Console.WriteLine("".PadRight(40, '='));
            }
            else
            {
                Console.WriteLine("ingresa un numero valido.");
            }
            
            Console.WriteLine("\npresiona lo que sea para continuar...");
            Console.ReadKey();
            Console.Clear();
        }
        
        static void AnalizarVariosNumeros()
        {
            Console.WriteLine("\nanalisis de varios numeros");
            Console.WriteLine("separalos con espacios");
            
            string input = Console.ReadLine() ?? "";
            string[] numerosTexto = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            List<int> numerosActuales = new List<int>();
            
            foreach (string numeroTexto in numerosTexto)
            {
                if (int.TryParse(numeroTexto, out int numero))
                {
                    numerosActuales.Add(numero);
                    numerosIngresados.Add(numero);
                }
            }
            
            if (numerosActuales.Count > 0)
            {
                Console.WriteLine($"\nresultado de {numerosActuales.Count} numeros:");
                Console.WriteLine("".PadRight(50, '='));
                
                foreach (int numero in numerosActuales)
                {
                    var clasificacion = Numeros.ClasificarNumero(numero);
                    var categorias = (List<string>)clasificacion["Categorias"];
                    
                    Console.WriteLine($"🔢 {numero,3} → {string.Join(", ", categorias)}");
                }
                
                // Mostrar estadísticas de este grupo
                var stats = Numeros.ObtenerEstadisticas(numerosActuales);
                Console.WriteLine("\nanalisis de la lista:");
                Console.WriteLine($"pares: {stats["CantidadPares"]}");
                Console.WriteLine($"impares: {stats["CantidadImpares"]}");
                Console.WriteLine($"primos: {stats["CantidadPrimos"]}");
                Console.WriteLine($"perfectos: {stats["CantidadPerfectos"]}");
                
                Console.WriteLine("".PadRight(50, '='));
            }
            else
            {
                Console.WriteLine("error, busca que hiciste mal.");
            }
            
            Console.WriteLine("\npresiona lo que sea para continuar...");
            Console.ReadKey();
            Console.Clear();
        }
        
        static void MostrarEstadisticasGenerales()
        {
            Console.WriteLine("\nestadistica de la lista");
            Console.WriteLine("".PadRight(40, '='));
            
            if (numerosIngresados.Count == 0)
            {
                Console.WriteLine("No hay números en el historial.");
            }
            else
            {
                var estadisticas = Numeros.ObtenerEstadisticas(numerosIngresados);
                
                Console.WriteLine($"total de numeros: {estadisticas["TotalNumeros"]}");
                Console.WriteLine($"numeros pares: {estadisticas["CantidadPares"]}");
                Console.WriteLine($"numeros impares: {estadisticas["CantidadImpares"]}");
                Console.WriteLine($"numeros primos: {estadisticas["CantidadPrimos"]}");
                Console.WriteLine($"numeros perfectos: {estadisticas["CantidadPerfectos"]}");
                
                var pares = (List<int>)estadisticas["NumerosPares"];
                var primos = (List<int>)estadisticas["NumerosPrimos"];
                var perfectos = (List<int>)estadisticas["NumerosPerfectos"];
                
                if (pares.Count > 0)
                    Console.WriteLine($"\npares encontrados: [{string.Join(", ", pares.OrderBy(x => x))}]");
                
                if (primos.Count > 0)
                    Console.WriteLine($"primos encontrados: [{string.Join(", ", primos.OrderBy(x => x))}]");
                
                if (perfectos.Count > 0)
                    Console.WriteLine($"perfectos encontrados: [{string.Join(", ", perfectos.OrderBy(x => x))}]");
            }
            
            Console.WriteLine("".PadRight(40, '='));
            Console.WriteLine("\npresiona lo que sea para continuar...");
            Console.ReadKey();
            Console.Clear();
        }
        
        static void LimpiarHistorial()
        {
            Console.Write("\nborrar lista (s/n): ");
            string respuesta = Console.ReadLine()?.ToLower() ?? "";
            
            if (respuesta == "s" || respuesta == "si" || respuesta == "sí")
            {
                numerosIngresados.Clear();
                Console.WriteLine("ok");
            }
            else
            {
                Console.WriteLine("o");
            }
            
            Console.WriteLine("\npresiona lo que sea para continuar...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}