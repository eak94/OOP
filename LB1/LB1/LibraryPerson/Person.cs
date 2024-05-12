using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibraryPerson
{
    /// <summary>
    /// Класс персон
    /// </summary>
    //TODO: XML+
    public class Person
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        private string _name;

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        private string _secondName;

        /// <summary>
        /// Возраст пользователя
        /// </summary>
        private int _age;

        /// <summary>
        /// Минимальный возраст
        /// </summary>
        public const int _minAge = 0;

        /// <summary>
        /// Максимальный возраст
        /// </summary>
        public const int _maxAge = 100;

        /// <summary>
        /// Свойства класса имя
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                bool flag = false;
                while (!flag)
                {
                    try
                    {
                        //TODO: RSDN
                        _name = ExceptionsName(value,
                            "Имя должно содержать только русские или английские буквы\n");
                        flag = true;
                    }
                    catch (ArgumentException exception)
                    {
                        Console.WriteLine($"{exception.Message} Введите имя заново:");
                        //TODO: remove+(удалила взаиможействие консоль)

                    }
                }
            }
        }

        /// <summary>
        /// Свойства класса Фамилия
        /// </summary>
        public string SecondName
        {
            get
            {
                return _secondName;
            }
            set
            {
                bool flag = false;
                while (!flag)
                {
                    try
                    {
                        //TODO: RSDN
                        _secondName = ExceptionsName(value, "Фамилия должна содержать только русские или английские буквы\n");
                        flag = true;
                    }
                    catch (ArgumentException exception)
                    {
                        Console.WriteLine($"{exception.Message}Введите фамилию заново:");
                        //TODO: remove+(удалила взаиможействие консоль)
                    }
                }
            }
        }
        /// <summary>
        /// Проверка символов, вводимых в поле Имя и Фамилия
        /// </summary>
        /// <param name="value">введенное Имя или Фамилия </param>
        /// <returns>Возвращает строку с Именем или Фамилией с заглавной буквы</returns>
        public static string ExceptionsName(string value, string errorMessage)
        {

            string nameSecondnamePattern = "^[а-яА-Яa-zA-Z]+-?[а-яА-Яa-zA-Z]*$";

            Regex regex = new Regex(nameSecondnamePattern);
            string validatedValue = string.Empty;

            foreach (Match match in regex.Matches(value))
            {
                validatedValue += match.Value;
            }

            if (string.IsNullOrEmpty(validatedValue))
            {
                throw new ArgumentException(errorMessage);
            }

            string[] words = validatedValue.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length >= 1)
                {
                    words[i] = words[i].Substring(0, 1).ToUpper() +
                               words[i].Substring(1).ToLower();
                }
            }

            validatedValue = string.Join(" ", words);
            return validatedValue;
        }

        /// <summary>
        /// Свойства класса Возраст
        /// </summary>
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                try
                {
                    _age = ExceptionsAge(value);
                }
                catch (ArgumentException exception)
                {
                    Console.WriteLine($"{exception.Message} Введите возраст заново:");
                    //TODO: remove+(удалила взаиможействие консоль)
                }
            }
        }
        /// <summary>
        /// Метод для обработки возвраста
        /// </summary>
        /// <param name="age">Возраст</param>
        /// <returns>Возвращает поле для возраста</returns>
        /// <exception cref="ArgumentException"></exception>
        public int ExceptionsAge(int age)
        {
            //TODO: remove +(удалила проверку дробного значения)
            if (age < _minAge)
            {
                throw new ArgumentException($"Возраст не может быть отрицательным\n");
            }

            if (age > _maxAge)
            {
                throw new ArgumentException($"Возраст не может быть больше {_maxAge}\n");
            }

            return age;
        }

        /// <summary>
        /// Свойства класса Пол
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Конструктор класса Person
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="secondName">Фамилия</param>
        /// <param name="age">Возраст</param>
        /// <param name="gender">Пол</param>
        public Person(string name, string secondName, int age, Gender gender)
        {
            Name = name;
            SecondName = secondName;
            Age = age;
            Gender = gender;
        }

        /// <summary>
        /// Возвращает строковое представление информации о человеке
        /// </summary>
        /// <returns>Строковое представление информации о человеке</returns>
        public string GetInfo()
        {
            return ($"Имя: {Name}, Фамилия: {SecondName}, Возраст: {Age}, Пол: {Gender}\n");
        }

        /// <summary>
        /// Экземляр класса по умолчанию
        /// </summary>
        public Person() : this("Иван", "Иванов", 50, Gender.Male)
        { }
    }
}
