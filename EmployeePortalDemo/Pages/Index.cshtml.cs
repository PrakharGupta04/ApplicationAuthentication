using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeePortalDemo.Pages
{
    public class IndexModel : PageModel
    {
		public string PortalName { get; set; } = "Employee Portal";
		public void OnGet()
        {

		}
    }
}
