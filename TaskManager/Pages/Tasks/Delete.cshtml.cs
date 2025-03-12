using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Pages.Tasks
{
    public class DeleteModel : PageModel
    {
        private readonly IDataService _dataService;

        [BindProperty]
        public TaskItem Task { get; set; } = new() { Name = string.Empty };

        public DeleteModel(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _dataService.GetTaskByIdAsync(id.Value);
            if (task == null)
            {
                return NotFound();
            }

            Task = task;

            var users = await _dataService.GetUsersAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _dataService.DeleteTaskAsync(Task.Id);
            return RedirectToPage("./Index");
        }
    }
}
