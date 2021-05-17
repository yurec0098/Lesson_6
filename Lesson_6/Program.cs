using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Lesson_6
{
	class Program
	{
		/*
		 * Написать консольное приложение Task Manager, которое выводит на экран запущенные процессы и позволяет завершить указанный процесс.
		 * Предусмотреть возможность завершения процессов с помощью указания его ID или имени процесса. 
		 * В качестве примера можно использовать консольные утилиты Windows tasklist и taskkill.
		 */
		static void Main(string[] args)
		{
			Console.WriteLine("\tTask Manager");

			foreach (var p in Process.GetProcesses(Environment.MachineName))
				Console.WriteLine($"[{p.Id}] {p.ProcessName}");

			Console.WriteLine("Для завершения какого либо процесса введите его имя или ID:");
			string line = Console.ReadLine();

			if (int.TryParse(line, out int id))
			{
				KillProcessByID(id);
			}
			else
			{
				var processes = Process.GetProcessesByName(line);
				if (processes.Length > 0)
				{
					Console.WriteLine($"Процессы с именем='{line}':");
					foreach (var p in processes)
						Console.WriteLine($"[{p.Id}] {p.ProcessName}");

					Console.WriteLine($"Чтобы закрыть все введите 'All'(Все) или введите ID нужного");
					line = Console.ReadLine();
					if ("All".Equals(line, StringComparison.OrdinalIgnoreCase) || "Все".Equals(line, StringComparison.OrdinalIgnoreCase))
					{
						int counter = 0;
						foreach (var pr in processes)
						{
							pr.Kill(true);
							if (pr.HasExited)
								counter++;
						}

						Console.WriteLine($"Было закрыто {counter} процессов из {processes.Length}");
					}
					else if (int.TryParse(line, out id))
						KillProcessByID(id);
					else
						throw new ArgumentException();
				}
				else
					Console.WriteLine($"Процессы с именем='{line}' не найдены");
			}

			Console.ReadLine();
		}

		private static void KillProcessByID(int id)
		{
			if (Process.GetProcesses(Environment.MachineName).Any(p => p.Id == id))
			{
				var process = Process.GetProcessById(id);
				process.Kill(true);
				Console.WriteLine($"Процесс '{process.ProcessName}' {(process.HasExited ? "" : "не ")}было закрыто");
			}
			else
				Console.WriteLine($"Процесс с ID='{id}' не найден");
		}
	}
}
