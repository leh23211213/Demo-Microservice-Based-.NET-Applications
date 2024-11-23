using App.Services.AuthAPI.Data;
using App.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
namespace App.Services.AuthAPI.Services
{
    public interface IRegisterAPIService
    {
        Task<string> Register(RegistrationRequest RegistrationRequest);
    }

    public class RegisterAPIService : IRegisterAPIService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public RegisterAPIService(
                                ApplicationDbContext dbContext,
                                RoleManager<IdentityRole> roleManager,
                                UserManager<ApplicationUser> userManager
                            )
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> Register(RegistrationRequest registrationRequest)
        {
            if (!IsUniqueUser(registrationRequest.Email)) return "User all ready exists";

            ApplicationUser applicationUser = new()
            {
                Id = GenerateId().ToString(),
                UserName = registrationRequest.Email,
                Email = registrationRequest.Email,
                NormalizedEmail = registrationRequest.Email.ToUpper(),
                Name = registrationRequest.Name,
                PhoneNumber = registrationRequest.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, registrationRequest.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(registrationRequest.Role).GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole(registrationRequest.Role));
                    }
                    await _userManager.AddToRoleAsync(applicationUser, registrationRequest.Role);
                    return string.Empty;
                }
                else
                {
                    return string.Join("; ", result.Errors.Select(e => e.Description)) ?? "Unknown error";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public bool IsUniqueUser(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        private const long Epoch = 1609459200000L; // 2021-01-01 00:00:00 UTC
        private const int MachineIdBits = 10;     // Số bit dành cho Machine ID
        private const int SequenceBits = 12;      // Số bit dành cho Sequence
        private const long MaxMachineId = (1L << MachineIdBits) - 1;
        private const long MaxSequence = (1L << SequenceBits) - 1;
        private readonly long _machineId;
        private long _lastTimestamp = -1L;
        private long _sequence = 0L;
        private readonly object _lock = new object();

        public long GenerateId()
        {
            lock (_lock)
            {
                long timestamp = GetCurrentTimestamp();

                if (timestamp < _lastTimestamp)
                {
                    throw new InvalidOperationException("Clock moved backwards. Refusing to generate ID.");
                }

                if (timestamp == _lastTimestamp)
                {
                    // Tăng dần Sequence trong cùng một mili giây
                    _sequence = (_sequence + 1) & MaxSequence;

                    if (_sequence == 0)
                    {
                        // Đợi đến mili giây tiếp theo
                        timestamp = WaitForNextMillis(_lastTimestamp);
                    }
                }
                else
                {
                    // Reset Sequence khi thời gian thay đổi
                    _sequence = 0L;
                }

                _lastTimestamp = timestamp;

                // Tạo ID bằng cách kết hợp các thành phần
                return ((timestamp - Epoch) << (MachineIdBits + SequenceBits)) |
                       (_machineId << SequenceBits) |
                       _sequence;
            }
        }
        private long GetCurrentTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        private long WaitForNextMillis(long lastTimestamp)
        {
            long timestamp = GetCurrentTimestamp();
            while (timestamp <= lastTimestamp)
            {
                timestamp = GetCurrentTimestamp();
            }
            return timestamp;
        }

    }
}