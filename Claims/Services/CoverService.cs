using Claims.Auditing;
using Claims.Premium;
using Claims.Repositories;
using Claims.Validation;

namespace Claims.Services
{
    public class CoverService : ICoverService
    {

        private readonly ICoverRepository _repository;
        private readonly IPremiumCalculator _premiumCalculator;
        private readonly IAuditer _auditer;

        private readonly ICoverValidator _validator;

        public CoverService(ICoverRepository repository, IPremiumCalculator premiumCalculator, IAuditer auditer, ICoverValidator validator)
        {
            _repository = repository;
            _premiumCalculator = premiumCalculator;
            _auditer = auditer;
            _validator = validator;
        }

        public Task<IEnumerable<Cover>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Cover?> GetByIdAsync(string id)
        {
            return _repository.GetByIdAsync(id);
        }

        public async Task<Cover> CreateAsync(Cover cover)
        {

            var errors = _validator.Validate(cover);
            if (errors.Count > 0) throw new ValidationException(errors);

            cover.Id = Guid.NewGuid().ToString();
            cover.Premium = _premiumCalculator.Calculate(cover.Type,cover.StartDate, cover.EndDate);
            await _repository.AddAsync(cover);
            _auditer.AuditCover(cover.Id, "POST");
            return cover;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (deleted)
            {
                _auditer.AuditCover(id, "DELETE");
            }
            return deleted;
        }

        public decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType)
        {
            return _premiumCalculator.Calculate(coverType, startDate, endDate);
        }
    }
}
