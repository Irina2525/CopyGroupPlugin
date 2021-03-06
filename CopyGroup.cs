using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyGroupPlugin
{
    [TransactionAttribute(TransactionMode.Manual)] // Manual - ручной режим
    public class CopyGroup : IExternalCommand

    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument; //  - дполучили доступ к документу 
            Document doc = uiDoc.Document; // рлдучаем ссылку на экз-р классса Document, кт будкт содержать базу данных эл-тов внутри окрытого эл-та

            //попросить пользователя выбрать группу для копирования
            Reference reference = uiDoc.Selection.PickObject(ObjectType.Element, "Выберите группу объектов"); // получили ссылку на выбранную пользователем группу объектов
            Element element = doc.GetElement(reference);
            Group group = element as Group; // тот объект по кт пользователь щелкнул получили и преобразовали к типу Group

            //попросим пользователя выбрать какую то точку 
            XYZ point = uiDoc.Selection.PickPoint("Выберите точку");

            Transaction transaction = new Transaction(doc);
            transaction.Start("Копирование группы объектов");
            doc.Create.PlaceGroup(point, group.GroupType);
            transaction.Commit();

            return Result.Succeeded;


        }
    }
}
