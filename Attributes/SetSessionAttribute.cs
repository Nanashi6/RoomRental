using Microsoft.AspNetCore.Mvc.Filters;

namespace RoomRental.Attributes
{
    public class SetSessionAttribute : Attribute, IActionFilter
    {
        private string _name;//имя ключа
        private string[] _saveNames;//имя ключа
        public SetSessionAttribute(string name, params string[] saveNames)
        {
            _name = name;
            _saveNames = saveNames;
        }

        // Выполняется до выполнения метода контроллера, но после привязки данных передаваемых в контроллер
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        // Выполняется после выполнения метода контроллера
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            // считывание данных из ModelState и запись в сессию
            if (context.ModelState.Count > 0)
            {
                foreach (var item in context.ModelState)
                {
                    dict.Add(item.Key, item.Value.AttemptedValue);
                }
                Infrastructure.SessionExtensions.Set(context.HttpContext.Session, _name, dict);
            }
        }
    }
}
