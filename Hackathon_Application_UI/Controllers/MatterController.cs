using Microsoft.AspNetCore.Mvc;

namespace Hackathon_Application_UI.Controllers
{
    using System.Threading.Tasks;
    using Hackathon_Application_UI.Interface;
    using Hackathon_Application_UI.Models;
    using Microsoft.AspNetCore.Mvc;

    namespace MvcApp.Controllers

    {

        public class MatterController : Controller

        {

            private readonly IMatterService _matterService;

            public MatterController(IMatterService matterService)

            {

                _matterService = matterService;

            }

            // GET: Matter

            public async Task<IActionResult> Index()

            {

                var matters = await _matterService.GetAllMattersAsync();

                return View(matters);

            }

            // GET: Matter/Details/5

            public async Task<IActionResult> Details(int id)

            {

                var matter = await _matterService.GetMatterByIdAsync(id);

                if (matter == null)

                    return NotFound();

                return View(matter);

            }

            // GET: Matter/Create

            public IActionResult Create()

            {

                return View();

            }

            // POST: Matter/Create

            [HttpPost]

            [ValidateAntiForgeryToken]

            public async Task<IActionResult> Create([Bind("Title,Description,ClientId,ClientEmail,Status")] Matter matter)

            {

                if (ModelState.IsValid)

                {

                    await _matterService.CreateMatterAsync(matter);

                    return RedirectToAction(nameof(Index));

                }

                return View(matter);

            }

            // GET: Matter/Edit/5

            public async Task<IActionResult> Edit(int id)

            {

                var matter = await _matterService.GetMatterByIdAsync(id);

                if (matter == null)

                    return NotFound();

                return View(matter);

            }

            // POST: Matter/Edit/5

            [HttpPost]

            [ValidateAntiForgeryToken]

            public async Task<IActionResult> Edit(int id, [Bind("MatterId,Title,Description,ClientId,ClientEmail,Status")] Matter matter)

            {

                if (id != matter.MatterId)

                    return NotFound();

                if (ModelState.IsValid)

                {

                    var updatedMatter = await _matterService.UpdateMatterAsync(matter);

                    if (updatedMatter == null)

                        return NotFound();

                    return RedirectToAction(nameof(Index));

                }

                return View(matter);

            }

            // GET: Matter/Delete/5

            public async Task<IActionResult> Delete(int id)

            {

                var matter = await _matterService.GetMatterByIdAsync(id);

                if (matter == null)

                    return NotFound();

                return View(matter);

            }

            // POST: Matter/Delete/5

            [HttpPost, ActionName("Delete")]

            [ValidateAntiForgeryToken]

            public async Task<IActionResult> DeleteConfirmed(int id)

            {

                await _matterService.DeleteMatterAsync(id);

                return RedirectToAction(nameof(Index));

            }

        }

    }
}

 