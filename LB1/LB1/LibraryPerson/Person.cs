using System.Text.RegularExpressions;
namespace LibraryPerson
{
    /// <summary>
    /// Класс персон
    /// </summary>
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
        public const int MinAge = 0;

        /// <summary>
        /// Максимальный возраст
        /// </summary>
        public const int MaxAge = 100;

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
                _name = ConvertRegistr
                    (CheckNullorEmptyName(value, nameof(Name)));

                if (_secondName != null)
                {
                    CheckLanguage(_name, _secondName);
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
                _secondName = ConvertRegistr
                    (CheckNullorEmptyName(value, nameof(SecondName)));

                if (_name != null)
                {
                    CheckLanguage(_name, _secondName);
                }
            }
        }

        /// <summary>
        /// Метод для проверки не заполненности поля Имени 
        /// </summary>
        /// <param name="value"> Имя </param>
        /// <param name="propertiname"> Cвойство имени </param>
        /// <returns> Возвращается имя и фамилия </returns>
        public string CheckNullorEmptyName(string value, string propertiname)
        {
            if (value == null || string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"Поле {propertiname} не может быть пустым." +
                    $" Введите, пожалуйста, еще раз");
            }

            return value;
        }

        /// <summary>
        /// Метод для преобразования верхнего регистра Имени или Фамилии персоны
        /// </summary>
        /// <param name="nameOrSecondName"> Имя или Фамилия персоны </param>
        /// <returns> Возвращается преобразованное в верхний регистр Имя или Фамилия персоны </returns>
        public string ConvertRegistr(string nameOrSecondName)
        {
            nameOrSecondName = nameOrSecondName[0].ToString().ToUpper()
                        + nameOrSecondName.Substring(1);

            Regex regexNameOrSecondName = new Regex(@"[-]");

            if (regexNameOrSecondName.IsMatch(nameOrSecondName))
            {
                string[] words = nameOrSecondName.Split(new char[] { '-' });
                string firstWord = words[0];
                string secondWord = words[1];
                firstWord = firstWord[0].ToString().ToUpper() + firstWord.Substring(1);
                secondWord = secondWord[0].ToString().ToUpper() + secondWord.Substring(1);
                nameOrSecondName = firstWord + '-' + secondWord;
            }

            return nameOrSecondName;
        }

        /// <summary>
        /// Проверка на ввод имени или фамилии на одном языке с возможность ввода двойного имени и фамилии
        /// </summary>
        /// <param name="nameOrSecondName"> Введенное Имя или Фамилия </param>
        /// <returns> Возвращается Имя или Фамилия персоны </returns>
        public string CheckNameSurname(string nameOrSecondName)
        {
            string regexnameOrSecondName = @"(^[А-яё]+(-[А-яё])?[А-яё]*$)" +
                "|(^[A-Za-z]+(-[A-Za-z])?[A-Za-z]*$)";

            Regex nameLanguage = new Regex(regexnameOrSecondName);

            if (!nameLanguage.IsMatch(nameOrSecondName))
            {
                throw new FormatException("Введёное слово не распознано." +
                    "Введите, пожалуйста, еще раз");
            }

            return nameOrSecondName;
        }

        /// <summary>
        /// Метод для сравнения языка имени и фамилии
        /// </summary>
        /// <param name="name"> Имя </param>
        /// <param name="secondName"> Фамилия </param>
        public void CheckLanguage(string name, string secondName)
        {
            Language nameLanguage = DefineLanguage(name);
            Language secondNameLanguage = DefineLanguage(secondName);

            if (nameLanguage != secondNameLanguage)
            {
                throw new ArgumentException("Язык Имени и Фамилии" +
                    " должен совпадать. Введите, пожалуйста, еще раз.");
            }
        }

        /// <summary>
        /// Метод для проверки языка 
        /// </summary>
        /// <param name="word"> Строка </param>
        /// /// <returns> Возвращается язык </returns>
        private Language DefineLanguage(string word)
        {
            Regex englishLanguage = new Regex(@"[a-zA-Z]");
            Regex russianLanguage = new Regex(@"[а-яА-Я]");

            if (englishLanguage.IsMatch(word))
            {
                return Language.English;
            }

            else if (russianLanguage.IsMatch(word))
            {
                return Language.Russian;
            }

            else
            {
                throw new ArgumentException("Разрешено вводить только символы " +
                    "русского или английского алфавита.\n" +
                    "Введите, пожалуйста, еще раз");
            }
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
                _age = CheckAge(value);
            }
        }

        //TODO: rename+
        /// <summary>
        /// Метод для обработки возвраста
        /// </summary>
        /// <param name="age"> Возраст </param>
        /// <returns> Возвращает поле для возраста </returns>
        public int CheckAge(int age)
        {
            if (age < MinAge)
            {
                throw new ArgumentException($"Возраст не может быть отрицательным\n");
            }

            if (age > MaxAge)
            {
                throw new ArgumentException($"Возраст не может быть больше {MaxAge}\n");
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
        /// <returns> Строковое представление информации о человеке </returns>
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
