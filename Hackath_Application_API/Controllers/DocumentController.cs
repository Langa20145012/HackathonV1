using System.Threading.Tasks;
using Hackath_Application_API.Interfaces;
using Hackathon_Application_Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet("matter/{matterId}")]
        public async Task<IActionResult> GetDocumentsByMatterId(int matterId)
        {
            var documents = await _documentService.GetDocumentsByMatterIdAsync(matterId);
            return Ok(documents);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentById(int id)
        {
            var document = await _documentService.GetDocumentByIdAsync(id);

            if (document == null)
                return NotFound();

            return Ok(document);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDocument([FromBody] Document document)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdDocument = await _documentService.CreateDocumentAsync(document);
            return CreatedAtAction(nameof(GetDocumentById), new { id = createdDocument.DocumentId }, createdDocument);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(int id, [FromBody] Document document)
        {
            if (id != document.DocumentId)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedDocument = await _documentService.UpdateDocumentAsync(document);

            if (updatedDocument == null)
                return NotFound();

            return Ok(updatedDocument);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateDocumentStatus(int id, [FromBody] string status)
        {
            var updatedDocument = await _documentService.UpdateDocumentStatusAsync(id, status);

            if (updatedDocument == null)
                return NotFound();

            return Ok(updatedDocument);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var result = await _documentService.DeleteDocumentAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
