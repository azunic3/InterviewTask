using System.ComponentModel.DataAnnotations;

namespace InterviewTask.Dtos
{
    public class CreateAvailabilityRequestDto
    {
        public string DrugKey { get; set; } = null!;

        public string Email { get; set; } = null!;
    }
}
