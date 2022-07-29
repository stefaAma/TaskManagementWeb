using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.IServices;
using TaskManagement.Models;
using TaskManagement.ViewModels.TaskCategories;

namespace TaskManagement.Services
{
    public class TaskCategoriesService : ITaskCategoriesService
    {
        private TaskManagementContext TaskManagementContext { get; set; }

        public TaskCategoriesService(TaskManagementContext taskManagementContext)
        {
            TaskManagementContext = taskManagementContext;
        }

        public async Task<IEnumerable<TaskCategory>> GetCategories()
        {
            return await TaskManagementContext
                .TaskCategories
                .OrderBy(tc => tc.Name) 
                .ToListAsync();
        }

        public async Task<bool> CreateTaskCategory(TaskCategory category)
        {
            if (!categoryCheck(category))
                return false;
            category.RgbaColor = HexToRgba(category.RgbaColor);
            TaskManagementContext.TaskCategories.Add(category);
            int changes = await TaskManagementContext.SaveChangesAsync();
            if (changes != 1)
                return false;
            return true;
        }

        private bool categoryCheck(TaskCategory category)
        {
            if (category == null || string.IsNullOrEmpty(category.Name)
                || (category.Effort != 1 && category.Effort != 1.25 && category.Effort != 1.5
                && category.Effort != 1.75 && category.Effort != 2))
                return false;
            return true;
        }

        public async Task<TaskCategory> GetTaskCategoryById(Guid Id)
        {
            var category = await TaskManagementContext.TaskCategories.FindAsync(Id);
            if (category == null)
                throw new KeyNotFoundException("Category cannot be found!");
            category.RgbaColor = RgbaToHex(category.RgbaColor);
            return category;
        }

        private string RgbaToHex(string rgba)
        {
            string r = "";
            string g = "";
            string b = "";
            bool isRCompleted = false;
            bool isGCompleted = false;
            bool isBCompleted = false;
            int position = 0;
            
            foreach (var c in rgba)
            {
                if (Char.IsDigit(c) && !isRCompleted)
                {
                    r += c;
                    if (position + 1 < rgba.Length && !Char.IsDigit(rgba[position + 1]))
                        isRCompleted = true;
                }
                else if (Char.IsDigit(c) && !isGCompleted)
                {
                    g += c;
                    if (position + 1 < rgba.Length && !Char.IsDigit(rgba[position + 1]))
                        isGCompleted = true;
                }
                else if (Char.IsDigit(c) && !isBCompleted)
                {
                    b += c;
                    if (position + 1 < rgba.Length && !Char.IsDigit(rgba[position + 1]))
                        isBCompleted = true;
                }
                position++;
            }

            int rInt = Convert.ToInt32(r, 10);
            int gInt = Convert.ToInt32(g, 10);
            int bInt = Convert.ToInt32(b, 10);

            string rHex = rInt.ToString("X");
            string gHex = gInt.ToString("X");
            string bHex = bInt.ToString("X");

            if (rHex.Length == 1)
                rHex = "0" + rHex;
            if (gHex.Length == 1)
                gHex = "0" + gHex;
            if(bHex.Length == 1)
                bHex = "0" + bHex;

            return "#" + rHex + gHex + bHex;
        }

        private string HexToRgba(string hex)
        {
            string r = hex.Substring(1, 2);
            string g = hex.Substring(3, 2);
            string b = hex.Substring(5, 2);
            int rInt = Convert.ToInt32(r, 16);
            int gInt = Convert.ToInt32(g, 16);
            int bInt = Convert.ToInt32(b, 16);
            return "rgba(" + rInt + "," + gInt + "," + bInt + ",0.5)";
        }

        public async Task<bool> EditTaskCategory(TaskCategory category)
        {
            if (!categoryCheck(category))
                return false;
            category.RgbaColor = HexToRgba(category.RgbaColor);
            TaskManagementContext.TaskCategories.Update(category);
            int changes = await TaskManagementContext.SaveChangesAsync();
            if (changes != 1)
                return false;
            return true;
        }

        public async Task<bool> DeleteTaskCategory(Guid Id)
        {
            var category = await TaskManagementContext.TaskCategories.FindAsync(Id);
            if (category == null)
                throw new KeyNotFoundException("Category cannot be found!");
            TaskManagementContext.TaskCategories.Remove(category);
            int changes = await TaskManagementContext.SaveChangesAsync();
            if (changes != 1)
                return false;
            return true;
        }

        public Report GenerateReport(IEnumerable<TaskCategory> categories)
        {
            Report report = new Report();
            foreach (var category in categories)
            {
                if (category.Effort == 1)
                    report.LowEffort++;
                else if (category.Effort == 1.25)
                    report.MediumLowEffort++;
                else if (category.Effort == 1.5)
                    report.MediumEffort++;
                else if (category.Effort == 1.75)
                    report.MediumHighEffort++;
                else if(category.Effort == 2)
                    report.HighEffort++;
            }
            var length = categories.Count();
            report.LowEffort = (report.LowEffort / length) * 100;
            report.MediumLowEffort = (report.MediumLowEffort / length) * 100;
            report.MediumEffort = (report.MediumEffort / length) * 100;
            report.MediumHighEffort = (report.MediumHighEffort / length) * 100;
            report.HighEffort = (report.HighEffort / length) * 100;

            return report;
        }
    }
}
