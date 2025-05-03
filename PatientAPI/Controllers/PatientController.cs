using BusinessLogic.Managers;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Models;

namespace PatientAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly PatientManager _manager;

        public PatientsController()
        {
            // Ruta relativa al archivo de pacientes
            _manager = new PatientManager("patients.txt");
        }

        // GET: api/patients
        [HttpGet]
        public ActionResult<List<Patient>> GetAll()
        {
            return Ok(_manager.GetAllPatients());
        }

        // GET: api/patients/{ci}
        [HttpGet("{ci}")]
        public ActionResult<Patient> GetByCI(string ci)
        {
            var patient = _manager.GetPatientByCI(ci);
            if (patient == null)
                return NotFound("Patient not found.");

            return Ok(patient);
        }

        // POST: api/patients
        [HttpPost]
        public ActionResult Add([FromBody] Patient patient)
        {
            try
            {
                _manager.AddPatient(patient);
                return CreatedAtAction(nameof(GetByCI), new { ci = patient.CI }, patient);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/patients/{ci}
        [HttpPut("{ci}")]
        public ActionResult Update(string ci, [FromBody] Patient updatedData)
        {
            var success = _manager.UpdatePatient(ci, updatedData.Name, updatedData.LastName);
            if (!success)
                return NotFound("Patient not found.");

            return NoContent();
        }

        // DELETE: api/patients/{ci}
        [HttpDelete("{ci}")]
        public ActionResult Delete(string ci)
        {
            var success = _manager.DeletePatient(ci);
            if (!success)
                return NotFound("Patient not found.");

            return NoContent();
        }

        [HttpPost]
        [Route("assign-gift")]
        public ActionResult<Electronic> GetGift([FromBody] Patient patient)
        {
            // Puedes reutilizar el _manager si es la misma instancia de PatientManager
            if (patient == null)
                return BadRequest("Patient data is required.");

            var gift = _manager.AssignAGiftForStudent(patient);

            if (gift == null || string.IsNullOrEmpty(gift.Name))
                return NotFound("No gift could be assigned.");

            return Ok(gift);
        }

    }
}
