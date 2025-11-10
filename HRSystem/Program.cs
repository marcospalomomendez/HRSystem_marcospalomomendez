using System;
using System.Collections.Generic;

// Ejercicio 1: Abstracción, Encapsulación y Validación
abstract class Empleado
{
    public string Nombre { get; set; }

    private double _salarioBase;

    // Encapsulación y validación: no valores negativos (sin ternarias)
    public double SalarioBase
    {
        get { return _salarioBase; }
        set
        {
            if (value >= 0)
            {
                _salarioBase = value;
            }
            else
            {
                _salarioBase = 0.0;
            }
        }
    }

    // Constructor base
    public Empleado(string nombre, double salarioBase)
    {
        Nombre = nombre;
        SalarioBase = salarioBase;
    }

    // ✅ Métodos polimórficos
    public abstract double CalcularNomina();

    public override string ToString()
    {
        return $"Empleado: {Nombre} | Salario base: {SalarioBase:C2}";
    }
}

// Ejercicio 2: Herencia y Primera Implementación (EmpleadoFijo)
class EmpleadoFijo : Empleado
{
    private double _bonoAnual;

    public double BonoAnual
    {
        get { return _bonoAnual; }
        set
        {
            if (value >= 0)
            {
                _bonoAnual = value;
            }
            else
            {
                _bonoAnual = 0.0;
            }
        }
    }

    public EmpleadoFijo(string nombre, double salarioBase, double bonoAnual)
        : base(nombre, salarioBase)
    {
        BonoAnual = bonoAnual;
    }

    //Polimorfismo
    public override double CalcularNomina()
    {
        return SalarioBase + (BonoAnual / 12);
    }

    public override string ToString()
    {
        return $"[Empleado Fijo] {base.ToString()} | Bono anual: {BonoAnual:C2} | Nómina mensual: {CalcularNomina():C2}";
    }
}

// Ejercicio 3: Atributos Específicos y Polimorfismo Final (EmpleadoPorHora)
class EmpleadoPorHora : Empleado
{
    private double _tarifaHora;
    private double _horasTrabajadas;

    public double TarifaHora
    {
        get { return _tarifaHora; }
        set
        {
            if (value >= 0)
            {
                _tarifaHora = value;
            }
            else
            {
                _tarifaHora = 0.0;
            }
        }
    }

    public double HorasTrabajadasMes
    {
        get { return _horasTrabajadas; }
        set
        {
            if (value >= 0)
            {
                _horasTrabajadas = value;
            }
            else
            {
                _horasTrabajadas = 0.0;
            }
        }
    }

    public EmpleadoPorHora(string nombre, double salarioBase, double tarifaHora, double horasTrabajadas)
        : base(nombre, salarioBase)
    {
        TarifaHora = tarifaHora;
        HorasTrabajadasMes = horasTrabajadas;
    }

    public override double CalcularNomina()
    {
        return SalarioBase + (TarifaHora * HorasTrabajadasMes);
    }

    public override string ToString()
    {
        return $"[Empleado por Hora] {base.ToString()} | Tarifa: {TarifaHora:C2}/h | Horas: {HorasTrabajadasMes:F2} | Nómina mensual: {CalcularNomina():C2}";
    }
}

// Ejercicio 4: Integración del Sistema de Consola (Polimorfismo con Colecciones)
class Program
{
    static List<Empleado> empleados = new List<Empleado>();

    static void Main()
    {
        int opcion;
        do
        {
            MostrarMenu();
            opcion = LeerOpcion();

            switch (opcion)
            {
                case 1:
                    ContratarEmpleado();
                    break;
                case 2:
                    VerNominas();
                    break;
                case 3:
                    CalcularCosteTotal();
                    break;
                case 4:
                    Console.WriteLine("Saliendo del sistema...");
                    break;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }

        } while (opcion != 4);
    }

    static void MostrarMenu()
    {
        Console.WriteLine("\n========== HRSystem - Gestión de Personal ==========");
        Console.WriteLine("1. Contratar empleado");
        Console.WriteLine("2. Ver nóminas individuales");
        Console.WriteLine("3. Calcular coste total de nóminas");
        Console.WriteLine("4. Salir");
    }

    static int LeerOpcion()
    {
        while (true)
        {
            Console.Write("Seleccione una opción (1-4): ");
            int numero;
            bool esNumero = int.TryParse(Console.ReadLine(), out numero);
            if (esNumero && numero >= 1 && numero <= 4)
            {
                return numero;
            }
            Console.WriteLine("Entrada inválida. Intente nuevamente.");
        }
    }

    static void ContratarEmpleado()
    {
        Console.WriteLine("\nSeleccione el tipo de empleado:");
        Console.WriteLine("1. Empleado Fijo");
        Console.WriteLine("2. Empleado por Hora");

        int tipo = 0;
        while (true)
        {
            Console.Write("Opción (1-2): ");
            bool esNumero = int.TryParse(Console.ReadLine(), out tipo);
            if (esNumero && tipo >= 1 && tipo <= 2)
            {
                break;
            }
            Console.WriteLine("Entrada inválida. Intente nuevamente.");
        }

        Console.Write("Nombre del empleado: ");
        string nombre = Console.ReadLine();

        double salarioBase = LeerDoublePositivo("Salario base mensual: ");

        if (tipo == 1)
        {
            double bono = LeerDoublePositivo("Bono anual: ");
            empleados.Add(new EmpleadoFijo(nombre, salarioBase, bono));
            Console.WriteLine("Empleado fijo contratado correctamente.");
        }
        else if (tipo == 2)
        {
            double tarifa = LeerDoublePositivo("Tarifa por hora: ");
            double horas = LeerDoublePositivo("Horas trabajadas este mes: ");
            empleados.Add(new EmpleadoPorHora(nombre, salarioBase, tarifa, horas));
            Console.WriteLine("Empleado por hora contratado correctamente.");
        }
    }

    static double LeerDoublePositivo(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            double valor;
            bool esNumero = double.TryParse(Console.ReadLine(), out valor);
            if (esNumero && valor >= 0)
            {
                return valor;
            }
            Console.WriteLine("Valor inválido. Debe ser un número no negativo.");
        }
    }

    static void VerNominas()
    {
        Console.WriteLine("\n--- Nóminas Individuales ---");
        if (empleados.Count == 0)
        {
            Console.WriteLine("No hay empleados contratados.");
            return;
        }

        int i = 1;
        foreach (Empleado e in empleados)
        {
            Console.WriteLine($"{i}. {e}");
            i++;
        }
    }

    static void CalcularCosteTotal()
    {
        double total = 0;
        foreach (Empleado e in empleados)
        {
            total += e.CalcularNomina();
        }

        Console.WriteLine($"\nCoste total mensual de nóminas: {total:C2}");
    }
}
