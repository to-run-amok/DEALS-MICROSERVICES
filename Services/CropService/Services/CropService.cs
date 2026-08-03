public class Cropservice : ICropService
{
    private readonly ICropRepository _cropRepository;
    private readonly RabbitMqPublisher _publisher;
    public Cropservice(ICropRepository cropRepository,RabbitMqPublisher publisher)
    {
        _cropRepository = cropRepository;
        _publisher = publisher;
    }

    public async Task<CropResponseDto> CreateAsync(CropCreateDto dto,int farmerId)
    {
        var newCrop = new Crop
        {
            Type = dto.Type,
            Name = dto.Name,
            Quantity = dto.Quantity,
            Location = dto.Location,
            Status = CropStatus.Avaliable,
            PostedAt = DateTime.UtcNow,
            FarmerId = farmerId,
            AgreedPrice = null,
        };

        await _cropRepository.AddAsync(newCrop);
        await _cropRepository.SaveChangesAsync();

        await _publisher.PublishAsync("crop-created",
            new CropCreatedEvent
            {
                CropId = newCrop.Id,
                CropType = newCrop.Type,
                CropName = newCrop.Name,
                FarmerId = newCrop.FarmerId
            });

        return MapResponseDto(newCrop);
    }
    public async Task<CropResponseDto> GetByIdAsync(int id)
    {
        var crop = await _cropRepository.GetByIdAsync(id);

        if(crop==null)
        {
            throw new KeyNotFoundException("Crop with this id doesnt exist.");
        }

        return MapResponseDto(crop);

    }
    public async Task<IEnumerable<CropResponseDto>> GetAllAsync()
    {
        var crops = await _cropRepository.GetAllAsync();

        return crops.Select(MapResponseDto);
    }

    public async Task<IEnumerable<CropResponseDto>> GetMyCropsAsync(int farmerId)
    {
        var crops = await _cropRepository.GetByFarmerIdAsync(farmerId);

        return crops.Select(MapResponseDto);
    }
    public async Task<CropResponseDto> UpdateAsync(int cropId,int farmerId,CropUpdateDto dto)
    {
        var existingCrop = await _cropRepository.GetByIdAsync(cropId);

        if(existingCrop==null)
        {
            throw new KeyNotFoundException("Crop listing not found.");
        }
        if(existingCrop.FarmerId != farmerId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this listing");
        }
        if(existingCrop.Status == CropStatus.Sold)
        {
            throw new InvalidOperationException("Sold crops cannot be modified.");
        }

        if(!string.IsNullOrWhiteSpace(dto.Type)) existingCrop.Type = dto.Type.Trim();
        else existingCrop.Type = existingCrop.Type;

        if(!string.IsNullOrWhiteSpace(dto.Name)) existingCrop.Name = dto.Name.Trim();
        else existingCrop.Name = existingCrop.Name;

        if(!string.IsNullOrWhiteSpace(dto.Location)) existingCrop.Location = dto.Location.Trim();
        else existingCrop.Location = existingCrop.Location;

        if(dto.Quantity.HasValue)
        {
            if(dto.Quantity.Value <= 0)
            {
                throw new InvalidOperationException("Qunatity must be greater than zero.");
            }

            existingCrop.Quantity = dto.Quantity.Value;
        }
        else existingCrop.Quantity = existingCrop.Quantity;

        await _cropRepository.SaveChangesAsync();

        return MapResponseDto(existingCrop);
    }

    public async Task DeleteAsync(int cropId,int farmerId)
    {
        var crop = await _cropRepository.GetByIdAsync(cropId);

        if(crop==null)
        {
            throw new KeyNotFoundException("Crop listing not found.");
        }
        if(crop.FarmerId != farmerId)
        {
            throw new UnauthorizedAccessException("you are not authorized to delete this crop.");
        }
        if(crop.Status == CropStatus.Sold)
        {
            throw new InvalidOperationException("Sold crops cannot be deleted.");
        }

        _cropRepository.Delete(crop);
        await _cropRepository.SaveChangesAsync();
    }

    public async Task MarkAsSoldAsync(int cropId)
    {
        var crop = await _cropRepository.GetByIdAsync(cropId);

        if(crop == null)
            throw new KeyNotFoundException();

        crop.Status = CropStatus.Sold;
        await _cropRepository.SaveChangesAsync();
    }
    private static CropResponseDto MapResponseDto(Crop crop)
    {
        return new CropResponseDto
        {
            Id = crop.Id,
            Type = crop.Type,
            Name = crop.Name,
            Quantity = crop.Quantity,
            Location = crop.Location,
            Status = crop.Status.ToString(),
            PostedAt = crop.PostedAt,
            FarmerId = crop.FarmerId
        };
    }
}