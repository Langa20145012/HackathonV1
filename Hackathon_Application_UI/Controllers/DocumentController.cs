using Microsoft.AspNetCore.Mvc;

namespace Hackathon_Application_UI.Controllers
{
    using System.Threading.Tasks;
    using Hackathon_Application_UI.Interface;
    using Hackathon_Application_UI.Models;
    using Microsoft.AspNetCore.Mvc;
    namespace MvcApp.Controllers
    {

        public class DocumentController : Controller

        {

            private readonly IDocumentService _documentService;

            private readonly IMatterService _matterService;

            public DocumentController(IDocumentService documentService, IMatterService matterService)

            {

                _documentService = documentService;

                _matterService = matterService;

            }

            // GET: Document/MatterDocuments/5

            public async Task<IActionResult> MatterDocuments(int matterId)

            {

                var matter = await _matterService.GetMatterByIdAsync(matterId);

                if (matter == null)

                    return NotFound();

                ViewBag.MatterId = matterId;

                ViewBag.MatterTitle = matter.Title;

                var documents = await _documentService.GetDocumentsByMatterIdAsync(matterId);

                return View(documents);

            }

            // GET: Document/Details/5

            public async Task<IActionResult> Details(int id)

            {

                var document = await _documentService.GetDocumentByIdAsync(id);

                if (document == null)

                    return NotFound();

                return View(document);

            }

            // GET: Document/Upload/5

            public async Task<IActionResult> Upload(int matterId)

            {

                var matter = await _matterService.GetMatterByIdAsync(matterId);

                if (matter == null)

                    return NotFound();

                ViewBag.MatterId = matterId;

                ViewBag.MatterTitle = matter.Title;

                var model = new DocumentUploadModel { MatterId = matterId };

                return View(model);

            }

            // POST: Document/Upload

            [HttpPost]

            [ValidateAntiForgeryToken]

            public async Task<IActionResult> Upload(DocumentUploadModel model)

            {

                if (ModelState.IsValid)

                {

                    await _documentService.UploadDocumentAsync(model.MatterId, model.File, model.Status);

                    return RedirectToAction(nameof(MatterDocuments), new { matterId = model.MatterId });

                }

                var matter = await _matterService.GetMatterByIdAsync(model.MatterId);

                ViewBag.MatterId = model.MatterId;

                ViewBag.MatterTitle = matter.Title;

                return View(model);

            }

            // GET: Document/UpdateStatus/5

            public async Task<IActionResult> UpdateStatus(int id)

            {

                var document = await _documentService.GetDocumentByIdAsync(id);

                if (document == null)

                    return NotFound();

                var model = new DocumentStatusUpdateModel

                {

                    DocumentId = document.DocumentId,

                    Status = document.Status

                };

                return View(model);

            }

            // POST: Document/UpdateStatus

            [HttpPost]

            [ValidateAntiForgeryToken]

            public async Task<IActionResult> UpdateStatus(DocumentStatusUpdateModel model)

            {

                if (ModelState.IsValid)

                {

                    var document = await _documentService.UpdateDocumentStatusAsync(model.DocumentId, model.Status);

                    if (document == null)

                        return NotFound();

                    return RedirectToAction(nameof(MatterDocuments), new { matterId = document.MatterId });

                }

                return View(model);

            }

            // GET: Document/Delete/5

            public async Task<IActionResult> Delete(int id)

            {

                var document = await _documentService.GetDocumentByIdAsync(id);

                if (document == null)

                    return NotFound();

                return View(document);

            }

            // POST: Document/Delete/5

            [HttpPost, ActionName("Delete")]

            [ValidateAntiForgeryToken]

            public async Task<IActionResult> DeleteConfirmed(int id)

            {

                var document = await _documentService.GetDocumentByIdAsync(id);

                if (document == null)

                    return NotFound();

                await _documentService.DeleteDocumentAsync(id);

                return RedirectToAction(nameof(MatterDocuments), new { matterId = document.MatterId });

            }

            // GET: Document/Download/5

            public async Task<IActionResult> Download(int id)

            {

                var document = await _documentService.GetDocumentByIdAsync(id);

                if (document == null)

                    return NotFound();

                var fileContent = await _documentService.DownloadDocumentAsync(id);

                if (fileContent == null)

                    return NotFound();

                return File(fileContent, document.ContentType, document.FileName);

            }

        }

    }
}

 